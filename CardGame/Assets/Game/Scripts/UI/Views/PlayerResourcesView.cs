using UnityEngine;
using System.Collections;

public class PlayerResourcesView : UIView 
{

	private ElementCounterDisplay airCounter;
	private ElementCounterDisplay landCounter;
	private ElementCounterDisplay waterCounter;


	void Awake ()
	{
		ElementCounterDisplay[] childCounters = GetComponentsInChildren<ElementCounterDisplay>();
		for(int i=0; i < childCounters.Length; i++)
		{
			if(childCounters[i].Element == ElementType.AIR)
				airCounter = childCounters[i];
			else if(childCounters[i].Element == ElementType.LAND)
				landCounter = childCounters[i];
			else if(childCounters[i].Element == ElementType.WATER)
				waterCounter = childCounters[i];
		}

		EventBroadcaster.Instance.AddObserver(EventNames.UI_RESOURCE_COUNTERS_UPDATE, UpdateCounters);
	}


	void OnDestroy ()
	{
		EventBroadcaster.Instance.RemoveObserver(EventNames.UI_RESOURCE_COUNTERS_UPDATE);
	}


	public void UpdateCounters(Parameters elementParams)
	{
		int air = elementParams.GetIntExtra("air", 0);
		int land = elementParams.GetIntExtra("land", 0);
		int water = elementParams.GetIntExtra("water", 0);

		if(airCounter != null)
			airCounter.Amount = air;

		if(landCounter != null)
			landCounter.Amount = land;

		if(waterCounter != null)
			waterCounter.Amount = water;
	}

}
