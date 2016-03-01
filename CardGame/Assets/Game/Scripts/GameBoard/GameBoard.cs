using UnityEngine;
using System.Collections;

public class GameBoard : MonoBehaviour 
{

	[SerializeField] public int cellAreaId;

	private CellHandler cellHandler = null;
	private BoardObject selectedObject = null;

	private int currentTeam = 1;


	public int CurrentTeam
	{
		get { return currentTeam; }
		set { currentTeam = Mathf.Clamp(value, 1,2); }
	}


	void Awake ()
	{
	}


	void Start ()
	{
		
		LoadCells();
		LoadCharacters();

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
			bool actionFinished = ReleaseObject(targetCell);
			if(actionFinished)
				ToggleTeam();
		}
	}


	public void HoldObject(Cell targetCell)
	{
		if(targetCell == null || selectedObject != null)
		{
			return;
		}

		if(targetCell.IsVacant() || !targetCell.ResidingObject.IsMoveable())
		{
			return;
		}

		if(targetCell.ResidingObject.IsCharacter())
		{
			GameCharacter character = (GameCharacter)targetCell.ResidingObject;
			if(character.Team != CurrentTeam)
			{
				return;
			}
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

		bool releaseSuccess = false;
		if(targetCell.IsVacant())
		{
			
			Cell previousCell = cellHandler.GetCell( selectedObject.cellId );
			releaseSuccess = selectedObject.TransferCells(targetCell, previousCell);
			selectedObject = null;
			return releaseSuccess;
		}
		else
		{
			releaseSuccess = ProcessEncounter(targetCell);
			selectedObject = null;
			return releaseSuccess;
		}
	}


	public bool ProcessEncounter (Cell targetCell)
	{
		if(targetCell == null || targetCell.IsVacant() || selectedObject == null)
			return false;

		Cell previousCell = cellHandler.GetCell( selectedObject.cellId );
		if(!selectedObject.AllowMovementOnCell(targetCell, previousCell))
			return false;

		if(!selectedObject.IsCharacter() || !targetCell.ResidingObject.IsCharacter())
			return false;

		GameCharacter selectedChar = (GameCharacter)selectedObject;
		if(!selectedChar.CheckIfEnemy(targetCell.ResidingObject))
			return false;

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

		return true;
	}


	public void ToggleTeam ()
	{
		currentTeam++;
		if(CurrentTeam > 2)
			currentTeam = 1;

		CharacterHandler.Instance.SetTeam( CurrentTeam );
	}

}
