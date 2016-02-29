using UnityEngine;
using System.Collections;



public class GameTouchElement : MonoBehaviour 
{
	protected bool ALLOW_LOG = false;	// for testing
	
	
	public virtual void OnClick () 
	{
		if( ALLOW_LOG ) Debug.Log(name + " OnClick()");
	}
	
	public virtual void OnPress (bool pressed)
	{
		if( ALLOW_LOG ) Debug.Log(name + " OnPress() " + pressed);
	}
	
	public virtual void OnDragOver ()
	{
		if( ALLOW_LOG ) Debug.Log(name + " OnDragOver()");
	}
	
	public virtual void OnDragOut ()
	{
		if( ALLOW_LOG ) Debug.Log(name + " OnDragOut()");
	}
	
	public virtual void OnHover ()
	{
		if( ALLOW_LOG ) Debug.Log(name + " OnHover()");
	}

}
