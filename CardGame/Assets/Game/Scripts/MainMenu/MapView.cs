using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class MapView : UIView 
{

	void Awake ()
	{
		viewState = UIViewState.HIDDEN;
	}

	public override void Show ()
	{
		if(ViewState == UIViewState.ENTERING || ViewState == UIViewState.VISIBlE)
			return;

		this.gameObject.SetActive(true);
		SetState(UIViewState.VISIBlE);

		this.SetActivePosition(true);
	}

	public void OnLevel1Clicked ()
	{
		LevelClicked(1);
		GoToGame();
	}

	public void OnLevel2Clicked ()
	{
		LevelClicked(2);
		GoToGame();
	}

	public void OnLevel3Clicked ()
	{
		LevelClicked(3);
		GoToGame();
	}

	private void GoToGame ()
	{
		SceneManager.LoadScene("GameScene");
	}

	private void LevelClicked(int level)
	{
		PlayerPrefs.SetInt("level", level);
	}
}
