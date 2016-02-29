using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;


[Serializable]
[JsonName("CharacterDatabase")]
public class CharacterDatabase 
{
	public static string BASE_PATH = Application.dataPath + "/Game/Resources/";
	public const string RESOURCE = "Data/characterdatabase";

	public CharacterData[] characterList = new CharacterData[0];

	private Dictionary<int, CharacterData> characterDictionary;

	private static CharacterDatabase instance = null; 
	public static CharacterDatabase Instance
	{
		get 
		{ 
			if(instance == null)
			{
				instance = CharacterDatabase.LoadData();
			}
			return instance;
		}
	}


	public CharacterDatabase ()
	{
		characterDictionary = new Dictionary<int, CharacterData>();
	}

	public void Destroy ()
	{
		characterList = null;
		characterDictionary.Clear();
		characterDictionary = null;
		instance = null;
	}

	public void PreSaveProcessing()
	{
		characterList = new CharacterData[characterDictionary.Count];
		List<int> keys = new List<int>( characterDictionary.Keys );
		keys.Sort();
		for(int i=0; i < keys.Count; i++)
		{
			int key = keys[i];
			characterList[i] = characterDictionary[key];
		}
	}

	public void PostLoadProcessing()
	{
		if(characterDictionary == null)
			characterDictionary = new Dictionary<int, CharacterData>();
		else
			characterDictionary.Clear();
		
		for(int i = 0; i < characterList.Length; i++)
		{
			characterDictionary.Add(characterList[i].id, characterList[i]);
		}
	}

	public static CharacterDatabase LoadData()
	{
		if(instance == null)
		{
			TextAsset jsonFile = Resources.Load(RESOURCE) as TextAsset;
			if(jsonFile == null)
			{
				Debug.Log(RESOURCE + " File is NULL, creating new file");
				CharacterDatabase blankDatabase = new CharacterDatabase();
				return blankDatabase;
			}

			string content = jsonFile.text;
			content = content.Trim();

			JsonReaderSettings settings = new JsonReaderSettings();
			settings.TypeHintName = "__Type";

			JsonReader reader = new JsonReader(content);
			CharacterDatabase newData = reader.Deserialize<CharacterDatabase>();

			newData.PostLoadProcessing();

			//Debug.Log("Data loaded");

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

		//Debug.Log("SavePath: " + RESOURCE);

		JsonWriterSettings settings = new JsonWriterSettings();
		settings.PrettyPrint = true;
		settings.TypeHintName = "__Type";

		JsonWriter writer = new JsonWriter(BASE_PATH + RESOURCE + ".json", settings);
		writer.Write(this);
		writer.TextWriter.Flush();
		writer.TextWriter.Close();

		#endif
	}

	public CharacterData GetData(int id)
	{
		if(characterDictionary.ContainsKey(id))
			return characterDictionary[id];
		else
			return null;
	}

	public int Count ()
	{
		return characterList.Length;
	}

	public CharacterData AddData ()
	{
		int maxId = 0;
		List<int> keys = new List<int>(characterDictionary.Keys);
		if(keys.Count > 0)
		{
			keys.Sort();
			maxId = keys[ keys.Count-1 ];
		}

		int newId = maxId+1;

		CharacterData newData = new CharacterData();
		newData.id = newId;
		newData.name = "character_" + newId;
		characterDictionary.Add(newData.id, newData);
		PreSaveProcessing();
		return newData;
	}

	public void RemoveData(int dataId)
	{
		if(!characterDictionary.ContainsKey(dataId))
			return;

		characterDictionary.Remove(dataId);
		PreSaveProcessing();
		for(int i=0; i < characterList.Length; i++)
		{
			characterList[i].id = i;
			if(characterList[i].name.Contains("character"))
				characterList[i].name = "character_" + i;
		}
		PostLoadProcessing();
	}


}