using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour 
{

	public const string PREFS_CELL_ID_KEY = "level";

	[SerializeField] bool testMode = true;
	[SerializeField] int cellAreaId = 1;
	[SerializeField] int level = 1;



	void Awake ()
	{
		if(!TestMode || !Debug.isDebugBuild || !Application.isEditor)
		{
			level = PlayerPrefs.GetInt(PREFS_CELL_ID_KEY, 1);
			cellAreaId = level;
		}
	}

	public bool TestMode
	{
		get { return testMode; }
	}


	public int CellAreaID
	{
		get 
		{ 
			return cellAreaId;
		}
	}

	public int Level
	{
		get 
		{ 
			return level;
		}
	}

}
