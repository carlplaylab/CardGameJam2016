using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[SerializeField]
public class BoardPlayer : MonoBehaviour 
{
	[SerializeField] private int team;
	[SerializeField] private CharacterTeam characterTeam;
	[SerializeField] private CardDeck cardDeck;

	private bool turnActive = false;
	private OpponentAI brain;


	public int TeamId
	{
		get { return team; }
	}

	public bool TurnActive
	{
		get { return turnActive; }
	}

	public CharacterTeam Team
	{
		get { return characterTeam; }
	}

	public CardDeck Deck
	{
		get { return cardDeck; }
	}

	public PlayerIngameData IngameData
	{
		get { return IngameDataCenter.Instance.GetPlayerData(TeamId); }
	}

	public void Setup(int teamNumber, int level, int resourceCount)
	{
		team = teamNumber;
		characterTeam = new CharacterTeam(teamNumber);
		cardDeck = new CardDeck();
		cardDeck.SetupDeck(level, teamNumber);

		brain = GetComponent<OpponentAI>();
		if(brain != null)
			brain.Setup();

		IngameData.ResourceData.AddResource(ElementType.LAND, resourceCount);
	}

	public void SetInitialCards()
	{
		int cardAmount = CardDeck.SPAWN_LIMIT;
		if(TeamId == 1)
		{
			Parameters cardparams = new Parameters();
			cardparams.PutExtra("card", -1);
			cardparams.PutExtra("cardamount", cardAmount);
			for(int i=0; i < cardAmount; i++)
			{
				int newCardId = Deck.GetCard();
				cardparams.PutExtra("card_" + i, newCardId);
			}
			EventBroadcaster.Instance.PostEvent(EventNames.UI_ADD_CARD_TO_DECK, cardparams);
		}
		else
		{
			for(int i=0; i < cardAmount; i++)
			{
				Deck.GetCard();
			}
		}
	}

	public void SetTurn(int teamNumber)
	{
		turnActive = teamNumber == TeamId;

		if(turnActive)
		{
			int cardAmount = Deck.GetCardsNeeded();
			//Debug.Log("TeamId " + TeamId + " GetCardsNeeded " + cardAmount);

			if(TeamId == 1)
			{
				Parameters cardparams = new Parameters();
				cardparams.PutExtra("card", -1);
				cardparams.PutExtra("cardamount", cardAmount);

				for(int i=0; i < cardAmount; i++)
				{
					int newCardId = Deck.GetCard();
					cardparams.PutExtra("card_" + i, newCardId);
				}
				EventBroadcaster.Instance.PostEvent(EventNames.UI_ADD_CARD_TO_DECK, cardparams);
			}
			else
			{
				for(int i=0; i < cardAmount; i++)
				{
					int newCardId = Deck.GetCard();
				}
			}
		}

		if(brain != null)
			brain.SetTurnActive(turnActive);
	}

	public GameCharacter CreateCharacter(int cardId, Cell targetCell)
	{
		if( targetCell == null )
		{
			Debug.Log("NULL target cell" );
			return null;
		}
		
		CardData cdata = CardDatabase.Instance.GetData(cardId);
		if(	cdata == null || 
			cdata.cardType != CardType.CHARACTER)
		{
			return null;
		}

		int cardCharacterID = cdata.dataId;
		if( targetCell == null || 
			!targetCell.IsVacant() || 
			!CheckAffordCharacter(cardCharacterID) )
		{
			Debug.Log("not vacant " + (!targetCell.IsVacant()) );
			Debug.Log("can afford " + CheckAffordCharacter(cardCharacterID));
			return null;
		}

		GameCharacter newCharacter = CharacterHandler.Instance.CreateCharacterOnCell(cardCharacterID, targetCell);
		if(newCharacter == null)
			return null;

		CharacterData charData = CharacterDatabase.Instance.GetData(cardCharacterID);
		IngameData.SpendResource(charData.elementType, charData.spawnCost);
		Team.AddGameCharacter(newCharacter);

		newCharacter.SetTeam(TeamId);
		Deck.CardUsed(cardId);

		SoundManager.PlaySound("newcharacter");

		return newCharacter;

	}

