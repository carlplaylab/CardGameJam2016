using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[SerializeField]
public class PlayerResources
{
	public const int MAX_RESOURCES = 10;

	private Dictionary<ElementType, int> resourceCounter;

	public PlayerResources ()
	{
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
}
