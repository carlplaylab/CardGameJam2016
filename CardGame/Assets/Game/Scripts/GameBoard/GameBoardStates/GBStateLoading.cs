using UnityEngine;
using System.Collections;

public class GBStateLoading : GBState
{

	private int loadingCount = 0;


	public override void Start (GameBoard board)
	{
		loadingCount = 0;
	}

	public override void Update (GameBoard board)
	{
		if(loadingCount == 0)
		{
			// Insert loading of cells
		}
		else if(loadingCount == 1)
		{
			// Insert loading of character data
		}
		else if(loadingCount == 2)
		{
			// Insert loading of character reference objects
		}
		else if(loadingCount == 3)
		{
			// Insert creation of characters in team 1
		}
		else if(loadingCount == 4)
		{
			// Insert creation of characters in team 2
		}
		else if(loadingCount == 5)
		{
			// End loading
		}

		loadingCount++;
	}

	public override void End (GameBoard board)
	{
	}
}
