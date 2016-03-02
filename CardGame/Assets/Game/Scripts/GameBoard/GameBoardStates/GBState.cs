using UnityEngine;
using System.Collections;



public enum BoardState
{
	LOADING = 0,
	RESOURCE_ADDING = 1,
	PLAYER_TURN = 2,
	OPONENTS_TURN = 3
}


public class GBState
{

	public virtual BoardState GetState()
	{
		return BoardState.PLAYER_TURN;
	}

	public virtual void Start (GameBoard board)
	{
	}

	public virtual void Update (GameBoard board)
	{
	}

	public virtual void End (GameBoard board)
	{
	}


}
