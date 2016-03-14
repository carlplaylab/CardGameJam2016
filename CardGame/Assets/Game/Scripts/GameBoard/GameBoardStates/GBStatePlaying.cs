using UnityEngine;
using System.Collections;


public enum PlayState
{
	ELEMENT_TAKING = 0,
	IDLE = 1,
	CARD_DRAG = 2
}

public class GBStatePlaying : GBState 
{

	int charId = 1;
	Cell hoveredCell;
	PlayState playState;


	public override BoardState GetState()
	{
		return BoardState.PLAYER_TURN;
	}

	public override void Start (GameBoard board)
	{
		board.InputEnable = true;
		hoveredCell = null;
		playState = PlayState.ELEMENT_TAKING;

		board.BoardCells.SetCellsColliderActive(false);
		GameBoardManager.Instance.ElementUI.Show();
		GameBoardManager.Instance.ElementUI.onElementsTaken = ElementsTaken;
	}

	public override void Update (GameBoard board)
	{
		if(Input.GetKeyDown(KeyCode.C))
		{
			Parameters charParams = new Parameters();
			charParams.PutExtra("character", charId);
			EventBroadcaster.Instance.PostEvent(EventNames.UI_ADD_CHARACTER_CARD, charParams);
			charId ++;
			if(charId >= CharacterDatabase.Instance.Count())
				charId = 1;
		}

		if(playState == PlayState.CARD_DRAG)
		{
			Cell newHoveredCell = board.BoardCells.GetHoveredCell();
			if(hoveredCell != null)
			{
				hoveredCell.HighlightCell(false);
			}
			if(hoveredCell != newHoveredCell && newHoveredCell != null)
			{
				hoveredCell = newHoveredCell;
				hoveredCell.HighlightCell(true, hoveredCell.IsVacant());
			}

			newHoveredCell = newHoveredCell;
		}
	}

	public override void End (GameBoard board)
	{
		board.InputEnable = false;
		hoveredCell = null;
	}


	#region CharacterFunctions

	public override void StartDragCardOnBoard (GameBoard board, int charID)
	{
		CharacterData data = CharacterDatabase.Instance.GetData(charId);
		if(data == null)
			return;
		
		int focusedRow = 0;
		if(GameBoardManager.Instance.CurrentTeam == 2)
			focusedRow = board.BoardCells.Rows;

		int maxRow = Mathf.Min(focusedRow + 1, board.BoardCells.Rows);
		int minRow = Mathf.Max(focusedRow - 1, 0);

		int maxCol = board.BoardCells.Cols;
		int minCol = 0;

		Rect targetRect = new Rect(minCol, minRow, (maxCol - minCol), (maxRow - minRow));
		board.BoardCells.HiglightArea(targetRect);

	    hoveredCell = board.GetHoveredCell();	// instantly checks cell using gameInput
		if(hoveredCell != null)
			hoveredCell.HighlightCell(true, hoveredCell.IsVacant() );

		playState = PlayState.CARD_DRAG;

	}

	public override bool EndDragCardOnBoard (GameBoard board, int cardCharacterID)
	{
		bool cardConvertSuccess = false;
		board.BoardCells.RemoveHighlights();
		playState = PlayState.IDLE;

		Cell newHoveredCell = board.GetHoveredCell(); // instantly checks cell using gameInput
		if(hoveredCell != null)
		{
			if(newHoveredCell == null || newHoveredCell != hoveredCell)
			{
				hoveredCell.HighlightCell(false);
			}
		}

		CharacterData charData = CharacterDatabase.Instance.GetData(cardCharacterID);
		if(cardCharacterID != charData.id)
		{
			Debug.LogWarning("WRONG DATA charId " + cardCharacterID  + " charData.id " + charData.id );
		}
		hoveredCell = newHoveredCell;
		PlayerIngameData playerData = IngameDataCenter.Instance.GetPlayerData(GameBoardManager.Instance.CurrentTeam);
		bool canAfford = playerData.HasEnoughResource(charData.elementType, charData.spawnCost);
		if(!canAfford)
		{
			Debug.Log("Cant afford character");
		}

		if(hoveredCell != null && 
			charData != null &&
			playerData != null &&
			hoveredCell.IsVacant() && 
			canAfford )
		{
			GameCharacter newCharacater = CharacterHandler.Instance.CreateCharacterOnCell(cardCharacterID, hoveredCell);
			if(newCharacater != null)
			{
				playerData.SpendResource(charData.elementType, charData.spawnCost);
				cardConvertSuccess = true;
			}
		}
		hoveredCell = null;

		return cardConvertSuccess;
	}

	#endregion


	public void ElementsTaken ()
	{
		ElementType[] elements = GameBoardManager.Instance.ElementUI.GetElements();
		PlayerIngameData playerData = IngameDataCenter.Instance.GetPlayerData(GameBoardManager.Instance.CurrentTeam);
		for(int i=0; i < elements.Length; i++)
		{
			playerData.ResourceData.AddResource(elements[i], 1);
		}
		playerData.ResourceData.UpdateUI();
		GameBoardManager.Instance.Board.BoardCells.SetCellsColliderActive(true);
	}
		
}
