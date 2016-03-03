using UnityEngine;
using System.Collections;

[SerializeField]
public class PlayerIngameData 
{

	private int team;

	private PlayerResources resourceData;


	public int Team
	{
		get { return team; }
	}

	public PlayerResources ResourceData
	{
		get { return resourceData; }
	}

	public PlayerIngameData (int teamID)
	{
		team = teamID;
		resourceData = new PlayerResources();
	}

	public string GetStats ()
	{
		string stats = "Player " + Team + " \n";
		stats += resourceData.GetStats() + "\n";
		return stats;
	}

	public bool HasEnoughResource(ElementType resType, int neededAmount)
	{
		int currentRes = ResourceData.GetResource(resType);
		return currentRes >= neededAmount;
	}
}
