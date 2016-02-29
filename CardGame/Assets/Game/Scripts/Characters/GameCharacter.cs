using UnityEngine;
using System.Collections;

public class GameCharacter : BoardObject 
{

	private CharacterData characterData;
	private CharacterStats characterStats;
	private CharacterInfoDisplay infoDisplay;

	private int teamNumber;


	void Awake ()
	{
		infoDisplay = this.GetComponentInChildren<CharacterInfoDisplay>();
	}
		

	public int Team
	{
		get { return teamNumber; }
	}


	public void SetupCharacter(int Team )
	{
	}

	public override void Initialize (BoardObjectData objectData = null)
	{
		base.Initialize();
		data.movementSpace = characterData.movement;
	}


}
