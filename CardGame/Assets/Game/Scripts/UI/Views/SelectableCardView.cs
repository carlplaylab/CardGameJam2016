using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SelectableCardView : DraggableUIView
{
	[SerializeField] private Image charImage;
	[SerializeField] private Image elementIcon;
	[SerializeField] private Text costText;

	public int CharacterID = 2;
	public int id = 0;



	public override void OnClickEffect()
	{
		OnFocusEffect();

		if(IsFocused)
		{
			gameObject.SendMessageUpwards("OnCardSelected", this, SendMessageOptions.DontRequireReceiver);
		}
	}


	public override void OnBeginDragEffect()
	{
		if(IsDragged)
		{
			gameObject.SendMessageUpwards("OnCardDrag", this, SendMessageOptions.DontRequireReceiver);
		}
	}


	public void SetDetails (int id, CharacterData charData)
	{
		CharacterID = charData.id;
		this.id = id;

		charImage.sprite = IngameSpriteCenter.Instance.GetSprite(charData.cardSprite);
		elementIcon.sprite = IngameSpriteCenter.Instance.GetButtonSprite(charData.elementType);
		costText.text = charData.spawnCost.ToString();
	}

}
