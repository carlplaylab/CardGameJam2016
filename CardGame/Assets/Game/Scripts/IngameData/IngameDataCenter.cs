using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class IngameDataCenter 
{

	private static IngameDataCenter instance;
	public static IngameDataCenter Instance
	{
		get { return instance; }
	}

	private List<PlayerIngameData> dataList;


	public IngameDataCenter ()
	{
		dataList = new List<PlayerIngameData>();
		PlayerIngameData player1 = new PlayerIngameData(1);
		PlayerIngameData player2 = new PlayerIngameData(2);
		dataList.Add(player1);
		dataList.Add(player2);
	}


	public static IngameDataCenter Initialize ()
	{
		if(instance == null)
		{
			instance = new IngameDataCenter();
		}
		return instance;
	}


	public void Destroy ()
	{
		if(dataList != null)
			dataList.Clear();

		instance = null;
	}


	public PlayerIngameData Player1
	{
		get
		{
			if(dataList.Count > 0)
				return dataList[0];
			else
				return null;
		}
	}


	public PlayerIngameData Player2
	{
		get
		{
			if(dataList.Count > 1)
				return dataList[1];
			else
				return null;
		}
	}


	public PlayerIngameData GetPlayerData(int team)
	{
		if(dataList != null)
		{
			if(team == 1)
				return Player1;
			else
				return Player2;
		}
		return null;
	}


	public void LogStats ()
	{
		Debug.Log("INGAME DATA CENTER Stats");
		Debug.Log( Player1.GetStats() );
		Debug.Log( Player2.GetStats() );
	}


	public void SpendResource(int team, ElementType etype, int  cost)
	{
		PlayerIngameData player = GetPlayerData(team);
		if(player != null)
		{
			player.ResourceData.SpendResource(etype, cost);

			if(team == 1)
			{
				player.ResourceData.UpdateUI();
			}
		}
	}

}
