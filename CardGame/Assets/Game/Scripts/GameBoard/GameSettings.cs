using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour 
{

	public const string PREFS_CELL_ID_KEY = "selected_cell_id";

	[SerializeField] bool testMode = true;
	[SerializeField] int cellAreaId = 1;
	[SerializeField] int level = 1;


	public bool TestMode
	{
		get { return testMode; }
	}


	public int CellAreaID
	{
		get 
		{ 
			int cellId = cellAreaId;
			if(!TestMode)
			{
				cellId = PlayerPrefs.GetInt(PREFS_CELL_ID_KEY, 1);
			}
			return cellId;
		}
	}

	public int Level
	{
		get { return level; }
	}

}
