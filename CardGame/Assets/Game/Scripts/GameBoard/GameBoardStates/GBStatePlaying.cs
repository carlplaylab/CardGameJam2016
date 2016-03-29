using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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

			hoveredCell = newHoveredCell;
		}
	}

	public override void End (GameBoard board)
	{
		board.InputEnable = false;
		hoveredCell = null;
	}


	#region CharacterFunctions

	public override void StartDragCardOnBoard (GameBoard board, int cardId)
	{
		CardData cdata = CardDatabase.Instance.GetData(cardId);
		if(cdata == null)
			return ;

		if(cdata.cardType == CardType.CHARACTER)
			DragCharacterOnBoard(board, cdata.dataId);
		else
			DragSkillOnBoard(board, cdata.dataId);
	}

	public override bool EndDragCardOnBoard (GameBoard board, int cardId)
	{
		CardData cdata = CardDatabase.Instance.GetData(cardId);
		if(cdata == null)
			return false;

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
		hoveredCell = newHoveredCell;

		if(hoveredCell == null)
			return false;

		BoardPlayer player = board.GetPlayer( GameBoardManager.Instance.CurrentTeam );
		if(cdata.cardType == CardType.CHARACTER && hoveredCell.row < 2)
		{
			GameCharacter newCharacter = player.CreateCharacter(cardId, hoveredCell);
			hoveredCell = null;

			return (newCharacter != null);
		}
		else if(cdata.cardType == CardType.SKILL)
		{
			return ReleaseSkillOnBoard(board, hoveredCell, cdata.dataId);
		}

		return false;
	}

	#endregion


	private void DragCharacterOnBoard(GameBoard board, int charId)
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


	private void DragSkillOnBoard(GameBoard board, int skillId)
	{
		BoardPlayer player = board.GetPlayer( GameBoardManager.Instance.CurrentTeam );
		SkillData skdata = SkillsDatabase.Instance.GetData(skillId);
		if(skdata == null)
			return;

		List<int> skilledCharacters = CharacterDatabase.Instance.GetCharacterThatUsesSkill(skillId);
		foreach(int charId in skilledCharacters)
		{
			player.HighlightAreaAroundCharacters(board, charId);
		}
	}

	// TODO:
	// - check if cell is within range of characters
	// - drop skill code
	// - damage characters
	// - play FX
	// - consume card
	// - consume resources
	private bool ReleaseSkillOnBoard(GameBoard board, Cell targetCell, int skillId)
	{
		BoardPlayer player = board.GetPlayer( GameBoardManager.Instance.CurrentTeam );
		SkillData skdata = SkillsDatabase.Instance.GetData(skillId);
		if(skdata == null)
			return false;

		if(player.DropSkillOnBoard(board, targetCell, skdata))
		{
			BoardPlayer oponent = board.GetPlayer( GameBoardManager.Instance.OpposingTeam );
			oponent.HitPlayersFromSkill(board, targetCell, skdata);
			return true;
		}
		return false;
	}


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