	public bool CheckAffordCharacter(int cardCharacterID)
	{
		CharacterData charData = CharacterDatabase.Instance.GetData(cardCharacterID);
		if(charData == null)
		{
			Debug.Log("no character data");
			return false;
		}
		return IngameData.HasEnoughResource(charData.elementType, charData.spawnCost);
	}

	public bool HighlightAreaAroundCharacters(GameBoard board, int characterId)
	{
		bool hasHighlight = false;
		List<GameCharacter> targets = Team.GetCharactersWithID(characterId);
		if(targets.Count > 0)
		{
			hasHighlight = true;
			CharacterData charData = CharacterDatabase.Instance.GetData(characterId);
			int movement = charData.movement;
			for(int i=0; i < targets.Count; i++)
			{
				Cell charCell = board.BoardCells.GetCell(targets[i].cellId);
				if(charCell != null)
				{
					board.BoardCells.HighlightAround(charCell, movement);
				}
			}
		}
		return hasHighlight;
	}


	public bool CanAffordSkill(SkillData skill)
	{
		int cost = skill.cost;
		return IngameData.HasEnoughResource(skill.elementType, skill.cost);
	}

	public bool DropSkillOnBoard(GameBoard board, Cell targetCell, SkillData skill)
	{
		if(board == null || skill == null)
			return false;

		if( !CanAffordSkill(skill) )
			return false;

		List<int> skilledCharacterIDs = CharacterDatabase.Instance.GetCharacterThatUsesSkill(skill.id);
		List<GameCharacter> usableCharacters = new List<GameCharacter>();
		for(int i=0; i < skilledCharacterIDs.Count; i++)
		{
			int charID = skilledCharacterIDs[i];
			List<GameCharacter> foundChars = Team.GetCharactersWithID(charID);
			for(int f=0; f < foundChars.Count; f++)
			{
				usableCharacters.Add( foundChars[f] );
			}
		}

		int targetCellId = targetCell.id;
		GameCharacter focusedCharacter = null;
		for(int i=0; i < usableCharacters.Count; i++)
		{
			GameCharacter gchar = usableCharacters[i];
			if(board.BoardCells.CheckIfWithinRange(targetCellId, gchar.cellId, gchar.GetMovement()))
			{
				focusedCharacter = gchar;
				break;
			}
		}

		if(focusedCharacter == null)
		{
			return false; //  skill not released on proper area
		}

		if(focusedCharacter.cellId == targetCellId && 
			skill.areaCol == 1 && 
			skill.areaRow == 1)
		{
			return false;	// targetted its own player
		}

		// Use Skill
		IngameData.SpendResource(skill.elementType, skill.cost);

		return true;
	}


	public void HitPlayersFromSkill (GameBoard board, Cell targetCell, SkillData skill)
	{
		Rect targetRect = new Rect();
		targetRect.yMin = Mathf.Max( targetCell.row - skill.areaRow, 0);
		targetRect.yMax = Mathf.Min( targetCell.row + skill.areaRow, board.BoardCells.Rows);
		targetRect.xMin = Mathf.Max( targetCell.col - skill.areaCol, 0);
		targetRect.xMax = Mathf.Min( targetCell.col + skill.areaCol, board.BoardCells.Cols);

		List<int> cells = board.BoardCells.GetCellIdsInArea(targetRect);
		List<GameCharacter> targets = Team.GetCharactersInCells(cells);
		int damage = skill.damage;
		bool hitSuccess = false;
		for(int i=0; i < targets.Count; i++)
		{
			GameCharacter gc = targets[i];
			if( gc.ReceiveAttack( damage ) )
			{
				Cell deadCell = board.BoardCells.GetCell(gc.cellId);
				if(deadCell != null)
					deadCell.ResidingObject = null;
				
				Team.KillGameCharacter(gc, 0.5f);
				hitSuccess = true;
			}

			if(skill.id == 2)
				EffectsHandler.Instance.PlayEffectsAt(EffectsType.FX_ARROW, gc.transform.position);
			else
				EffectsHandler.Instance.PlayEffectsAt(EffectsType.FX_FIREBALL, gc.transform.position);
		}

		if(hitSuccess)
		{
			SoundManager.PlaySound("damage");
		}
	}

}
