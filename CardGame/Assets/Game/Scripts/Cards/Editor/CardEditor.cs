using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class CardEditor : AbstractListDataEditorWindow<CardData>  
{

	[MenuItem ("MyMenu/Do Something with a Shortcut Key %l")]
	static void DoSomethingWithAShortcutKey () 
	{
		GetWindow();
	}

	[MenuItem("Card Game Tools/Card Editor %l")]
	public static void GetWindow()
	{
		CardEditor cardEditor = EditorWindow.GetWindow<CardEditor>("Card Database", true);
		cardEditor.minSize = new Vector2(1024f, 400f);
	}

	private bool initialized = false;
	private CardData selectedData = null;
	private string[] elements;
	private string[] cardtypes;

	public void Initialize ()
	{
		if(initialized)
			return;

		elements = new string[4];
		for(int i=0; i < 4; i++)
		{
			elements[i] = ((ElementType)i).ToString();
		}

		cardtypes = new string[2];
		cardtypes[0] = ((CardType)0).ToString();
		cardtypes[1] = ((CardType)1).ToString();

		// making sure it loads
		CardDatabase cardDatabase = CardDatabase.Instance;
		Debug.Log("card database, loaded cards : " + cardDatabase.Count());
	}

	void OnDestroy ()
	{
		initialized = false;
		if(CardDatabase.Instance != null)
			CardDatabase.Instance.Destroy();
	}


	#region implemented abstract members of AbstractListDataEditorWindow

	public override void OnKeyDown (KeyCode key)
	{
		if(key == KeyCode.DownArrow || key == KeyCode.RightArrow)
		{
			NextItem ();
		}
		else if(key == KeyCode.UpArrow || key == KeyCode.LeftArrow)
		{
			PreviousItem ();
		}
	}

	protected override bool CreateData ()
	{
		//Initialize();
		return true;
	}

	protected override void AddData ()
	{
		CardData newData = CardDatabase.Instance.AddData ();

		this.SelectData (newData);
	}

	protected override void RemoveData (CardData data)
	{
		if(data == null)
			return;
		int dataId = data.id;
		CardDatabase.Instance.RemoveData (dataId);
	}

	protected override IEnumerable GetDataList ()
	{
		CardData[] currentData = CardDatabase.Instance.cardList;
		for (int i=0; i < currentData.Length; i++) {
			yield return currentData[i];
		}
	}

	protected override int GetDataCount ()
	{
		return CardDatabase.Instance.Count();
	}

	protected override string GetItemName (CardData data)
	{
		return data.name;
	}

	protected override bool LoadFile ()
	{
		Initialize();
		OnSelectedChange( CardDatabase.Instance.GetData(0) );
		return true;
	}

	protected override void SaveFile ()
	{
		CardDatabase.Instance.SaveData();
	}

	protected override void OnSelectedChange(CardData data){
		selectedData = data;
		//force removed focus on selection change so same field won't be selected on change
		GUI.FocusControl ("");

	}

	protected override void OnGUIDataEdit (CardData data)
	{
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.Label("Card: " + data.id + ", " + data.name);
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(10f);

		data.name = EditorGUILayout.TextField("Name", data.name);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Description", GUILayout.Width(100f));
		data.description = EditorGUILayout.TextArea(data.description, GUILayout.Height(40f));
		EditorGUILayout.EndHorizontal();
		data.cardSprite = EditorGUILayout.TextField("Card Sprite", data.cardSprite);

		GUILayout.Space(10f);
		GUILayout.Label("Stats: ");
		data.elementType = (ElementType)EditorGUILayout.Popup((int)data.elementType,elements);
		data.cost = EditorGUILayout.IntField("Cost", data.cost);

		GUILayout.Space(10f);
		GUILayout.Label("Details: ");
		data.cardType = (CardType)EditorGUILayout.Popup((int)data.cardType,cardtypes);
		if(data.cardType == CardType.CHARACTER)
		{
			int newCharID = EditorGUILayout.IntField("Character id", data.dataId);
			if(newCharID != data.dataId)
			{
				data.dataId = newCharID;
				SyncDataWithCharacter(ref data, newCharID);
			}
		}
		else
		{
			int newSkillID = EditorGUILayout.IntField("Skill id", data.dataId);
			if(newSkillID != data.dataId)
			{
				data.dataId = newSkillID;
				SyncDataWithSkill(ref data, newSkillID);
			}
		}
	}

	#endregion

	private void SyncDataWithCharacter(ref CardData data, int characterID)
	{
		data.dataId = characterID;
		CharacterData chData = CharacterDatabase.Instance.GetData(characterID);
		if(chData != null)
		{
			data.name = chData.name;
			data.description = chData.description;
			data.elementType = chData.elementType;
			data.cost = chData.spawnCost;
			data.cardSprite = chData.cardSprite;
		}
	}

	private void SyncDataWithSkill(ref CardData data, int skillID)
	{
		data.dataId = skillID;
		SkillData skData = SkillsDatabase.Instance.GetData(skillID);
		if(skData != null)
		{
			data.name = skData.name;
			data.description = skData.description;
			data.elementType = skData.elementType;
			data.cost = skData.cost;
			data.cardSprite = skData.cardSprite;
		}
	}


	public void NextItem ()
	{
		if(selectedData != null)
		{
			int itemId = selectedData.id;
			if(CardDatabase.Instance.Count() < itemId)
			{
				itemId = 0;
			}
			SelectItemID(itemId);
		}
		else
		{
			SelectItemID(1);
		}
		Repaint();
	}

	public void PreviousItem ()
	{
		if(selectedData != null)
		{
			int itemId = selectedData.id ;
			if(itemId <= 0)
			{
				itemId = CardDatabase.Instance.Count()-1;
			}
			SelectItemID(itemId);
		}
		else
		{
			SelectItemID(1);
		}
		Repaint();
	}

	private void SelectItemID(int itemId)
	{
		if(CardDatabase.Instance != null)
		{
			CardData cData = CardDatabase.Instance.GetData(itemId);
			if(cData != null)
			{
				this.SelectData (cData);
				selectedData = cData;

				Repaint();

			}
		}
	}


}
