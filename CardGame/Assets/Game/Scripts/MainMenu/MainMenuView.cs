using UnityEngine;
using System.Collections;

public class MainMenuView : UIView 
{
	[SerializeField] MapView mapView;

	void Awake ()
	{
		viewState = UIViewState.VISIBlE;
	}

	public void OnPlayClicked ()
	{
		this.Hide();
		mapView.Show();

		SoundManager.PlaySound("click");
	}
}
