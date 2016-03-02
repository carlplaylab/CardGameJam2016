using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class CounterDisplay : MonoBehaviour 
{
	[SerializeField] private NumberIncrementText text;
	[SerializeField] private RectTransform icon;

	private int amount;


	public int Amount
	{
		get { return amount; }
		set {
			amount = value; 
			text.onStartAction = Focus;
			text.Amount = amount;
			text.onEndAction = UnFocus;
		}
	}

	public void Focus ()
	{
		icon.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
	}

	public void UnFocus ()
	{
		icon.transform.localScale = Vector3.one;
	}

}
