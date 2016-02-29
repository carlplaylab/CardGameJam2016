using UnityEngine;
using System.Collections;



public class BoardObject : MonoBehaviour 
{
	public enum BoardObjectState
	{
		IDLE = 0,
		FOCUSED = 1
	}

	public int cellId = 0;
	public ElementType type;
	public BoardObjectState state;
	public BoardObjectData data;



	public virtual void Initialize (BoardObjectData objectData = null)
	{
		if(objectData == null)
			data = GetComponent<BoardObjectData>();
		else
			data = objectData;
	}
		
	public virtual void OnFocus (bool focus)
	{
		if(focus)
		{
			this.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
		}
		else
		{
			this.transform.localScale = Vector3.one;
		}
	}

	public virtual bool IsMoveable ()
	{
		if(data != null)
			return data.moveable;
		else
			return true;
	}

	public virtual int GetMovement ()
	{
		if(data != null)
			return data.movementSpace;
		else
			return 1;
	}

	public virtual bool AllowMovementOnCell (Cell targetCell, Cell residingCell)
	{
		if(!IsMoveable())
			return false;

		if(data == null || targetCell == null || residingCell == null)
			return false;
		
		int cellDist = targetCell.GetDistanceFromCell(residingCell);
		return cellDist <= GetMovement();
	}

	public virtual bool OnLand (Cell landedCell)
	{
		if(landedCell != null)
		{
			this.transform.localPosition = landedCell.transform.localPosition;
			cellId = landedCell.id;
			return true;
		}
		return false;
	}

	public virtual bool TransferCells (Cell targetCell, Cell residingCell)
	{
		if(!AllowMovementOnCell(targetCell, residingCell))
			return false;

		if(OnLand(targetCell))
		{
			targetCell.ResidingObject = this;
			residingCell.ResidingObject = null;
			return true;
		}
		return false;
	}
}
