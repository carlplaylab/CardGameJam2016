using UnityEngine;
using System.Collections;


public class GameInput : MonoBehaviour 
{

	[SerializeField] private string layer = "MiniGame";
	[SerializeField] private string cameraName = "MainCamera";

	private const string ON_DRAGOVER = "OnDragOver";
	private const string ON_DRAGOUT = "OnDragOut";
	private const string ON_HOVER 	= "OnHover";
	private const string ON_PRESS	= "OnPress";
	private const string ON_CLICK	= "OnClick";

	private const float CLICK_SENSITIVITY = 300f;


	public class MouseOrTouch
	{
		public Vector2 pos;				// Current position of the mouse or touch event
		public Vector2 lastPos;			// Previous position of the mouse or touch event
		public Vector2 delta;			// Delta since last update
		public Vector2 totalDelta;		// Delta since the event started being tracked

		public GameObject last;			// Last object under the touch or mouse
		public GameObject current;		// Current game object under the touch or mouse
		public GameObject pressed;		// Last game object to receive OnPress
		public GameObject dragged;		// Game object that's being dragged

		public float clickTime = 0f;	// The last time a click event was sent out

		public bool touchBegan = true;
		public bool pressStarted = false;
		public bool dragStarted = false;	
	}

	private MouseOrTouch currentTouch;
	private Camera gameCamera = null;
	private Plane m2DPlane = new Plane(Vector3.back, 0f);
	private int gameLayerMask = 10;

	private bool active = true;
	public GameObject hoveredObject;


	public bool Active 
	{
		get 
		{ 
			return active; 
		}
		set 
		{ 
			bool prev = active;
			active = value;
			if(prev && !active)
			{
				ResetCurrentTouch();
			}
		}
	}

	void Awake () 
	{
		//instance = this;
		currentTouch = new MouseOrTouch();
		Reset();
	}


	void OnDestroy ()
	{
		//instance = null;
	}


	void Start () 
	{
		GameObject camObj = GameObject.Find( cameraName );
		if(camObj != null)
		{
			gameCamera = camObj.GetComponent<Camera>();
		}
		gameLayerMask = LayerMask.GetMask( layer );
	}


	public void Reset() 
	{ 
		hoveredObject = null;
		currentTouch.last = null;
		currentTouch.current = null;
		currentTouch.pressed = null;
		currentTouch.dragged = null;

		currentTouch.lastPos = Input.mousePosition;
		currentTouch.pos = Input.mousePosition;
		currentTouch.delta = Vector2.zero;
		currentTouch.totalDelta = Vector2.zero;
	}


	void Update ()
	{
		/*
		if(BlackOverlay.Visible || !Active)
		{
			return;
		}
		*/
		UpdateInput();
	}


	void UpdateInput ()
	{
		if(currentTouch != null)
		{
			currentTouch.pos = Input.mousePosition;
			currentTouch.delta = currentTouch.pos - currentTouch.lastPos;
			currentTouch.totalDelta += currentTouch.delta;
		}

		if(Input.GetMouseButtonDown(0))
		{
			OnMouseDown();
		}
		else if(Input.GetMouseButtonUp(0))
		{
			OnMouseUp();
		}
		else if(Input.GetMouseButton(0))
		{
			OnMouseHeld();
		}

		if(currentTouch != null)
		{
			currentTouch.lastPos = currentTouch.pos;
		}
	}


	public bool Raycast (Vector3 screenPos, int layerMask, bool checkAllHit = false)
	{
		// Check if camera is available
		if( gameCamera == null || !gameCamera.gameObject.activeSelf)
			return false;

		// Check if position is in screen
		Vector3 pos = gameCamera.ScreenToViewportPoint(screenPos);
		if (float.IsNaN(pos.x) || float.IsNaN(pos.y)) 
			return false;

		// Cast a ray into the screen
		Ray ray = gameCamera.ScreenPointToRay(screenPos);
		float dist = gameCamera.farClipPlane - gameCamera.nearClipPlane;

		if (m2DPlane.Raycast(ray, out dist))
		{
			Vector3 point = ray.GetPoint(dist);

			if(checkAllHit)
			{
				Collider2D[] c2dall = Physics2D.OverlapPointAll(point, layerMask);

				if(c2dall != null)
				{
					// prioritize colliders hit
					for(int i=0; i < c2dall.Length; i++)
					{
						GameTouchElement elem = c2dall[i].gameObject.GetComponent<GameTouchElement>();
						if(elem != null)
						{
							hoveredObject = elem.gameObject;
							return true;
						}
					}
				}
			}

			Collider2D c2d = Physics2D.OverlapPoint(point, layerMask);
			if (c2d)
			{
				hoveredObject = c2d.gameObject;
				return true;
			}
		}

		hoveredObject = null;
		return false;
	}


	static public void Notify (GameObject go, string funcName, object obj)
	{
		if(go != null)
		{
			go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
		}
	}


	void ResetCurrentTouch ()
	{
		if(currentTouch == null)
			return;

		currentTouch.last = null;
		currentTouch.pressed = null;
		currentTouch.dragged = null;
		currentTouch.current = null;

		currentTouch.touchBegan = false;
		currentTouch.pressStarted = false;
		currentTouch.dragStarted = false;
	}

	void OnMouseDown()
	{
		if(Raycast(Input.mousePosition, gameLayerMask, true))
		{
			if(currentTouch.pressed != hoveredObject) 
			{
				// previously touched object now released
				Notify(currentTouch.pressed, ON_PRESS, false);
			}

			if(currentTouch.dragged != hoveredObject) {
				// notify previously dragged object that it is released
				Notify(currentTouch.dragged, ON_DRAGOUT, null);
			}

			currentTouch.totalDelta = Vector2.zero;
			currentTouch.dragStarted = false;
			currentTouch.last = currentTouch.current;

			currentTouch.current = hoveredObject;
			currentTouch.pressed = hoveredObject;
			currentTouch.dragged = hoveredObject;

			if(hoveredObject != null)
			{
				currentTouch.touchBegan = true;
				currentTouch.pressStarted = true;
				currentTouch.dragStarted = true;
			}

			// notify new object that it is pressed
			Notify(currentTouch.pressed, ON_PRESS, true);
		}
	}


	void OnMouseUp()
	{
		if(Raycast(Input.mousePosition, gameLayerMask))
		{
			// notify previously dragged object that it is released
			Notify(currentTouch.dragged, ON_DRAGOUT, null);

			// previously pressed object is released
			Notify(currentTouch.pressed, ON_PRESS, false);

			if( Mathf.Abs(currentTouch.totalDelta.sqrMagnitude) < CLICK_SENSITIVITY && 
				currentTouch.pressed == hoveredObject )
			{
				Notify(currentTouch.pressed, ON_CLICK, null);
			}

			ResetCurrentTouch ();
		}
	}





	void OnMouseHeld()
	{
		if(!currentTouch.touchBegan)
			return;

		Raycast(Input.mousePosition, gameLayerMask, true);

		if(currentTouch.dragged != hoveredObject) 
		{
			// notify previously dragged object that it is released
			Notify(currentTouch.dragged, ON_DRAGOUT, null);

			currentTouch.dragged = hoveredObject;
			Notify(currentTouch.dragged, ON_DRAGOVER, null);
		}

		currentTouch.dragStarted = (hoveredObject != null);
		currentTouch.current = hoveredObject;
	}

}