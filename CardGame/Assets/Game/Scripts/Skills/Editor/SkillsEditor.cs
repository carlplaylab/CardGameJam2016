using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class SkillsEditor : AbstractListDataEditorWindow<SkillData>  
{

	[MenuItem ("MyMenu/Do Something with a Shortcut Key %k")]
	static void DoSomethingWithAShortcutKey () 
	{
		GetWindow();
	}

	[MenuItem("Card Game Tools/Skills Editor %k")]
	public static void GetWindow()
	{
		SkillsEditor skillEditor = EditorWindow.GetWindow<SkillsEditor>("Skills Database", true);
		skillEditor.minSize = new Vector2(1024f, 400f);
	}

	private bool initialized = false;
	private SkillData selectedData = null;
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
		SkillsDatabase skillDatabase = SkillsDatabase.Instance;
		Debug.Log("skills database, loaded skills : " + skillDatabase.Count());
	}

	void OnDestroy ()
	{
		initialized = false;
		if(SkillsDatabase.Instance != null)
			SkillsDatabase.Instance.Destroy();
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
		SkillData newData = SkillsDatabase.Instance.AddData ();

		this.SelectData (newData);
	}

	protected override void RemoveData (SkillData data)
	{
		if(data == null)
			return;
		int dataId = data.id;
		SkillsDatabase.Instance.RemoveData (dataId);
	}

	protected override IEnumerable GetDataList ()
	{
		SkillData[] currentData = SkillsDatabase.Instance.skillsList;
		for (int i=0; i < currentData.Length; i++) {
			yield return currentData[i];
		}
	}

	protected override int GetDataCount ()
	{
		return SkillsDatabase.Instance.Count();
	}

	protected override string GetItemName (SkillData data)
	{
		return data.name;
	}

	protected override bool LoadFile ()
	{
		Initialize();
		OnSelectedChange( SkillsDatabase.Instance.GetData(0) );
		return true;
	}

	protected override void SaveFile ()
	{
		SkillsDatabase.Instance.SaveData();
	}

	protected override void OnSelectedChange(SkillData data){
		selectedData = data;
		//force removed focus on selection change so same field won't be selected on change
		GUI.FocusControl ("");

	}

	protected override void OnGUIDataEdit (SkillData data)
	{
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.Label("Skill: " + data.id + ", " + data.name);
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(10f);

		data.name = EditorGUILayout.TextField("Name", data.name);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Description", GUILayout.Width(100f));
		data.description = EditorGUILayout.TextArea(data.description, GUILayout.Height(40f));
		EditorGUILayout.EndHorizontal();
		data.visualsName = EditorGUILayout.TextField("Skill Visual Prefab", data.visualsName);

		GUILayout.Space(10f);
		GUILayout.Label("Stats: ");
		data.elementType = (ElementType)EditorGUILayout.Popup((int)data.elementType,elements);
		data.cost = EditorGUILayout.IntField("Cost", data.cost);
		data.areaCol = EditorGUILayout.IntField("Area Col", data.areaCol);
		data.areaRow = EditorGUILayout.IntField("Area Row", data.areaRow);
		data.damage = EditorGUILayout.IntField("Damage", data.damage);
	}

	#endregion

	public void NextItem ()
	{
		if(selectedData != null)
		{
			int itemId = selectedData.id;
			if(SkillsDatabase.Instance.Count() < itemId)
			{
				itemId = 0;
			}
			SelectItemID(itemId);
		}
		else
		{
			SelectItemID(1);
		}
	}

	public void PreviousItem ()
	{
		if(selectedData != null)
		{
			int itemId = selectedData.id ;
			if(itemId <= 0)
			{
				itemId = SkillsDatabase.Instance.Count()-1;
			}
			SelectItemID(itemId);
		}
		else
		{
			SelectItemID(1);
		}
	}

	private void SelectItemID(int itemId)
	{
		if(SkillsDatabase.Instance != null)
		{
			SkillData cData = SkillsDatabase.Instance.GetData(itemId);
			if(cData != null)
			{
				this.SelectData (cData);
				selectedData = cData;

				Repaint();

			}
		}
	}
}
