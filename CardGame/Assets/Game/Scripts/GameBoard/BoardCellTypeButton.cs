using UnityEngine;
using System.Collections;

public class BoardCellTypeButton : GameTouchElement
{

	[SerializeField] public ElementType type;


	public override void OnPress (bool pressed)
	{
		SendMessageUpwards("ClickedButton", this, SendMessageOptions.DontRequireReceiver);
	}

	public Sprite GetSprite ()
	{
		return GetComponentInChildren<SpriteRenderer>().sprite;
	}

}
