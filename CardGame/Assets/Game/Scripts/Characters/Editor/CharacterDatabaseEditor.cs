using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CharacterDatabaseEditor : AbstractListDataEditorWindow<CharacterData>  
{
	
	[MenuItem ("MyMenu/Do Something with a Shortcut Key %e")]
	static void DoSomethingWithAShortcutKey () 
	{
		GetWindow();
	}

	[MenuItem("Card Game Tools/Character Database %e")]
	public static void GetWindow()
	{
		CharacterDatabaseEditor charEditor = EditorWindow.GetWindow<CharacterDatabaseEditor>("Character Database", true);
		charEditor.minSize = new Vector2(1024f, 400f);
	}


	private bool initialized = false;
	private CharacterData selectedData = null;
	private string[] elements;

	public void Initialize ()
	{
		if(initialized)
			return;
		
		elements = new string[4];
		for(int i=0; i < 4; i++)
		{
			elements[i] = ((ElementType)i).ToString();
		}

		// making sure it loads
		CharacterDatabase charDatabse = CharacterDatabase.Instance;
		Debug.Log("character database, loaded chars : " + charDatabse.Count());
	}


	void OnDestroy ()
	{
		initialized = false;
		if(CharacterDatabase.Instance != null)
			CharacterDatabase.Instance.Destroy();
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
		CharacterData newData = CharacterDatabase.Instance.AddData ();

		this.SelectData (newData);
	}

	protected override void RemoveData (CharacterData data)
	{
		if(data == null)
			return;
		int dataId = data.id;
		CharacterDatabase.Instance.RemoveData (dataId);
	}

	protected override IEnumerable GetDataList ()
	{
		CharacterData[] currentData = CharacterDatabase.Instance.characterList;
		for (int i=0; i < currentData.Length; i++) {
			yield return currentData[i];
		}
	}

	protected override int GetDataCount ()
	{
		return CharacterDatabase.Instance.Count();
	}

	protected override string GetItemName (CharacterData data)
	{
		return data.name;
	}

	protected override bool LoadFile ()
	{
		Initialize();
		OnSelectedChange( CharacterDatabase.Instance.GetData(0) );
		return true;
	}

	protected override void SaveFile ()
	{
		CharacterDatabase.Instance.SaveData();
	}

	protected override void OnSelectedChange(CharacterData data){
		selectedData = data;
		//force removed focus on selection change so same field won't be selected on change
		GUI.FocusControl ("");

	}

	protected override void OnGUIDataEdit (CharacterData data)
	{
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.Label("Character: " + data.id + ", " + data.name);
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(10f);

		data.name = EditorGUILayout.TextField("Name", data.name);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Description", GUILayout.Width(100f));
		data.description = EditorGUILayout.TextArea(data.description, GUILayout.Height(40f));
		EditorGUILayout.EndHorizontal();
		data.characterPrefab = EditorGUILayout.TextField("Prefab", data.characterPrefab);
		data.atlas = EditorGUILayout.TextField("Atlas", data.atlas);
		data.cardSprite = EditorGUILayout.TextField("Card Sprite", data.cardSprite);
		data.ingameSprite = EditorGUILayout.TextField("Card Sprite", data.ingameSprite);

		GUILayout.Space(10f);
		GUILayout.Label("Stats: ");
		data.elementType = (ElementType)EditorGUILayout.Popup((int)data.elementType,elements);
		data.spawnCost = EditorGUILayout.IntField("Spawn Cost", data.spawnCost);
		data.movement = EditorGUILayout.IntField("Movement", data.movement);
		data.attack = EditorGUILayout.IntField("Attack", data.attack);
		data.life = EditorGUILayout.IntField("Life", data.life);

	}

	#endregion


	public void NextItem ()
	{
		if(selectedData != null)
		{
			int itemId = selectedData.id;
			if(CharacterDatabase.Instance.Count() < itemId)
			{
				itemId = 0;
			}
			SelectItemID(itemId);
		}
	}

	public void PreviousItem ()
	{
		if(selectedData != null)
		{
			int itemId = selectedData.id ;
			if(itemId <= 0)
			{
				itemId = CharacterDatabase.Instance.Count()-1;
			}
			SelectItemID(itemId);
		}
	}

	private void SelectItemID(int itemId)
	{
		if(CharacterDatabase.Instance != null)
		{
			CharacterData cData = CharacterDatabase.Instance.GetData(itemId);
			if(cData != null)
			{
				this.SelectData (cData);
				selectedData = cData;

				Repaint();

			}
		}
	}

}
