using UnityEngine;
using System.Collections;



public class GameTouchElement : MonoBehaviour 
{
	protected bool ALLOW_LOG = true;	// for testing
	protected bool hovered = false;
	
	public virtual void OnClick () 
	{
		if( ALLOW_LOG ) Debug.Log(name + " OnClick()");
		hovered = false;
	}
	
	public virtual void OnPress (bool pressed)
	{
		if( ALLOW_LOG ) Debug.Log(name + " OnPress() " + pressed);

		hovered = pressed;
	}
	
	public virtual void OnDragOver ()
	{
		if( ALLOW_LOG ) Debug.Log(name + " OnDragOver()");
		hovered = true;
	}

	public virtual void OnDragOut ()
	{
		if( ALLOW_LOG ) Debug.Log(name + " OnDragOut()");
		hovered = false;
	}
	
	public virtual void OnHover ()
	{
		if( ALLOW_LOG ) Debug.Log(name + " OnHover()");
	}

	public virtual bool CheckHovered ()
	{
		return hovered;
	}

}
