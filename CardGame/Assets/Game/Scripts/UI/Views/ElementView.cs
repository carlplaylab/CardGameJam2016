using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElementView : UIView
{

	[SerializeField] Image buttonImage;
	[SerializeField] Image iconImage;
	[SerializeField] Text amountText;

	[SerializeField] Sprite[] buttonSprites;
	[SerializeField] Sprite[] iconSprites;

	private ElementType elementType;
	private int amount;


	public int Amount
	{
		get
		{
			return amount;
		}
		set
		{
			amount = value;
			amountText.text = amount.ToString();
		}
	}


	public ElementType Element
	{
		get
		{
			return elementType;
		}
		set
		{
			elementType = value;
			int elementNumber = (int)elementType;
			if(elementNumber < buttonSprites.Length)
			{
				buttonImage.sprite = buttonSprites[elementNumber];
			}
			if(elementNumber < iconSprites.Length)
			{
				iconImage.sprite = iconSprites[elementNumber];
			}
		}
	}


	public void SetupView(ElementType etype, int amount)
	{
	}
}
