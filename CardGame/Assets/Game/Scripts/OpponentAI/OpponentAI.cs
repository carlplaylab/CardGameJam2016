using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class OpponentAI : MonoBehaviour
{

	[SerializeField] private int team;
	[SerializeField] private TextMesh textmesh;

	private BoardPlayer player;

	private bool activeState = false;
	private int logThinkCounter = 0;
	private int thinkingCounter = 0;
	private int think_CardId = -1;
	private Cell think_MyCell = null;
	private Cell think_TargetCell = null;
	private float think_timer = 0f;
	private string think_text;

	public void Setup ()
	{
		player = GetComponent<BoardPlayer>();
		team = player.TeamId;
	}

	public void SetTurnActive (bool active)
	{
		if(!activeState && active)
		{
			logThinkCounter = 0;
			thinkingCounter = 0;
			think_CardId = -1;
			think_timer = 0f;
			think_MyCell = null;
			think_TargetCell = null;
			think_text = "";
		}
		activeState = active;

		GenerateRandomElements();
	}


	void GenerateRandomElements ()
	{
		for(int i=0; i < 5; i++)
		{
			// 60% chance of getting land elements than water elements
			ElementType randType = (UnityEngine.Random.Range(0, 99) % 3 < 2) ? ElementType.LAND : ElementType.WATER;
			player.IngameData.ResourceData.AddResource(randType,1);
		}
	}

	void Update ()
	{
		if(activeState)
		{
			UpdateThinking();
		}
	}


	void UpdateThinking ()
	{
		if(logThinkCounter != thinkingCounter)
		{
			Debug.Log("UpdateThinking " + thinkingCounter + ", " + think_text);
			logThinkCounter = thinkingCounter;
			textmesh.text = think_text;
		}

		if(thinkingCounter == 0)
		{
			ThinkOfCardId ();
		}
		else if(thinkingCounter == 1)
		{
			SpawnAvailableCard ();
		}
		else if(thinkingCounter == 2)
		{
			think_timer -= Time.deltaTime;
			if(think_timer <= 0f)
				thinkingCounter++;
		}
		else if(thinkingCounter == 3)
		{
			DropAvailableCard();
		}
		else if(thinkingCounter == 4)
		{
			think_timer -= Time.deltaTime;
			if(think_timer <= 0f)
				thinkingCounter++;
		}
		else if(thinkingCounter == 5)
		{
			PickACharacter ();
		}
		else if(thinkingCounter == 6)
		{
			think_timer -= Time.deltaTime;
			if(think_timer <= 0f)
				thinkingCounter++;
		}
		else if(thinkingCounter == 7)
		{
			MoveCharacter ();
		}
		else if(thinkingCounter == 9)
		{
			think_timer -= Time.deltaTime;
			if(think_timer <= 0f)
				thinkingCounter++;
		}
		else if(thinkingCounter == 10)
		{
			GameBoardManager.Instance.Board.BoardCells.RemoveHighlights();
			GameBoardManager.Instance.OnPlayerTurnEnd();
		}
	}

	// consider if a spawn card is purchaseable
	void ThinkOfCardId()
	{
		think_CardId = player.Deck.GetRandomSpawnCard();
		CardData cdata = CardDatabase.Instance.GetData(think_CardId);
		CharacterData charData = CharacterDatabase.Instance.GetData(cdata.characterId);

		bool canAfford = player.IngameData.HasEnoughResource(charData.elementType, charData.spawnCost);
		if(!canAfford)
			think_CardId = -1;

		if(think_CardId >= 0)
		{
			thinkingCounter = 1;

			think_text += "Planning to spawn " + charData.name;
		}
		else
		{
			think_timer = UnityEngine.Random.Range(1f, 2f);
			thinkingCounter = 4;

			think_text += "No characater to spawn.";
		}

	}


	void SpawnAvailableCard ()
	{
		think_timer = UnityEngine.Random.Range(1f, 2f);

		if(think_CardId < 0)
		{
			thinkingCounter = 4;
			return;
		}

		CellHandler boardCells = GameBoardManager.Instance.Board.BoardCells;

		// top area range
		int focusedRow = boardCells.Rows;
		int maxRow = Mathf.Min(focusedRow + 1, boardCells.Rows);
		int minRow = Mathf.Max(focusedRow - 1, 0);
		int maxCol = boardCells.Cols;
		int minCol = 0;

		Rect targetRect = new Rect(minCol, minRow, (maxCol - minCol), (maxRow - minRow));
		boardCells.HiglightArea(targetRect);

		thinkingCounter = 2;

		think_text += "\nhighlight target cells";
	}


	void DropAvailableCard ()
	{
		if(think_CardId < 0)
		{
			thinkingCounter = 4;
			return;
		}

		CellHandler boardCells = GameBoardManager.Instance.Board.BoardCells;
		int focusedRow = boardCells.Rows;
		int maxRow = Mathf.Min(focusedRow + 1, boardCells.Rows);
		int minRow = Mathf.Max(focusedRow - 1, 0);
		int maxCol = boardCells.Cols;
		int minCol = 0;

		Rect targetRect = new Rect(minCol, minRow, (maxCol - minCol), (maxRow - minRow));
		Cell randomCell = boardCells.GetRandomVacantCell(targetRect);
		CardData cdata = CardDatabase.Instance.GetData(think_CardId);
		CharacterData charData = CharacterDatabase.Instance.GetData(cdata.characterId);

		if(randomCell != null && charData != null)
		{
			GameCharacter newCharacater = CharacterHandler.Instance.CreateCharacterOnCell(cdata.characterId, randomCell);
			if(newCharacater != null)
			{
				newCharacater.SetTeam(team);
				player.IngameData.SpendResource(charData.elementType, charData.spawnCost);
				player.Deck.CardUsed(think_CardId);

				think_text += "\ndropping new character";
				think_timer = 1f;
				thinkingCounter = 9;

				return;
			}
		}

		think_text += "\nCannot drop new character";
		thinkingCounter = 4;
	}


	void PickACharacter ()
	{
		think_text += "\npicking a character";

		think_MyCell = null;
		think_TargetCell = null;
		think_timer = 0.1f;
		thinkingCounter = 6;

		GameCharacter soldier = player.Team.GetRandomCharacter();
		if(soldier == null)
		{
			think_text += "\nno solider found";
			return;
		}

		CellHandler boardCells = GameBoardManager.Instance.Board.BoardCells;
		think_MyCell = boardCells.GetCell( soldier.cellId );
		if(think_MyCell == null)
		{
			think_text += "\nno cell found";
			return;
		}

		int maxRow = Mathf.Min(think_MyCell.row + soldier.GetMovement(), boardCells.Rows);
		int minRow = Mathf.Max(think_MyCell.row - soldier.GetMovement(), 0);
		int maxCol = Mathf.Min(think_MyCell.col + soldier.GetMovement(), boardCells.Cols);
		int minCol = Mathf.Max(think_MyCell.col - soldier.GetMovement(), 0);
		Rect targetRect = new Rect(minCol, minRow, (maxCol - minCol), (maxRow - minRow));
		boardCells.HiglightArea(targetRect);

		List<Cell> targetCells = boardCells.GetCellsInArea(targetRect);
		for(int i=0; i < targetCells.Count; i++)
		{
			if(!targetCells[i].IsVacant())
			{
				GameCharacter resider = targetCells[i].ResidingObject.GetComponent<GameCharacter>();
				if(resider != null && resider.Team != team)
				{
					think_TargetCell = targetCells[i];
					break;
				}
			}
		}

		if( think_TargetCell == null )
		{
			GameCharacter target = GameBoardManager.Instance.Board.GetPlayer(1).Team.GetNearestCharacter(soldier, boardCells);
			if(target != null)
			{
				think_TargetCell = boardCells.GetCell(target.cellId);
			}
			else
			{
				think_TargetCell = boardCells.GetRandomVacantCell(targetRect);
			}
		}

		think_timer = 1f;

		think_text += "\nchose " + soldier.name;
	}


	void MoveCharacter ()
	{
		think_timer = 1f;
		thinkingCounter = 9;
		if(think_MyCell == null || think_TargetCell == null)
		{
			think_text += "\ncannot move character";
			return;
		}

		bool moveSuccess = GameBoardManager.Instance.Board.DropObjectOnCell(think_MyCell.ResidingObject, think_TargetCell);
			
		think_text += "\nmoved to target success: " + moveSuccess;
		think_timer = 2f;
	}
}
