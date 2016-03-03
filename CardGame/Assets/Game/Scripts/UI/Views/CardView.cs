using UnityEngine;
using System.Collections;

public class CardView : UIView 
{
	[SerializeField] ElementView elementView;
	[SerializeField] CharacterView characterView;


	void Awake ()
	{
		EventBroadcaster.Instance.AddObserver(EventNames.UI_SHOW_CHARACTER_CARD, ShowCard);
	}


	void OnDestroy ()
	{
		EventBroadcaster.Instance.RemoveObserver(EventNames.UI_SHOW_CHARACTER_CARD);
	}



	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Hide();
		}
	}


	public void ShowCard(Parameters charParams)
	{
		int charId = charParams.GetIntExtra("character", 1);
		CharacterData data = CharacterDatabase.Instance.GetData(charId);
		if(data != null)
		{
			ShowCharacter(data);
		}
	}


	public void ShowCharacter (CharacterData data)
	{
		elementView.Element = data.elementType;
		elementView.Amount = data.spawnCost;

		Sprite charSprite = IngameSpriteCenter.Instance.GetSprite( data.cardSprite );
		characterView.SetCharacter(data, charSprite);

		Show();
	}

}
