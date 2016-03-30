using UnityEngine;
using System.Collections;

public class ResultsView : UIView 
{

	[SerializeField] private GameObject winObj;
	[SerializeField] private GameObject loseObj;

	public void SetResults(bool win)
	{
		winObj.SetActive(win);
		loseObj.SetActive(!win);
	}

	public void OnButtonClicked ()
	{
		Hide();

		GameBoardManager.Instance.LeaveGameScene();
	}
}
