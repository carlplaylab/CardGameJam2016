using UnityEngine;
using System.Collections;


public enum UIViewState
{
	HIDDEN = 0,
	ENTERING = 1,
	VISIBlE = 2,
	LEAVING = 3
}

public class UIView : MonoBehaviour 
{
	[SerializeField] protected Vector3 activePosition;
	[SerializeField] protected Vector3 hiddenPosition;

	protected UIViewState viewState;
	protected RectTransform rect;


	public UIViewState ViewState
	{
		get { return viewState; }
	}


	public virtual void Show ()
	{
		if(ViewState == UIViewState.ENTERING || ViewState == UIViewState.VISIBlE)
			return;

		this.gameObject.SetActive(true);
		SetState(UIViewState.VISIBlE);
	}


	public virtual void Hide ()
	{
		if(ViewState == UIViewState.LEAVING || ViewState == UIViewState.HIDDEN)
			return;

		this.gameObject.SetActive(false);
		SetState(UIViewState.HIDDEN);
	}


	public virtual void SetState(UIViewState newState)
	{
		viewState = newState;
	}

	public virtual void PostLoadProcess ()
	{
	}

	public bool CheckTouchInRectangle()
	{
		if(rect == null)
			rect = GetComponent<RectTransform>();
		
		Vector2 screenPt = (Vector2)Input.mousePosition;
		return RectTransformUtility.RectangleContainsScreenPoint(rect, screenPt, IngameUIManager.Instance.UiCamera);
	}

	public void SetActivePosition(bool active)
	{
		Vector3 pos = active ? activePosition : hiddenPosition;
		RectTransform rect = GetComponent<RectTransform>();
		if(rect != null)
		{
			rect.anchoredPosition3D = pos;
		}
	}
}
