using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;



public class DraggableUIView : UIView
{
	[SerializeField] private Vector3 focusedScale;
	[SerializeField] private float offsetY = 0.05f;

	private bool isFocused = false;
	private bool isDragged = false;
	protected Vector3 preDragPosition;


	public bool IsFocused
	{
		get { return isFocused; }
	}

	public bool IsDragged
	{
		get { return isDragged; }
	}


	void Awake ()
	{
		Initialize();
	}


	public virtual void Initialize ()
	{
		rect = GetComponent<RectTransform>();

		Button button = GetComponentInChildren<Button>();
		button.onClick.AddListener ( OnClick );

		EventTrigger eTrigger = button.gameObject.GetComponent<EventTrigger>();

		EventTrigger.Entry beginDrag = new EventTrigger.Entry();
		beginDrag.eventID = EventTriggerType.BeginDrag;
		beginDrag.callback.AddListener(OnBeginDrag);
		eTrigger.triggers.Add(beginDrag);

		EventTrigger.Entry drag = new EventTrigger.Entry();
		drag.eventID = EventTriggerType.Drag;
		drag.callback.AddListener(OnDrag);
		eTrigger.triggers.Add(drag);

		EventTrigger.Entry endDrag = new EventTrigger.Entry();
		endDrag.eventID = EventTriggerType.EndDrag;
		endDrag.callback.AddListener(OnEndDrag);
		eTrigger.triggers.Add(endDrag);
	}


	public void OnClick ()
	{
		if(isDragged)
			return;
		
		//Debug.Log("OnClick");
		SetFocused(!isFocused);
		isDragged = false;

		OnClickEffect();
	}


	public void OnBeginDrag (BaseEventData eventdata)
	{
		//Debug.Log("OnBeginDrag");
		preDragPosition = this.gameObject.transform.position;
		SetFocused(true);
		isDragged = true;

		OnBeginDragEffect();
	}


	public void OnEndDrag (BaseEventData eventdata)
	{
		//Debug.Log("OnEndDrag");
		SetFocused(false);
		isDragged = false;

		OnEndDragEffect();
	}


	public void OnDrag (BaseEventData eventdata)
	{
		if(!isDragged)
			return;

		Vector3 mgrPos = IngameUIManager.Instance.GetMousePosition();
		Vector3 currentPosition = rect.position;
		currentPosition.x = mgrPos.x;
		currentPosition.y = mgrPos.y + offsetY;
		rect.position = currentPosition;
	}


	public virtual void Return ()
	{
		this.gameObject.transform.position = preDragPosition;
	}


	public virtual void SetFocused(bool focus)
	{
		isFocused = focus;
	}


	public virtual void OnClickEffect()
	{
		OnFocusEffect();
	}


	public virtual void OnFocusEffect()
	{
		if(IsFocused)
		{
			this.transform.localScale = focusedScale;
		}
		else
		{
			this.transform.localScale = Vector3.one;
		}
	}

	public virtual void OnBeginDragEffect()
	{
		OnFocusEffect();
	}

	public virtual void OnEndDragEffect()
	{
		OnFocusEffect();
		Return();
	}

}
