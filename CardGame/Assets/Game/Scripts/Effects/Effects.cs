using UnityEngine;
using System.Collections;
using System;


public class Effects : MonoBehaviour 
{
	protected bool playing = false;
	protected float timer = 0f;
	protected float duration = 0f;

	public System.Action onEndAction;

	public bool IsPlaying
	{
		get { return playing; } 
	}

	void Update ()
	{
		if(!playing)
			return;

		UpdateEffects();
	}

	public virtual void UpdateEffects ()
	{
	}

	public virtual void Play ()
	{
	}

	public virtual void Stop ()
	{
		playing = false;
	}

	public virtual void End ()
	{
		playing = false;
		if(onEndAction != null)
			onEndAction();
	}

	public virtual void Reset ()
	{
		timer = 0f;
		duration = 0f;
	}

	public virtual Vector3 Lerp (Vector3 start, Vector3 end)
	{
		timer += Time.deltaTime;
		float t = Mathf.Clamp(timer/duration, 0f, 1f);
		if(timer > duration)
			timer = duration;
		
		return Vector3.Lerp(start, end, t);
	}
}
