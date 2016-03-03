using UnityEngine;
using System.Collections;

public class GBStatePlaying : GBState 
{

	int charId = 1;

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
		if(Input.GetKeyDown(KeyCode.C))
		{
			Parameters charParams = new Parameters();
			charParams.PutExtra("character", charId);
			EventBroadcaster.Instance.PostEvent(EventNames.UI_SHOW_CHARACTER_CARD, charParams);
			charId ++;
			if(charId >= CharacterDatabase.Instance.Count())
				charId = 1;
		}
	}

	public override void End (GameBoard board)
	{
		board.InputEnable = false;
	}
}
