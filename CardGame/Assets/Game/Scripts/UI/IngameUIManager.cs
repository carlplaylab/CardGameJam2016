using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IngameUIManager : MonoBehaviour 
{
	private static IngameUIManager instance;
	public static IngameUIManager Instance
	{
		get { return instance; }
	}

	[SerializeField] private GameObject[] uiToLoad;

	private Camera uiCamera;
	private RectTransform uiRect;
	private bool initialized = false;



	public Camera UiCamera
	{
		get { return uiCamera; }
	}


	void Awake ()
	{
		instance = this;

		uiCamera = GetComponent<Canvas>().worldCamera;
		uiRect = GetComponent<RectTransform>();
	}


	void OnDestroy ()
	{
		instance = null;
	}


	public void Initialize ()
	{
		if(initialized)
			return;

		initialized = true;
		for(int i=0; i < uiToLoad.Length; i++)
		{
			GameObject newUI = GameObject.Instantiate( uiToLoad[i] ) as GameObject;
			newUI.transform.SetParent(this.transform);
			newUI.transform.localScale = Vector3.one;
			RectTransform rectXform = uiToLoad[i].GetComponent<RectTransform>();
			RectTransform newRectXform = newUI.GetComponent<RectTransform>();
			newRectXform.localPosition = rectXform.localPosition;
			newRectXform.anchoredPosition = rectXform.anchoredPosition;
			newUI.name = uiToLoad[i].name;

			UIView uiview = newUI.GetComponent<UIView>();
			if(uiview != null)
			{
				uiview.PostLoadProcess();
			}
		}
	}


	public Vector3 GetMousePosition ()
	{
		Vector3 mousePos = uiCamera.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		return mousePos;
	}


}
