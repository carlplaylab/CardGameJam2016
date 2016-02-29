using UnityEngine;
using System.Collections;

public class GameBoard : MonoBehaviour 
{

	[SerializeField] private int cellAreaId;
	[SerializeField] private BoardObject boardObject;

	private CellHandler cellHandler = null;
	private BoardObject selectedObject = null;

	void Awake ()
	{
		LoadCells();
	}


	void Start ()
	{
		cellHandler.AssignObjectOn(boardObject, 0, 0);
		selectedObject = null;
	}


	private void LoadCells ()
	{
		Object resObj = Resources.Load("Prefabs/GameBoards/cellArea_" + cellAreaId);
		if(resObj == null)
			return;

		GameObject cellAreaObj = GameObject.Instantiate(resObj) as GameObject;
		cellAreaObj.transform.SetParent(this.transform);
		cellAreaObj.transform.localPosition = Vector3.zero;
		cellAreaObj.name = "cellHandler";

		cellHandler = cellAreaObj.GetComponent<CellHandler>();
		cellHandler.InitializeForGame();

		boardObject.Initialize();
	}


	public void CellClicked(Cell targetCell)
	{
		if(selectedObject == null)
		{
			HoldObject(targetCell);
		}
		else
		{
			ReleaseObject(targetCell);
		}
	}


	public void HoldObject(Cell targetCell)
	{
		if(targetCell == null || selectedObject != null)
		{
			return;
		}

		if(targetCell.ResidingObject == null || !targetCell.ResidingObject.IsMoveable())
		{
			return;
		}

		selectedObject = targetCell.ResidingObject;
		selectedObject.OnFocus(true);
		cellHandler.HighlightAround(targetCell, selectedObject.data.movementSpace);
	}

	public bool ReleaseObject(Cell targetCell)
	{
		if(selectedObject == null)
		{
			return false;
		}

		bool transferSuccess = false;
		if(targetCell != null && targetCell.IsVacant())
		{
			
			Cell previousCell = cellHandler.GetCell( selectedObject.cellId );
			transferSuccess = selectedObject.TransferCells(targetCell, previousCell);
		}

		selectedObject.OnFocus(false);
		selectedObject = null;
		cellHandler.RemoveHighlights();

		return transferSuccess;
	}

}
