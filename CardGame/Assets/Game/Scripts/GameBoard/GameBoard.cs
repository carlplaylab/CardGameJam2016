using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameBoard : MonoBehaviour 
{
	public const int PLAYER_TEAM = 1;
	public const int OPPONENT_TEAM = 2;

	[SerializeField] private BoardPlayer[] players;

	private GameInput input = null;
	private CellHandler cellHandler = null;
	private BoardObject selectedObject = null;
	private int currentTeam = 1;

	public Action onPlayerMoveEnded = null;
	private MoveType previousMoveType = MoveType.NONE;



	void Awake ()
	{
		input = GetComponent<GameInput>();
	}


	public void Setup ()
	{
		int level = GameBoardManager.Instance.Settings.Level;
		for(int i=0; i < players.Length; i++)
		{
			players[i].Setup(i+1, level);
		}
	}


	void Start ()
	{
		selectedObject = null;
	}


	public bool InputEnable
	{
		get { return input.Active; }
		set { input.Active = value; }
	}


	public CellHandler BoardCells
	{
		get { return cellHandler; }
		set { cellHandler = value; }
	}

	public CharacterTeam GetTeam(int teamNumber)
	{
		int teamIdx = Mathf.Clamp(teamNumber-1, 0, players.Length-1);
		return players[teamIdx].Team;
	}

	public CharacterTeam GetOpposingTeam()
	{
		return GetTeam( (currentTeam+1)%(players.Length) );
	}

	public CharacterTeam GetCurrentTeam()
	{
		return GetTeam( currentTeam );
	}

	public CardDeck GetDeck(int teamNumber)
	{
		int teamIdx = Mathf.Clamp(teamNumber-1, 0, players.Length-1);
		return players[teamIdx].Deck;
	}

	public BoardPlayer GetPlayer(int teamNumber)
	{
		int teamIdx = Mathf.Clamp(teamNumber-1, 0, players.Length-1);
		return players[teamIdx];
	}

	public void CellClicked(Cell targetCell)
	{
		if(currentTeam != PLAYER_TEAM)
			return;
		
		if(selectedObject == null)
		{
			HoldObject(targetCell);
		}
		else
		{
			previousMoveType = ReleaseObject(targetCell);

			if((previousMoveType == MoveType.MOVED || 
				previousMoveType == MoveType.SPAWN )
				&& onPlayerMoveEnded != null)
			{
				onPlayerMoveEnded();
			}
		}
	}


	public void HoldObject(Cell targetCell)
	{
		if(targetCell == null || selectedObject != null || previousMoveType == MoveType.ATTACKED)
		{
			return;
		}

		if(targetCell.IsVacant() || !targetCell.ResidingObject.IsMoveable())
		{
			return;
		}

		if(targetCell.ResidingObject.IsCharacter())
		{
			GameCharacter character = (GameCharacter)targetCell.ResidingObject;
			if(character.Team != currentTeam)
			{
				return;
			}
		}

		selectedObject = targetCell.ResidingObject;
		selectedObject.OnFocus(true);
		cellHandler.HighlightAround(targetCell, selectedObject.data.movementSpace);
	}


	public MoveType ReleaseObject(Cell targetCell)
	{
		if(selectedObject == null || targetCell == null)
		{
			return MoveType.NONE;
		}

		cellHandler.RemoveHighlights();
		selectedObject.OnFocus(false);

		MoveType releaseType = DropObjectOnCell(selectedObject, targetCell);
		selectedObject = null;
		return releaseType;

		/*
		if(!targetCell.IsWalkable())
		{
			selectedObject = null;
			return releaseSuccess;
		}
		else if(targetCell.IsVacant())
		{
			
			Cell previousCell = cellHandler.GetCell( selectedObject.cellId );
			releaseSuccess = selectedObject.TransferCells(targetCell, previousCell);
			selectedObject = null;
			return releaseSuccess;
		}
		else
		{
			releaseSuccess = ProcessEncounter(targetCell);
			selectedObject = null;
			return releaseSuccess;
		}
		*/
	}


	public MoveType DropObjectOnCell(BoardObject bobj, Cell targetCell)
	{
		MoveType releaseType =  MoveType.NONE;
		if(!targetCell.IsWalkable())
		{
			return MoveType.NONE;
		}
		else if(targetCell.IsVacant())
		{

			Cell previousCell = cellHandler.GetCell( bobj.cellId );
			if(bobj.TransferCells(targetCell, previousCell))
				releaseType = MoveType.MOVED;
			return releaseType;
		}
		else
		{
			releaseType = ProcessEncounter(bobj, targetCell);
			return releaseType;
		}
	}


	public MoveType ProcessEncounter (BoardObject selObj, Cell targetCell)
	{
		if(targetCell == null || targetCell.IsVacant() || selObj == null)
			return MoveType.NONE;

		Cell previousCell = cellHandler.GetCell( selObj.cellId );
		if(!selObj.AllowMovementOnCell(targetCell, previousCell))
			return MoveType.NONE;

		if(!selObj.IsCharacter() || !targetCell.ResidingObject.IsCharacter())
			return MoveType.NONE;

		GameCharacter selectedChar = (GameCharacter)selObj;
		if(!selectedChar.CheckIfEnemy(targetCell.ResidingObject))
			return MoveType.NONE;

		selectedChar.PlayAttackFx(targetCell);
		InputEnable = false;

		return MoveType.ATTACKED;
	}


	public void AttackHit (GameCharacter attacker, Cell targetCell)
	{
		previousMoveType = MoveType.NONE;

		GameCharacter targetChar = (GameCharacter)targetCell.ResidingObject;
		bool targetDies = targetChar.ReceiveAttack( attacker.AttackStrength );
		bool selectedDies = attacker.ReceiveAttack( targetChar.AttackStrength );

		if(targetDies)
		{
			GetPlayer(targetChar.Team).Team.KillGameCharacter(targetChar);
			targetCell.ResidingObject = null;
		}

		if(selectedDies)
		{
			Cell previousCell = cellHandler.GetCell( attacker.cellId );
			GetPlayer(attacker.Team).Team.KillGameCharacter(attacker);
			previousCell.ResidingObject = null;
		}

		InputEnable = true;
		if(currentTeam == 1 && onPlayerMoveEnded != null)
		{
			onPlayerMoveEnded();
		}
	}


	public void SetTeam (int newTeam)
	{
		currentTeam = newTeam;
		CharacterHandler.Instance.SetTeam( newTeam );

		for(int i=0; i < players.Length; i++)
		{
			players[i].SetTurn(newTeam);
		}
	}

	// instantly checks cell using gameInput
	public Cell GetHoveredCell ()
	{
		GameObject go = input.GetHoveredOjbect();
		if(go != null)
		{
			Cell cell = go.GetComponent<Cell>();
			return cell;
		}
		return null;
	}

}
