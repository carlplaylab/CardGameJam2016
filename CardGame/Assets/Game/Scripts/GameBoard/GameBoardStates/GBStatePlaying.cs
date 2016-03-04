using UnityEngine;
using System.Collections;

public class GBStatePlaying : GBState 
{

	int charId = 1;

	public override BoardState GetState()
	{
		return BoardState.PLAYER_TURN;
	}

	public override void Start (GameBoard board)
	{
		board.InputEnable = true;
		Debug.Log("input enabled");
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
	}

	public override void End (GameBoard board)
	{
		board.InputEnable = false;
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
	}

	public override void EndDragCardOnBoard (GameBoard board, int charID)
	{
		board.BoardCells.RemoveHighlights();
	}

	#endregion
}
