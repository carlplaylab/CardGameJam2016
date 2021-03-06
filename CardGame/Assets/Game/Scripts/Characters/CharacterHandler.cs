﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterHandler : MonoBehaviour 
{

	[SerializeField] private GameObject baseCharacterObj;
	[SerializeField] private int[] characterIds;
	[SerializeField] private Color[] teamColors;


	private static CharacterHandler instance;
	public static CharacterHandler Instance
	{
		get { return instance; }
	}

	private bool initialize = false;

	private List<GameCharacter> characterList;


	void Awake ()
	{
		instance = this;
	}

	void OnDestroy ()
	{
		instance = null;
	}

	public void Initialize ()
	{
		if(initialize)
			return;

		initialize = true;

		characterList = new List<GameCharacter>();
	}

	private GameCharacter CreateCharacter(int characterID)
	{
		/*
		if(refCharacterOjbects == null || !refCharacterOjbects.ContainsKey(characterID))
			return null;
		*/
		if(baseCharacterObj == null)
		{
			Debug.LogError("No reference character set");
			return null;
		}

		//GameObject refObject = refCharacterOjbects[characterID];
		GameObject newObject = GameObject.Instantiate(baseCharacterObj) as GameObject;
		GameCharacter newCharacter = newObject.GetComponent<GameCharacter>();

		CharacterData cdata = CharacterDatabase.Instance.GetData(characterID);
		int idx = characterList.Count;

		newObject.name = "char_" + idx;
		newObject.transform.SetParent(this.transform);

		newCharacter.SetupCharacter(cdata, idx, 1);
		characterList.Add(newCharacter);

		return newCharacter;
	}

	public GameCharacter CreateCharacterOnCell(int charId, Cell targetCell)
	{
		if(!targetCell.IsVacant())
			return null;

		GameCharacter newChar = CreateCharacter(charId);
		if(newChar != null)
		{
			newChar.OnLand(targetCell);	
			targetCell.ResidingObject = (BoardObject)newChar;
			return newChar;
		}
		return null;
	}

	public Color GetTeamColor(int team)
	{
		int teamIdx = Mathf.Clamp(team-1, 0, teamColors.Length-1);
		return teamColors[teamIdx];
	}

	public void SetTeam (int focusedTeam)
	{
		for(int i=0; i < characterList.Count; i++)
		{
			characterList[i].Interaction = (characterList[i].Team == focusedTeam) ;
		}
	}

	public void LogCharacterDied(int characterId)
	{
		//Debug.Log("LogCharacterDied " + characterId);
		if(characterId == 1 || characterId == 5)
		{
			bool win = (characterId == 5);
			Parameters winparams = new Parameters();
			winparams.PutExtra("result", win);
			EventBroadcaster.Instance.PostEvent(EventNames.UI_SHOW_RESULTS, winparams);
		}
	}
}
