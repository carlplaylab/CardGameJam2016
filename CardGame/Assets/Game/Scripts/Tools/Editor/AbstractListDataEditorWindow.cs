using UnityEditor;
using UnityEngine;
using System.Collections;

public abstract class AbstractListDataEditorWindow<T> : EditorWindow where T : class {

	protected bool isInitialized = false;
	bool successLoad = false;
	T selectedData = null;
	Vector2 scrollList = Vector2.zero;

	protected abstract bool CreateData();
	protected abstract void AddData();
	protected abstract void RemoveData(T data);
	protected abstract IEnumerable GetDataList();
	protected abstract int GetDataCount();
	protected abstract string GetItemName(T data);
	protected abstract void OnGUIDataEdit(T data);
	protected abstract bool LoadFile();
	protected abstract void SaveFile();
	protected abstract void OnSelectedChange (T data);

	protected virtual void InitializeEditor(){
	
	}


	protected virtual string GetCreateFileMessage()
	{
		return "Press this to create Data File";
	}

	protected virtual string GetListName()
	{
		return "Data List";
	}

	void OnEnable()
	{
		successLoad = LoadFile();
	}

	protected void OnGUI()
	{
		Event e = Event.current;
		if(e.type == EventType.KeyDown)
		{
			OnKeyDown(e.keyCode);
		}

		if (!successLoad) ShowNewFile();
		else ShowEdit();
	}

	void ShowNewFile()
	{
		EditorGUILayout.BeginVertical();
		GUILayout.Label(GetCreateFileMessage());
		if (GUILayout.Button("Create Data"))
		{
			successLoad = CreateData();
			SaveFile();
		}
		EditorGUILayout.EndVertical();
	}

  	void ShowEdit()
	{
		EditorGUILayout.BeginVertical();
		EditorGUILayout.BeginHorizontal();
		ShowList();
		ShowDataEdit();
		EditorGUILayout.EndHorizontal();
		ShowFileControl();
		EditorGUILayout.EndVertical();
	}

	void ShowList()
	{
		EditorGUILayout.BeginVertical("box", GUILayout.Width(200));
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.Label(GetListName());
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Add", EditorStyles.toolbarButton)) AddData();
		EditorGUILayout.EndHorizontal();
		scrollList = EditorGUILayout.BeginScrollView(scrollList);
		
		if (GetDataCount() == 0) GUILayout.Label("Add Data");
		else 
		{
			foreach (T data in GetDataList())
			{
				if (GUILayout.Button(GetItemName(data), data == selectedData ? EditorStyles.whiteLabel : EditorStyles.label)){
					if(selectedData != data){

						selectedData = data;
						OnSelectedChange(selectedData);
					}
				}
			}
		}
		
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	void ShowDataEdit()
	{
		if (selectedData == null) return;
		bool removeSelected = false;
		bool duplicateSelected = false;
		EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
		GUILayout.Label(GetItemName(selectedData));
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Duplicate", EditorStyles.toolbarButton)) duplicateSelected = true;
		if (GUILayout.Button("Remove", EditorStyles.toolbarButton)) removeSelected = true;
		EditorGUILayout.EndHorizontal();
		
		OnGUIDataEdit(selectedData);
		
		EditorGUILayout.EndVertical();
		if (removeSelected)
		{
			RemoveData(selectedData);
			selectedData = null;
		}

		if(duplicateSelected){
			DuplicateData(selectedData);
		}
	}

	public virtual void DuplicateData(T selectedData){

	}

	public void SelectData(T newSelected){
		selectedData = newSelected;
	}

	void ShowFileControl()
	{
		EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
		if (GUILayout.Button("Save", EditorStyles.toolbarButton)) SaveFile();
		GUILayout.Space(10);
		if (GUILayout.Button("Load", EditorStyles.toolbarButton)) LoadFile();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}

	public virtual void OnKeyDown (KeyCode key)
	{
	}
}
