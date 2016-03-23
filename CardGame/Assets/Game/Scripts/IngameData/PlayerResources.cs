using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[SerializeField]
public class PlayerResources
{
	public const int MAX_RESOURCES = 1000;

	public int team = 1;
	private Dictionary<ElementType, int> resourceCounter;

	public PlayerResources (int teamNumber)
	{
		team = teamNumber;
		resourceCounter = new Dictionary<ElementType, int>();
		resourceCounter.Add(ElementType.VOID, 0);
		resourceCounter.Add(ElementType.AIR, 0);
		resourceCounter.Add(ElementType.LAND, 0);
		resourceCounter.Add(ElementType.WATER, 0);
	}


	public void AddResource(ElementType eType, int additionalResources)
	{
		if(resourceCounter.ContainsKey(eType))
		{
			resourceCounter[eType] = Mathf.Clamp(resourceCounter[eType] + additionalResources, 0, MAX_RESOURCES);
		}
	}

	public void AddRandomResources(List<ElementType> eTypes,List<int> weights, int totalCount)
	{
		List<int> elemCounter = new List<int>();
		int totalWt = 0;
		for(int i=0; i < weights.Count; i++)
		{
			totalWt += weights[i];
			elemCounter.Add(0);
		}

		totalWt = Mathf.Clamp(totalWt,1,totalWt);

		for(int i=0; i < totalCount; i++)
		{
			int rand =  UnityEngine.Random.Range(0,totalWt) % totalWt;
			int sum = 0;
			for(int e=0; e < weights.Count; e++)
			{
				sum += weights[e];
				if(rand <= sum)
				{
					elemCounter[e] += 1;
					break;
				}
			}
		}

		for(int i=0; i < elemCounter.Count; i++)
		{
			AddResource(eTypes[i], elemCounter[i]);
		}

		UpdateUI();
	}


	public void SpendResource(ElementType eType, int spentResources)
	{
		if(resourceCounter.ContainsKey(eType))
		{
			resourceCounter[eType] = Mathf.Clamp(resourceCounter[eType] - spentResources, 0, MAX_RESOURCES);
		}
	}


	public int GetResource(ElementType eType)
	{
		if(resourceCounter.ContainsKey(eType))
		{
			return resourceCounter[eType];
		}
		return 0;
	}


	public int LandResources
	{
		get { return GetResource(ElementType.LAND); }
	}


	public int AirResources
	{
		get { return GetResource(ElementType.AIR); }
	}


	public int WaterResources
	{
		get { return GetResource(ElementType.WATER); }
	}


	public void UpdateUI()
	{
		Parameters elemParams = new Parameters();
		elemParams.PutExtra("team", team);
		elemParams.PutExtra("air", AirResources);
		elemParams.PutExtra("land", LandResources);
		elemParams.PutExtra("water", WaterResources);
		EventBroadcaster.Instance.PostEvent(EventNames.UI_RESOURCE_COUNTERS_UPDATE, elemParams);
	}


	public string GetStats ()
	{
		string stats = "Elements: ";
		foreach(ElementType etype in resourceCounter.Keys)
		{
			stats += etype.ToString() + " " + resourceCounter[etype] + ", ";
		}
		return stats;
	}

}
