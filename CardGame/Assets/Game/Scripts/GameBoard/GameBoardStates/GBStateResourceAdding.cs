using UnityEngine;
using System.Collections;

public class GBStateResourceAdding : GBState 
{
	private bool started = false;


	public override BoardState GetState()
	{
		return BoardState.RESOURCE_ADDING;
	}

	public override void Start (GameBoard board)
	{
		started = true;
	}

	public override void Update (GameBoard board)
	{
		if(started)
		{
			GameBoardManager.Instance.SetState(BoardState.PLAYER_TURN);
			started = false;
		}
	}

	public override void End (GameBoard board)
	{
	}


}
