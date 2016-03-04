using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SelectableCardView : DraggableUIView
{
	[SerializeField] private Image charImage;
	[SerializeField] private Image elementIcon;
	[SerializeField] private Text costText;
	[SerializeField] private GameObject[] bgObjects;

	public int CharacterID = 2;
	public int id = 0;

	private bool onUI = true;


	#region Overriden Functions

	public override void OnClickEffect()
	{
		OnFocusEffect();

		if(IsFocused)
		{
			onUI = true;
			gameObject.SendMessageUpwards("OnCardSelected", this, SendMessageOptions.DontRequireReceiver);
		}
	}


	public override void OnBeginDragEffect()
	{
		if(IsDragged)
		{
			onUI = true;
			gameObject.SendMessageUpwards("OnCardDrag", this, SendMessageOptions.DontRequireReceiver);
		}
	}


	public override void OnEndDragEffect()
	{
		base.OnEndDragEffect();
		onUI = true;
		gameObject.SendMessageUpwards("OnCardDragEnd", this, SendMessageOptions.DontRequireReceiver);
	}


	public override void Return ()
	{
		this.gameObject.transform.position = preDragPosition;
		SetUIStatus(true);
	}

	#endregion

	public void SetDetails (int id, CharacterData charData)
	{
		CharacterID = charData.id;
		this.id = id;

		charImage.sprite = IngameSpriteCenter.Instance.GetSprite(charData.cardSprite);
		elementIcon.sprite = IngameSpriteCenter.Instance.GetButtonSprite(charData.elementType);
		costText.text = charData.spawnCost.ToString();
	}
		

	public bool SetUIStatus(bool cardOnUI)
	{
		if(onUI == cardOnUI)
			return false;

		onUI = cardOnUI;

		for(int i=0; i < bgObjects.Length; i++)
		{
			bgObjects[i].SetActive(onUI);
		}
		elementIcon.gameObject.SetActive(onUI);
		costText.gameObject.SetActive(onUI);
		return true;
	}

}
