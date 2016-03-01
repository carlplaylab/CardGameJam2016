using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellHandler : MonoBehaviour 
{

	private List<Cell> cells;
	private List<List<Cell>> cellMatrix;


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

}
