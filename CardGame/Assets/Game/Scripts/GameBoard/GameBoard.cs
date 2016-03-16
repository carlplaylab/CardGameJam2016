using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameBoard : MonoBehaviour 
{

	[SerializeField] private CharacterTeam[] teams;
	[SerializeField] private CardDeck[] teamDeck;

	private GameInput input = null;
	private CellHandler cellHandler = null;
	private BoardObject selectedObject = null;
	private int currentTeam = 1;

	public Action onPlayerMoveEnded = null;


	void Awake ()
	{
		input = GetComponent<GameInput>();

		teams = new CharacterTeam[2];
		teamDeck = new CardDeck[2];

		for(int i=0; i < 2; i++)
		{
			teams[i] = new CharacterTeam(i+1) ;

			teamDeck[i] = new CardDeck();
			teamDeck[i].SetupDeck(GameBoardManager.Instance.Settings.Level, i+1);
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
		int teamIdx = Mathf.Clamp(teamNumber-1, 0, teams.Length-1);
		return teams[teamIdx];
	}

	public CharacterTeam GetOpposingTeam()
	{
		return GetTeam( (currentTeam+1)%(teams.Length) );
	}

	public CharacterTeam GetCurrentTeam()
	{
		return GetTeam( currentTeam );
	}

	public CardDeck GetDeck(int teamNumber)
	{
		int teamIdx = Mathf.Clamp(teamNumber-1, 0, teams.Length-1);
		return teamDeck[teamIdx];
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

		bool releaseSuccess = false;

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
			GetOpposingTeam().KillGameCharacter(targetChar);

			targetCell.ResidingObject = null;
		}

		if(selectedDies)
		{
			GetCurrentTeam().KillGameCharacter(selectedChar);
			selectedObject = null;
			previousCell.ResidingObject = null;
		}

		return true;
	}


	public void SetTeam (int newTeam)
	{
		currentTeam = newTeam;
		CharacterHandler.Instance.SetTeam( newTeam );
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
