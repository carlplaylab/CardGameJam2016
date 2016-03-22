using UnityEngine;
using System.Collections;
using System;


public class AttackEffects : Effects 
{

	[SerializeField] private float attackSpeed;
	[SerializeField] private Vector3 bounce;

	private Vector3 previousPos;
	private Vector3 startPos;
	private Vector3 endPos;
	private int pathCount = 0;


	void Awake ()
	{
		previousPos = this.transform.localPosition;
	}

	public void Setup(Vector3 end)
	{
		Reset();
		previousPos = this.transform.localPosition;
		startPos = this.transform.position;
		endPos = end;
		endPos.z = startPos.z;
		duration = Vector3.Distance(startPos, endPos) / attackSpeed;
	}

	public override void UpdateEffects ()
	{
		this.transform.position = Lerp(startPos, endPos);
		if(timer >= duration)
		{
			if(pathCount==0)
			{
				SetupBounce ();
				pathCount++;
			}
			else
			{
				End();
			}
		}
	}

	public override void Play ()
	{
		playing = true;
	}

	public override void Stop ()
	{
	}

	public override void End ()
	{
		base.End();
	}

	public override void Reset ()
	{
		base.Reset();
		pathCount = 0;
	}

	private void SetupBounce ()
	{
		base.Reset();

		EffectsHandler.Instance.PlayEffectsAt(EffectsType.FX_SLASH, this.transform.position);

		float direction = startPos.x < endPos.x ? -1f : 1f;
		startPos = this.transform.position;
		endPos.y += bounce.y;
		endPos.x += bounce.x * direction;
		duration = Vector3.Distance(startPos, endPos) / (attackSpeed/3f);
	}

	public void ResetPosition ()
	{
		this.transform.localPosition = previousPos;
	}

}
