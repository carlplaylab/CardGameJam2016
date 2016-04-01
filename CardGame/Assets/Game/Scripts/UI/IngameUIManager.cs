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
	[SerializeField] private GameObject refResult;

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

		EventBroadcaster.Instance.AddObserver(EventNames.UI_SHOW_RESULTS, ShowResults);
	}


	void OnDestroy ()
	{
		instance = null;
		EventBroadcaster.Instance.RemoveObserver(EventNames.UI_SHOW_RESULTS);
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


	public void ShowResults (Parameters results)
	{
		GameObject newUI = GameObject.Instantiate(refResult) as GameObject;
		newUI.transform.SetParent(this.transform);
		newUI.transform.localScale = Vector3.one;
		RectTransform rectXform = refResult.GetComponent<RectTransform>();
		RectTransform newRectXform = newUI.GetComponent<RectTransform>();
		newRectXform.localPosition = Vector3.zero;
		newRectXform.anchoredPosition = rectXform.anchoredPosition;

		bool win = results.GetBoolExtra("result", false);
		ResultsView resView = newUI.GetComponent<ResultsView>();
		resView.SetResults(win);
		resView.Show();
	}
}
