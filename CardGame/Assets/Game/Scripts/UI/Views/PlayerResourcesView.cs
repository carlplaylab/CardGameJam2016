using UnityEngine;
using System.Collections;

public class PlayerResourcesView : UIView 
{

	[SerializeField] private ElementCounterDisplay airCounter;
	[SerializeField] private ElementCounterDisplay landCounter;
	[SerializeField] private ElementCounterDisplay waterCounter;

	[SerializeField] private ElementCounterDisplay airCounter_opponent;
	[SerializeField] private ElementCounterDisplay landCounter_opponent;
	[SerializeField] private ElementCounterDisplay waterCounter_opponent;


	void Awake ()
	{
		EventBroadcaster.Instance.AddObserver(EventNames.UI_RESOURCE_COUNTERS_UPDATE, UpdateCounters);
	}


	void OnDestroy ()
	{
		EventBroadcaster.Instance.RemoveObserver(EventNames.UI_RESOURCE_COUNTERS_UPDATE);
	}


	public void UpdateCounters(Parameters elementParams)
	{
		int team = elementParams.GetIntExtra("team", 1);
		int air = elementParams.GetIntExtra("air", 0);
		int land = elementParams.GetIntExtra("land", 0);
		int water = elementParams.GetIntExtra("water", 0);

		if(team == GameBoard.PLAYER_TEAM)
		{
			airCounter.Amount = air;
			landCounter.Amount = land;
			waterCounter.Amount = water;
		}
		else
		{
			airCounter_opponent.Amount = air;
			landCounter_opponent.Amount = land;
			waterCounter_opponent.Amount = water;
		}
	}

}
