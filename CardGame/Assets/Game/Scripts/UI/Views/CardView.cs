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
		int cardid = charParams.GetIntExtra("card", 1);
		CardData data = CardDatabase.Instance.GetData(cardid);
		if(data == null)
		{
			return;
		}

		ShowCardDetails(data);
	}


	public void ShowCardDetails (CardData data)
	{
		elementView.Element = data.elementType;
		elementView.Amount = data.cost;

		Sprite cardsprite = IngameSpriteCenter.Instance.GetSprite( data.cardSprite );

		if(data.cardType == CardType.CHARACTER)
		{
			CharacterData chdata = CharacterDatabase.Instance.GetData(data.dataId);
			characterView.SetCharacter(chdata, cardsprite);
		}
		else 
		{
			SkillData skdata = SkillsDatabase.Instance.GetData(data.dataId);
			characterView.SetSkill(skdata, cardsprite);
		}

		Show();
	}


}
