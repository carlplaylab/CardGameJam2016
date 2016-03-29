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

		SetCellsVisible(false);
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

	public void SetCellsVisible(bool visible)
	{
		if(visible)
		{
			for(int i=0; i < cells.Count; i++)
			{
				cells[i].Show();
			}
		}
		else
		{
			for(int i=0; i < cells.Count; i++)
			{
				cells[i].Hide();
			}
		}
	}

	public void SetCellsColliderActive(bool activeCollider)
	{
		for(int i=0; i < cells.Count; i++)
		{
			cells[i].SetColliderActive(activeCollider);
		}
	}

	public void RemoveHighlights ()
	{
		for(int i=0; i < cells.Count; i++)
		{
			cells[i].HighlightCell(false);
		}
		SetCellsVisible(false);
	}

	public void HighlightAround(Cell targetCell, int range)
	{
		for(int i=0; i < cells.Count; i++)
		{
			if(targetCell.GetDistanceFromCell(cells[i]) <= range)
			{
				bool useGreen = cells[i].IsVacant() || targetCell == cells[i];
				cells[i].HighlightCell(true, useGreen);
			}
		}
		SetCellsVisible(true);
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
				cells[i].HighlightCell(true, cells[i].IsVacant() );
			}
		}

		SetCellsVisible(true);
	}


	public List<int> GetCellIdsInArea(Rect cellRect)
	{
		List<int> cellsList = new List<int>();
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
				cellsList.Add( cells[i].id );
			}
		}
		return cellsList;
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
			hoveredCell.HighlightCell(true, hoveredCell.IsVacant() );
		}
	}


	public List<Cell> GetCellsInArea(Rect cellRect)
	{
		int minCol = (int)cellRect.xMin;
		int minrow = (int)cellRect.yMin;
		int maxCol = (int)cellRect.xMax;
		int maxRow = (int)cellRect.yMax;

		List<Cell> focusedCells = new List<Cell>();
		for(int i=0; i < cells.Count; i++)
		{
			int row = cells[i].row;
			int col = cells[i].col;

			if(minrow <= row && row <= maxRow && 
				minCol <= col && col <= maxCol)
			{
				focusedCells.Add(cells[i]);
			}
		}
		return focusedCells;
	}

	public Cell GetRandomVacantCell(Rect cellRect)
	{
		List<Cell> focusedCells = GetCellsInArea(cellRect);
		List<Cell> randomCells = new List<Cell>(); 
		for(int i=0; i < focusedCells.Count; i++)
		{
			if(focusedCells[i].IsVacant() && focusedCells[i].IsWalkable())
			{
				randomCells.Add(focusedCells[i]);
			}
		}

		if(randomCells.Count > 0)
		{
			int randIdx = UnityEngine.Random.Range(0, randomCells.Count-1);
			return randomCells[randIdx];
		}

		return null;
	}

	#region UTILS

	public bool CheckIfWithinRange(int cell1, int cell2, int range)
	{
		Cell c1 = GetCell(cell1);
		Cell c2 = GetCell(cell2);

		int diffRow = Mathf.Abs(c1.row - c2.row);
		int diffCol = Mathf.Abs(c1.col - c2.col);

		return (diffRow <= range && diffCol <= range);
	}

	#endregion

}
