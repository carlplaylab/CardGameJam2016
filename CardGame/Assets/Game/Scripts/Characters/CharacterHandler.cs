using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterHandler : MonoBehaviour 
{
	[SerializeField] private int[] characterIds;


	private static CharacterHandler instance;
	public static CharacterHandler Instance
	{
		get { return instance; }
	}

	private bool initialize = false;
	private Dictionary<int, GameObject> refCharacterOjbects;

	private List<GameCharacter> characterList;


	void Awake ()
	{
		instance = this;
		Initialize();
	}

	void OnDestroy ()
	{
		instance = null;
	}


	private void Initialize ()
	{
		if(initialize)
			return;

		initialize = true;

		refCharacterOjbects = new Dictionary<int, GameObject>();
		characterList = new List<GameCharacter>();

		for(int i = 0; i < characterIds.Length; i++)
		{
			int charId = characterIds[i];
			CharacterData cdata = CharacterDatabase.Instance.GetData(charId);
			if(cdata == null)
				continue;
			
			GameObject cObj = Resources.Load("Prefabs/Characters/" + cdata.characterPrefab) as GameObject;
			refCharacterOjbects.Add( charId, cObj );
		}
	}


	private GameCharacter CreateCharacter(int characterID)
	{
		if(refCharacterOjbects == null || !refCharacterOjbects.ContainsKey(characterID))
			return null;

		GameObject refObject = refCharacterOjbects[characterID];
		GameObject newObject = GameObject.Instantiate(refObject) as GameObject;
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


	public void SetTeam (int focusedTeam)
	{
		for(int i=0; i < characterList.Count; i++)
		{
			characterList[i].Interaction = (characterList[i].Team == focusedTeam) ;
		}
	}
}
