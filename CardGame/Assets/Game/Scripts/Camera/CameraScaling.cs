using UnityEngine;
using System.Collections;



/// <summary>
/// Camera scaling.
/// This adjusts camera size to maintain a fixed base height width among different screen sizes
/// 
///  by: Carl Joven
/// </summary>
public class CameraScaling : MonoBehaviour {

	private const float BASE_WIDTH	= 768.0f;
	private const float BASE_HEIGHT = 1365.0f;


	void Awake () 
	{
		Camera cam = this.GetComponent<Camera>();
		cam.orthographicSize = cam.orthographicSize * BASE_WIDTH/BASE_HEIGHT * (float)Screen.height/(float)Screen.width;
	}


	void Start () {
	
	}


	void Update () {
	
	}
}
