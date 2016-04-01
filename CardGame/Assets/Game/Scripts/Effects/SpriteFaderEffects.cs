using UnityEngine;
using System.Collections;

public class SpriteFaderEffects : MonoBehaviour 
{

	[SerializeField] private SpriteRenderer[] sprites;
	[SerializeField] public float minAlpha;
	[SerializeField] public float maxAlpha;

	public float duration = 1f;

	private float timer;
	private float alpha;
	private bool fadeIn = false;
	private bool playing = false;



	public void Play  ()
	{
		playing = true;
	}

	void Update ()
	{
		if(!playing)
			return;
		
		timer += Time.deltaTime;
		float t = timer / (duration/2f);
		if(t > 1f)
			t = 1f;
		
		if(fadeIn)
		{
			alpha = Mathf.Lerp(maxAlpha, minAlpha, t);
		}
		else
		{
			alpha = Mathf.Lerp(minAlpha, maxAlpha, t);
		}
		SetAlpha(alpha);

		if(t >= 1f)
		{
			timer = 0f;
			fadeIn = !fadeIn;
		}
	}

	void SetAlpha(float newAlpha)
	{
		alpha = newAlpha;
		for(int i=0; i < sprites.Length; i++)
		{
			Color c = sprites[i].color;
			c.a = alpha;
			sprites[i].color = c;
		}
	}

}
