using UnityEngine;
using System.Collections;

public class GameCharacter : BoardObject 
{
	[SerializeField] private SpriteRenderer characterSprite; 
	[SerializeField] private SpriteRenderer baseSprite; 
	[SerializeField] private SpriteRenderer baseSupportSprite; 
	[SerializeField] private GameObject mainBody; 

	private CharacterData characterData;
	private CharacterStats characterStats;
	private CharacterInfoDisplay infoDisplay;
	private Animator animator;

	private int teamNumber;
	private int teamIndex;
	private int characterIndex;
	private bool allowInteraction;


	void Awake ()
	{
		infoDisplay = this.GetComponentInChildren<CharacterInfoDisplay>();
		animator = this.GetComponentInChildren<Animator>();
	}
		

	public int Team
	{
		get { return teamNumber; }
	}

	public int Index	// on overall character list
	{
		get { return characterIndex; }
	}

	public int TeamIndex	// on overall character list
	{
		get { return teamIndex; }
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


	public bool Interaction
	{
		get { return allowInteraction; }
		set 
		{
			allowInteraction = value;
			if(allowInteraction)
			{
				animator.Play("character_body_idle");
			}
			else
			{
				animator.Play("idle");
			}
		}
	}

	public override void OnFocus (bool focus)
	{
		if(focus)
		{
			mainBody.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
		}
		else
		{
			mainBody.transform.localScale = Vector3.one;
		}
	}

	public void AddedToTeam(int team, int idx)
	{
		SetTeam(team);
		teamIndex = idx;
	}

	public void SetTeam(int team)
	{
		teamNumber = team;
		Color baseColor = CharacterHandler.Instance.GetTeamColor(team);
		baseSupportSprite.color = baseColor;
	}


	public void SetupCharacter(CharacterData data, int charIndex,int team = 1)
	{
		characterData = data;
		characterStats = new CharacterStats(data);
		characterIndex = charIndex;

		//Debug.Log("Sprite : " + data.ingameSprite);
		Sprite csprite = IngameSpriteCenter.Instance.GetSprite(data.ingameSprite);
		if(csprite != null)
			characterSprite.sprite = csprite;

		Sprite bsprite = IngameSpriteCenter.Instance.GetBaseSprite(data.elementType);
		if(bsprite != null)
			baseSprite.sprite = bsprite;

		SetTeam(team);

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
		animator.Play("idle");

		infoDisplay.Hide();

		cellId = -1;
		state = BoardObjectState.INACTIVE;
	}

}
