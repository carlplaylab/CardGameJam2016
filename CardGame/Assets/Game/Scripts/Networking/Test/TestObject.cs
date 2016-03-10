using UnityEngine;
using UnityEngine.Networking;
using System.Collections;



public class TestObject : NetworkBehaviour 
{

	[SerializeField] private Vector3 spaceMovement;

	[SyncVar] Vector3 pos;



	void Awake ()
	{
		pos = this.transform.localPosition;
	}


	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			pos.y += spaceMovement.y;
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			pos.y -= spaceMovement.y;
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			pos.x += spaceMovement.x;
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			pos.x -= spaceMovement.x;
		}

		SyncState();
	}

	public void SyncState ()
	{
		this.transform.localPosition = pos;
	}
}
