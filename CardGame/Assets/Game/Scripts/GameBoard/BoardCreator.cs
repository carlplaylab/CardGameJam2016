using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class BoardCreator : MonoBehaviour 
{

	[SerializeField] CellData celldata;
	[SerializeField] BoardData boardData;
	[SerializeField] bool createGameBoard;
	[SerializeField] GameObject cellArea;

	[SerializeField] BoardCellTypeMenu cellMenu;
	[SerializeField] CellHandler cellHandler;

	private List<Cell> gameCells;


	void Awake ()
	{
		if(celldata == null)
			celldata = GetComponentInChildren<CellData>();
		if(boardData == null)
			boardData = GetComponentInChildren<BoardData>();
		if(cellHandler == null)
			cellHandler = GetComponentInChildren<CellHandler>();
	}


	void Start ()
	{
		if(createGameBoard)
		{
			CreateBoard();
		}
	}


	private void CreateBoard ()
	{
		gameCells = new List<Cell>();
		int cellCount = 0;
		for(int r=0; r < boardData.rows; r++)
		{
			for(int c=0; c < boardData.col; c++)
			{
				Cell newCell = CreateCell(r, c, cellCount);
				gameCells.Add(newCell);
				cellCount++;
			}
		}

		cellHandler.SetCells(gameCells);
	}


	private Cell CreateCell(int row, int col, int cellId) 
	{
		float cellHeight = celldata.cellHeight;
		float cellWidth = celldata.cellWidth;
		float startX = boardData.cellStartPosX;
		float startY = boardData.cellStartPosY;

		Vector3 pos = new Vector3(startX + cellWidth * col, 
			startY + cellHeight * row,
			0f);

		GameObject cellObj = celldata.CreateCell(cellArea.transform, pos);
		cellObj.SetActive(true);

		Cell newCell = cellObj.GetComponent<Cell>();
		newCell.name = "cell_" + row + "_" + col;
		newCell.row = row;
		newCell.col = col;
		newCell.id = cellId;
		return newCell;
	}

	/*
	private Cell CreateCell(int row, int col, int cellId) 
	{
		float cellHeight = celldata.cellHeight/2f;
		float cellWidth = celldata.cellWidth * 3f/2f;
		float startX = boardData.cellStartPosX;
		float startY = boardData.cellStartPosY;

		if(row%2 == 1)
		{
			startX = boardData.cellStartPosX + cellWidth/2f;
			//cellHeight = celldata.cellHeight * 0.5f;
		}

		Vector3 pos = new Vector3(startX + cellWidth*col, 
			startY + cellHeight * row,
			0f);
		
		GameObject cellObj = celldata.CreateCell(cellArea.transform, pos);
		cellObj.SetActive(true);

		Cell newCell = cellObj.GetComponent<Cell>();
		newCell.name = "cell_" + col + "_" + row;
		newCell.row = row;
		newCell.col = col;
		newCell.id = cellId;
		return newCell;
	}
	*/


	public void CellClicked(Cell targetCell)
	{
		if(!cellMenu.gameObject.activeSelf)
		{
			cellMenu.Show(targetCell);
			cellHandler.SetCellsInput(false);
		}
	}


	public void FocusCells ()
	{
		cellHandler.SetCellsInput(true);
	}


}
