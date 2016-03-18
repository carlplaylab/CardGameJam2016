using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameBoard : MonoBehaviour 
{

	[SerializeField] private BoardPlayer[] players;

	private GameInput input = null;
	private CellHandler cellHandler = null;
	private BoardObject selectedObject = null;
	private int currentTeam = 1;

	public Action onPlayerMoveEnded = null;


	void Awake ()
	{
		input = GetComponent<GameInput>();
	}


	public void Setup ()
	{
		int level = GameBoardManager.Instance.Settings.Level;
		for(int i=0; i < players.Length; i++)
		{
			players[i].Setup(i+1, level);
		}
	}


	void Start ()
	{
		selectedObject = null;
	}


	public bool InputEnable
	{
		get { return input.Active; }
		set { input.Active = value; }
	}


	public CellHandler BoardCells
	{
		get { return cellHandler; }
		set { cellHandler = value; }
	}


	public CharacterTeam GetTeam(int teamNumber)
	{
		int teamIdx = Mathf.Clamp(teamNumber-1, 0, players.Length-1);
		return players[teamIdx].Team;
	}

	public CharacterTeam GetOpposingTeam()
	{
		return GetTeam( (currentTeam+1)%(players.Length) );
	}

	public CharacterTeam GetCurrentTeam()
	{
		return GetTeam( currentTeam );
	}

	public CardDeck GetDeck(int teamNumber)
	{
		int teamIdx = Mathf.Clamp(teamNumber-1, 0, players.Length-1);
		return players[teamIdx].Deck;
	}

	public BoardPlayer GetPlayer(int teamNumber)
	{
		int teamIdx = Mathf.Clamp(teamNumber-1, 0, players.Length-1);
		return players[teamIdx];
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
			if(actionFinished && onPlayerMoveEnded != null)
			{
				onPlayerMoveEnded();
			}
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
			if(character.Team != currentTeam)
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

		bool releaseSuccess = DropObjectOnCell(selectedObject, targetCell);
		selectedObject = null;
		return releaseSuccess;

		/*
		if(!targetCell.IsWalkable())
		{
			selectedObject = null;
			return releaseSuccess;
		}
		else if(targetCell.IsVacant())
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
		*/
	}


	public bool DropObjectOnCell(BoardObject bobj, Cell targetCell)
	{
		bool releaseSuccess = false;
		if(!targetCell.IsWalkable())
		{
			return releaseSuccess;
		}
		else if(targetCell.IsVacant())
		{

			Cell previousCell = cellHandler.GetCell( bobj.cellId );
			releaseSuccess = bobj.TransferCells(targetCell, previousCell);
			Debug.Log("TransferCells");
			return releaseSuccess;
		}
		else
		{
			releaseSuccess = ProcessEncounter(bobj, targetCell);
			return releaseSuccess;
		}
	}


	public bool ProcessEncounter (BoardObject selObj, Cell targetCell)
	{
		Debug.Log("ProcessEncounter");

		if(targetCell == null || targetCell.IsVacant() || selObj == null)
			return false;

		Cell previousCell = cellHandler.GetCell( selObj.cellId );
		if(!selObj.AllowMovementOnCell(targetCell, previousCell))
			return false;

		if(!selObj.IsCharacter() || !targetCell.ResidingObject.IsCharacter())
			return false;

		GameCharacter selectedChar = (GameCharacter)selObj;
		if(!selectedChar.CheckIfEnemy(targetCell.ResidingObject))
			return false;

		GameCharacter targetChar = (GameCharacter)targetCell.ResidingObject;
		bool targetDies = targetChar.ReceiveAttack( selectedChar.AttackStrength );
		bool selectedDies = selectedChar.ReceiveAttack( targetChar.AttackStrength );

		if(targetDies)
		{
			GetOpposingTeam().KillGameCharacter(targetChar);

			targetCell.ResidingObject = null;
		}

		if(selectedDies)
		{
			GetCurrentTeam().KillGameCharacter(selectedChar);
			previousCell.ResidingObject = null;
		}

		return true;
	}


	public void SetTeam (int newTeam)
	{
		currentTeam = newTeam;
		CharacterHandler.Instance.SetTeam( newTeam );

		for(int i=0; i < players.Length; i++)
		{
			players[i].SetTurn(newTeam);
		}
	}

	// instantly checks cell using gameInput
	public Cell GetHoveredCell ()
	{
		GameObject go = input.GetHoveredOjbect();
		if(go != null)
		{
			Cell cell = go.GetComponent<Cell>();
			return cell;
		}
		return null;
	}

}
