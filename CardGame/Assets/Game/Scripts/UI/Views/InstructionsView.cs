using UnityEngine;
using System.Collections;

public class InstructionsView : UIView 
{

	[SerializeField] GameObject buttonObject;
	[SerializeField] GameObject infoObject;

	void Awake ()
	{
		buttonObject.SetActive(true);

		int instructionCounter = PlayerPrefs.GetInt("infocount", 0);
		if(instructionCounter <= 0)
		{
			infoObject.SetActive(true);
			instructionCounter++;
			PlayFx();
		}
		PlayerPrefs.SetInt("infocount", instructionCounter);
	}

	public void OnInfoButtonClicked ()
	{
		infoObject.SetActive ( !infoObject.activeSelf );
		PlayFx();
	}


	public void OnInstructionsCloseClicked ()
	{
		infoObject.SetActive(false);
		PlayFx(true);
	}

	private void PlayFx (bool ignoreActive = false)
	{
		if(infoObject.activeSelf || ignoreActive)
		{
			SoundManager.PlaySound("window_in");
		}
	}
}
