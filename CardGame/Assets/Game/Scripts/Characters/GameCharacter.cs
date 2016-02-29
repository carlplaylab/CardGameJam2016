using UnityEngine;
using System.Collections;

public class GameCharacter : BoardObject 
{

	private CharacterData characterData;
	private CharacterStats characterStats;
	private CharacterInfoDisplay infoDisplay;

	private int teamNumber;
	private int characterIndex;


	void Awake ()
	{
		infoDisplay = this.GetComponentInChildren<CharacterInfoDisplay>();
	}
		

	public int Team
	{
		get { return teamNumber; }
	}


	public int AttackStrength
	{
		get { 
			if(characterData != null)
				return characterData.attack;
			else
				return 0;
		}
	}


	public void SetTeam(int team)
	{
		teamNumber = team;
	}


	public void SetupCharacter(CharacterData data, int charIndex, int team = 1)
	{
		characterData = data;
		characterStats = new CharacterStats(data);
		teamNumber = team;
		characterIndex = charIndex;

		Initialize();
		UpdateStats();
	}


	public override void Initialize (BoardObjectData objectData = null)
	{
		base.Initialize();
		data.movementSpace = characterData.movement;
		data.elementType = characterData.elementType;
		data.objectType = BoardObjectType.CHARACTER;
	}


	public void UpdateStats()
	{
		infoDisplay.SetStats(characterStats);
	}


	public bool CheckIfEnemy(BoardObject targetObj)
	{
		if(targetObj.objectType == BoardObjectType.CHARACTER)
		{
			GameCharacter targetChar = (GameCharacter)targetObj;
			return targetChar.teamNumber != teamNumber;
		}
		return false;
	}


	public bool ReceiveAttack (int attackingStrength)
	{
		characterStats.life = Mathf.Clamp(characterStats.life - attackingStrength, 0, characterData.life);
		UpdateStats();
		return (characterStats.life <= 0);
	}


	public void TriggerDie ()
	{
		this.gameObject.SetActive(false);

		cellId = -1;
		state = BoardObjectState.INACTIVE;
	}

}
