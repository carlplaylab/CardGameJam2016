using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;


[Serializable]
[JsonName("CardDatabase")]
public class CardDatabase 
{
	public static string BASE_PATH = Application.dataPath + "/Game/Resources/";
	public const string RESOURCE = "Data/carddatabase";

	public CardData[] cardList = new CardData[0];

	private Dictionary<int, CardData> cardDictionary;

	private static CardDatabase instance = null; 
	public static CardDatabase Instance
	{
		get 
		{ 
			if(instance == null)
			{
				instance = CardDatabase.LoadData();
			}
			return instance;
		}
	}

	public CardDatabase ()
	{
		cardDictionary = new Dictionary<int, CardData>();
	}

	public void Destroy ()
	{
		cardList = null;
		cardDictionary.Clear();
		cardDictionary = null;
		instance = null;
	}

	public void PreSaveProcessing()
	{
		cardList = new CardData[cardDictionary.Count];
		List<int> keys = new List<int>( cardDictionary.Keys );
		keys.Sort();
		for(int i=0; i < keys.Count; i++)
		{
			int key = keys[i];
			cardList[i] = cardDictionary[key];
		}
	}

	public void PostLoadProcessing()
	{
		if(cardDictionary == null)
			cardDictionary = new Dictionary<int, CardData>();
		else
			cardDictionary.Clear();

		for(int i = 0; i < cardList.Length; i++)
		{
			cardDictionary.Add(cardList[i].id, cardList[i]);
		}
	}

	public static CardDatabase LoadData()
	{
		if(instance == null)
		{
			TextAsset jsonFile = Resources.Load(RESOURCE) as TextAsset;
			if(jsonFile == null)
			{
				Debug.Log(RESOURCE + " File is NULL, creating new file");
				CardDatabase blankDatabase = new CardDatabase();
				return blankDatabase;
			}

			string content = jsonFile.text;
			content = content.Trim();

			JsonReaderSettings settings = new JsonReaderSettings();
			settings.TypeHintName = "__Type";

			JsonReader reader = new JsonReader(content);
			CardDatabase newData = reader.Deserialize<CardDatabase>();

			newData.PostLoadProcessing();

			Debug.Log("Data loaded");

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

	public CardData GetData(int id)
	{
		if(cardDictionary.ContainsKey(id))
			return cardDictionary[id];
		else
			return null;
	}

	public int Count ()
	{
		return cardList.Length;
	}

	public CardData AddData ()
	{
		int maxId = 0;
		List<int> keys = new List<int>(cardDictionary.Keys);
		if(keys.Count > 0)
		{
			keys.Sort();
			maxId = keys[ keys.Count-1 ];
		}

		int newId = maxId+1;

		CardData newData = new CardData();
		newData.id = newId;
		newData.name = "card_" + newId;
		cardDictionary.Add(newData.id, newData);
		PreSaveProcessing();
		return newData;
	}

	public void RemoveData(int dataId)
	{
		if(!cardDictionary.ContainsKey(dataId))
			return;

		cardDictionary.Remove(dataId);
		PreSaveProcessing();
		for(int i=0; i < cardList.Length; i++)
		{
			cardList[i].id = i;
			if(cardList[i].name.Contains("card"))
				cardList[i].name = "card_" + i;
		}
		PostLoadProcessing();
	}

}
