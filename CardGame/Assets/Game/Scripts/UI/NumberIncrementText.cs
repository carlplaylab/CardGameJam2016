using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;



public class NumberIncrementText : MonoBehaviour 
{

	[SerializeField] private Text text;
	[SerializeField] private float incrementTime = 1;
	[SerializeField] private float delayTime = 0;

	private int amount = 0;
	private int currentAmount = 0;
	
	private int previousAmount = 0;
	private float timer = 1f;
	private float delayTimer = 1f;

	public Action onStartAction = null;
	public Action onEndAction = null;
	public Action onUpdateAction = null;

	private bool isOnStartInvoked = false;

	public float DelayTime 
	{
		get { return this.delayTime; }
		set { this.delayTime = value; }
	}

	public int Amount
	{
		get
		{
			return amount;
		}
		set
		{
			if(currentAmount == amount)
			{
				delayTimer = delayTime;
			}

			amount = value;
			previousAmount = currentAmount;
			timer = 0f;

			if(currentAmount > amount)
			{
				delayTimer = 0f;
			}

		}
	}


	void Awake ()
	{
		InitializeValue(0);
	}


	public void InitializeValue (int  value)
	{
		amount = value;
		currentAmount = value;
		previousAmount = currentAmount;
		text.text = currentAmount.ToString();
	}


	public void AdjustIncrementTime(float newTime)
	{
		incrementTime = newTime;
	}


	public void SkipToFinish ()
	{
		currentAmount = amount;
		delayTimer = 0f;
		UpdateText();
	}


	void Update ()
	{
		if(currentAmount != amount)
		{
			if(delayTimer > 0f)
			{
				delayTimer -= Time.deltaTime;
				return;
			}

			if(!isOnStartInvoked) {
				isOnStartInvoked = true;
				if(onStartAction != null) {
					onStartAction();
				}
			}

			timer += Time.deltaTime;

			if(timer < incrementTime)
			{
				currentAmount = (int)Mathf.Lerp(previousAmount, amount, (timer/incrementTime));
			}
			else
			{
				currentAmount = amount;
				if(onEndAction != null) {
					onEndAction();
				}
			}

			if(currentAmount > amount && previousAmount < amount)
			{
				currentAmount = amount;
			}
			else if(currentAmount < amount && previousAmount > amount)
			{
				currentAmount = amount;
			}

			UpdateText();

			if(onUpdateAction != null)
			{
				onUpdateAction();
			}
		}
	}

	private void UpdateText ()
	{
		text.text = string.Format("{0:n0}", currentAmount);
	}
}
