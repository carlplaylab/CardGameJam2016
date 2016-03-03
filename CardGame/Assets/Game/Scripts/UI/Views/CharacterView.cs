using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterView : UIView
{

	[SerializeField] private Image characterImage;
	[SerializeField] private Text descriptionText;
	[SerializeField] private Text attackText;
	[SerializeField] private Text lifeText;

	public void SetCharacter(CharacterData data, Sprite charSprite)
	{
		if(charSprite != null)
			characterImage.sprite = charSprite;
	
		descriptionText.text = data.description;
		attackText.text = data.attack.ToString();
		lifeText.text = data.life.ToString();
	}

}
