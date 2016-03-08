using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellHandler : MonoBehaviour 
{

	private List<Cell> cells;
	private List<List<Cell>> cellMatrix; // row, col
	private int maxCols = 0;
	private int maxRows = 0;

	public int Rows
	{
		get{ return maxRows; }
	}

	public int Cols
	{
		get{ return maxCols; }
	}

	public void InitializeForGame ()
	{
		Cell[] childCells = GetComponentsInChildren<Cell>();
		cells = new List<Cell>( childCells );

		int maxRow = 0;
		int maxCol = 0;
		for(int i=0; i < cells.Count; i++)
		{
			if(cells[i].row > maxRow)
				maxRow = cells[i].row;
			if(cells[i].col > maxCol)
				maxCol = cells[i].col;
		}

		cellMatrix = new List<List<Cell>>();
		for(int r=0; r <= maxRow; r++)
		{
			List<Cell> rowCells = new List<Cell>();
			for(int c=0; c <= maxCol; c++)
			{
				rowCells.Add(null);
			}
			cellMatrix.Add(rowCells);
		}

		for(int i=0; i < cells.Count; i++)
		{
			List<Cell> rowCells = cellMatrix[cells[i].row];
			rowCells[cells[i].col] = cells[i];
		}


		maxCols = maxCol;
		maxRows = maxRow;
	}

	public void SetCells(List<Cell> newCells)
	{
		cells = newCells;
	}

	public void SetCellsInput(bool controlActive)
	{
		if(cells == null)
			return;
		
		for(int i=0; i < cells.Count; i++)
		{
			cells[i].SetControls(controlActive);
		}
	}

	public void RemoveHighlights ()
	{
		for(int i=0; i < cells.Count; i++)
		{
			cells[i].highlighter.Hide();
		}
	}

	public void HighlightAround(Cell targetCell, int range)
	{
		for(int i=0; i < cells.Count; i++)
		{
			if(targetCell.GetDistanceFromCell(cells[i]) <= range)
			{
				bool useGreen = cells[i].IsVacant() || targetCell == cells[i];
				cells[i].highlighter.Show(useGreen);
			}
		}
	}

	public void HiglightArea(Rect cellRect)
	{
		int minCol = (int)cellRect.xMin;
		int minrow = (int)cellRect.yMin;
		int maxCol = (int)cellRect.xMax;
		int maxRow = (int)cellRect.yMax;

		for(int i=0; i < cells.Count; i++)
		{
			int row = cells[i].row;
			int col = cells[i].col;

			if(minrow <= row && row <= maxRow && 
				minCol <= col && col <= maxCol)
			{
				cells[i].highlighter.Show( cells[i].IsVacant() );
			}
		}
	}

	public Cell GetCell(int id)
	{
		if(id < cells.Count)
			return cells[id];
		else
			return null;
	}

	public Cell GetCellAt(int row, int col)
	{
		if(row < cellMatrix.Count)
		{
			List<Cell> rowCells = cellMatrix[row];
			if(col < rowCells.Count)
				return rowCells[col];
		}
		return null;
	}

	public virtual void AssignObjectOn(BoardObject obj, int row, int col)
	{
		Cell targetCell = GetCellAt(row, col);
		if(targetCell != null && targetCell.IsVacant())
		{
			targetCell.ResidingObject = obj;
			if(obj != null)
			{
				obj.OnLand(targetCell);
			}
		}

	}


	// Unlike GetHoveredCell() of GameBoard, this just checks cells status, not using GameInput
	public virtual Cell GetHoveredCell ()
	{
		for(int i=0; i < cells.Count; i++)
		{
			if(cells[i].CheckHovered())
				return cells[i];
		}
		return null;
	}


	public virtual void HighlightHoveredCell()
	{
		Cell hoveredCell = GetHoveredCell();
		if(hoveredCell != null)
		{
			hoveredCell.highlighter.Show( hoveredCell.IsVacant() );
		}
	}

}
