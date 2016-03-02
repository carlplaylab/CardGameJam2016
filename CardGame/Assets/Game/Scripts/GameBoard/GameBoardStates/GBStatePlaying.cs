using UnityEngine;
using System.Collections;

public class GBStatePlaying : GBState 
{

	public override BoardState GetState()
	{
		return BoardState.PLAYER_TURN;
	}

	public override void Start (GameBoard board)
	{
		board.InputEnable = true;
		Debug.Log("input enabled");
	}

	public override void Update (GameBoard board)
	{
	}

	public override void End (GameBoard board)
	{
		board.InputEnable = false;
	}
}
