using UnityEngine;
using System.Collections;

public class GBStateOponentsTurn : GBState 
{

	public override BoardState GetState()
	{
		return BoardState.OPONENTS_TURN;
	}

	public override void Start (GameBoard board)
	{
	}

	public override void Update (GameBoard board)
	{
	}

	public override void End (GameBoard board)
	{
	}
}
