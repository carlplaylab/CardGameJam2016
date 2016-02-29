﻿using UnityEngine;
using System.Collections;

public class GameBoard : MonoBehaviour 
{

	[SerializeField] private int cellAreaId;
	[SerializeField] private BoardObject boardObject;

	private CellHandler cellHandler = null;
	private BoardObject selectedObject = null;



	void Awake ()
	{
	}


	void Start ()
	{
		LoadCells();
		LoadCharacters();

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

		if(boardObject != null)
			boardObject.Initialize();
	}


	private void LoadCharacters ()
	{
		for(int i=1; i <= 6; i ++)
		{
			int row = 2;
			int col = i %3;
			int team = 1;

			if(i > 3)
			{
				team = 2;
				row = 5;
			}
			
			
			Cell freeCell = cellHandler.GetCellAt(row, col);
			if(freeCell != null)
			{
				GameCharacter newChar = CharacterHandler.Instance.CreateCharacterOnCell(i, freeCell);
				newChar.SetTeam(team);
			}
			else{
				Debug.Log("null cell");
			}
		}
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
		if(selectedObject == null || targetCell == null)
		{
			return false;
		}

		cellHandler.RemoveHighlights();
		selectedObject.OnFocus(false);

		bool transferSuccess = false;
		if(targetCell.IsVacant())
		{
			
			Cell previousCell = cellHandler.GetCell( selectedObject.cellId );
			transferSuccess = selectedObject.TransferCells(targetCell, previousCell);
		}
		else
		{
			ProcessEncounter(targetCell);
			selectedObject = null;
			return false;
		}

		selectedObject = null;
		return transferSuccess;
	}


	public void ProcessEncounter (Cell targetCell)
	{
		if(targetCell == null || targetCell.IsVacant() || selectedObject == null)
			return;

		Cell previousCell = cellHandler.GetCell( selectedObject.cellId );
		if(!selectedObject.AllowMovementOnCell(targetCell, previousCell))
			return;

		if(!selectedObject.IsCharacter() || !targetCell.ResidingObject.IsCharacter())
			return;

		GameCharacter selectedChar = (GameCharacter)selectedObject;
		if(!selectedChar.CheckIfEnemy(targetCell.ResidingObject))
			return;

		GameCharacter targetChar = (GameCharacter)targetCell.ResidingObject;
		bool targetDies = targetChar.ReceiveAttack( selectedChar.AttackStrength );
		bool selectedDies = selectedChar.ReceiveAttack( targetChar.AttackStrength );

		if(targetDies)
		{
			targetChar.TriggerDie();
			targetCell.ResidingObject = null;
		}

		if(selectedDies)
		{
			selectedChar.TriggerDie();
			selectedObject = null;
			previousCell.ResidingObject = null;
		}
	}

}
