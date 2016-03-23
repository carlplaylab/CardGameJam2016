using UnityEngine;
using System.Collections;

public class TimedEffects : Effects 
{

	[SerializeField] protected float life;
	[SerializeField] protected bool destroyOnEnd;

	public override void Play ()
	{
		timer = life;
		base.Play();
	}

	public override void PlayAt (Vector3 pos)
	{
		timer = life;
		base.PlayAt (pos);
	}


	public override void UpdateEffects ()
	{
		timer -= Time.deltaTime;
		if(timer <= 0f)
		{
			DestroyEffect();
		}
	}
}
