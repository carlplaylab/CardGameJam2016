using UnityEngine;
using System.Collections;

public class ResultsView : UIView 
{

	[SerializeField] private GameObject winObj;
	[SerializeField] private GameObject loseObj;

	public void SetResults(bool win)
	{
		GameBoardManager.Instance.Board.InputEnable = false;

		winObj.SetActive(win);
		loseObj.SetActive(!win);

		string sfx = win ? "success" : "fail";
		SoundManager.PlaySound(sfx);
	}

	public void OnButtonClicked ()
	{
		Hide();

		GameBoardManager.Instance.LeaveGameScene();
	}
}
