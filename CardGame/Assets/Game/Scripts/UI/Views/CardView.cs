using UnityEngine;
using System.Collections;

public class CardView : UIView 
{
	[SerializeField] ElementView elementView;
	[SerializeField] CharacterView characterView;


	void Awake ()
	{
		EventBroadcaster.Instance.AddObserver(EventNames.UI_SHOW_CHARACTER_CARD, ShowCard);
		EventBroadcaster.Instance.AddObserver(EventNames.UI_HIDE_CHARACTER_CARD, Hide);
	}


	void OnDestroy ()
	{
		EventBroadcaster.Instance.RemoveObserver(EventNames.UI_SHOW_CHARACTER_CARD);
		EventBroadcaster.Instance.RemoveObserver(EventNames.UI_HIDE_CHARACTER_CARD);
	}


	public override void PostLoadProcess ()
	{
		this.gameObject.SetActive(false);
		SetState(UIViewState.HIDDEN);
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
