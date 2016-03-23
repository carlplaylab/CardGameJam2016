using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[SerializeField]
public class CharacterTeam  
{
	[SerializeField] private List<GameCharacter> teamList;
	[SerializeField] private List<GameCharacter> aliveCharacters;
	[SerializeField] private List<GameCharacter> deadCharacters;

	private int team;


	public int Team
	{
		get { return team; }
	}


	public int Count
	{
		get { return aliveCharacters.Count; }
	}


	public CharacterTeam (int teamNumber)
	{
		team = teamNumber;

		teamList = new List<GameCharacter>();
		aliveCharacters = new List<GameCharacter>();
		deadCharacters = new List<GameCharacter>();
	}
		
	public GameCharacter GetRandomCharacter ()
	{
		if(aliveCharacters == null || aliveCharacters.Count <= 0)
			return null;
		
		int rand = UnityEngine.Random.Range(0, aliveCharacters.Count-1);
		return aliveCharacters[rand];
	}

	public void AddGameCharacter (GameCharacter newCharacter)
	{
		if(newCharacter == null)
			return;

		newCharacter.AddedToTeam(Team, teamList.Count);
		teamList.Add(newCharacter);
		aliveCharacters.Add(newCharacter);
	}

	public void KillGameCharacter (GameCharacter deadguy)
	{
		if(deadguy.Team != Team)
			return;

		float posY = 0f;
		if(Team == 1)
		{
			posY = -4.8f + deadCharacters.Count * 0.6f;
		}
		else
		{
			posY = 4.3f - deadCharacters.Count * 0.6f;
		}

		deadguy.TriggerDie();
		aliveCharacters.Remove(deadguy);
		deadCharacters.Add(deadguy);

		// additional
		deadguy.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
		deadguy.transform.position = new Vector3(3f, posY, 0f);
		deadguy.gameObject.SetActive(true);
	}

	public GameCharacter GetNearestCharacter(GameCharacter focusCharacter, CellHandler boardCells)
	{
		Cell baseCell = boardCells.GetCell(focusCharacter.cellId);
		GameCharacter nearestChar = null;
		float minDistance = 9999f;

		for(int i=0; i < aliveCharacters.Count; i++)
		{
			Cell otherCell = boardCells.GetCell(aliveCharacters[i].cellId);
			baseCell.GetDistanceFromCell(otherCell);
			if(otherCell.PythDistance < minDistance)
			{
				minDistance = otherCell.PythDistance;
				nearestChar = aliveCharacters[i];
			}
		}

		return nearestChar;
	}
}
