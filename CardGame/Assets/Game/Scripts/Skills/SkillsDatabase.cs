using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;


[Serializable]
[JsonName("SkillDatabase")]
public class SkillsDatabase 
{

	public static string BASE_PATH = Application.dataPath + "/Game/Resources/";
	public const string RESOURCE = "Data/skillsdatabase";

	public SkillData[] skillsList = new SkillData[0];

	private Dictionary<int, SkillData> skillsDictionary;

	private static SkillsDatabase instance = null; 
	public static SkillsDatabase Instance
	{
		get 
		{ 
			if(instance == null)
			{
				instance = SkillsDatabase.LoadData();
			}
			return instance;
		}
	}

	public SkillsDatabase ()
	{
		skillsDictionary = new Dictionary<int, SkillData>();
	}

	public void Destroy ()
	{
		skillsList = null;
		skillsDictionary.Clear();
		skillsDictionary = null;
		instance = null;
	}

	public void PreSaveProcessing()
	{
		skillsList = new SkillData[skillsDictionary.Count];
		List<int> keys = new List<int>( skillsDictionary.Keys );
		keys.Sort();
		for(int i=0; i < keys.Count; i++)
		{
			int key = keys[i];
			skillsList[i] = skillsDictionary[key];
		}
	}

	public void PostLoadProcessing()
	{
		if(skillsDictionary == null)
			skillsDictionary = new Dictionary<int, SkillData>();
		else
			skillsDictionary.Clear();

		for(int i = 0; i < skillsList.Length; i++)
		{
			skillsDictionary.Add(skillsList[i].id, skillsList[i]);
		}
	}

	public static SkillsDatabase LoadData()
	{
		if(instance == null)
		{
			TextAsset jsonFile = Resources.Load(RESOURCE) as TextAsset;
			if(jsonFile == null)
			{
				Debug.Log(RESOURCE + " File is NULL, creating new file");
				SkillsDatabase blankDatabase = new SkillsDatabase();
				return blankDatabase;
			}

			string content = jsonFile.text;
			content = content.Trim();

			JsonReaderSettings settings = new JsonReaderSettings();
			settings.TypeHintName = "__Type";

			JsonReader reader = new JsonReader(content);
			SkillsDatabase newData = reader.Deserialize<SkillsDatabase>();

			newData.PostLoadProcessing();

			Debug.Log("Skills Database Data loaded");

			return newData;
		}
		else
		{
			return instance;
		}
	}

	public void SaveData()
	{

		#if UNITY_EDITOR

		Debug.Log("SavePath: " + RESOURCE);

		JsonWriterSettings settings = new JsonWriterSettings();
		settings.PrettyPrint = true;
		settings.TypeHintName = "__Type";

		JsonWriter writer = new JsonWriter(BASE_PATH + RESOURCE + ".json", settings);
		writer.Write(this);
		writer.TextWriter.Flush();
		writer.TextWriter.Close();

		#endif
	}

	public SkillData GetData(int id)
	{
		if(skillsDictionary.ContainsKey(id))
			return skillsDictionary[id];
		else
			return null;
	}

	public int Count ()
	{
		return skillsList.Length;
	}

	public SkillData AddData ()
	{
		int maxId = 0;
		List<int> keys = new List<int>(skillsDictionary.Keys);
		if(keys.Count > 0)
		{
			keys.Sort();
			maxId = keys[ keys.Count-1 ];
		}

		int newId = maxId+1;

		SkillData newData = new SkillData();
		newData.id = newId;
		newData.name = "skill_" + newId;
		skillsDictionary.Add(newData.id, newData);
		PreSaveProcessing();
		return newData;
	}

	public void RemoveData(int dataId)
	{
		if(!skillsDictionary.ContainsKey(dataId))
			return;

		skillsDictionary.Remove(dataId);
		PreSaveProcessing();
		for(int i=0; i < skillsList.Length; i++)
		{
			skillsList[i].id = i;
			if(skillsList[i].name.Contains("skill_"))
				skillsList[i].name = "skill_" + i;
		}
		PostLoadProcessing();
	}
}
