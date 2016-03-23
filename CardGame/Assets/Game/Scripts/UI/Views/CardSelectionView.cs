using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class CardSelectionView : UIView 
{
	private const int CARD_LIMIT = 5;

	[SerializeField] private GameObject referenceCardObject;
	[SerializeField] private Vector3 startPos;

	private List<SelectableCardView> cardViewList;
	private SelectableCardView selectedCard;
	private bool mouseDown = false;


	void Awake ()
	{
		EventBroadcaster.Instance.AddObserver(EventNames.UI_ADD_CHARACTER_CARD, AddCard);
		EventBroadcaster.Instance.AddObserver(EventNames.UI_ADD_CARD_TO_DECK, AddCardToDeck);
		EventBroadcaster.Instance.AddObserver(EventNames.PLAYER_TURN_TOGGLED, PlayerTurnToggled);
	}


	void OnDestroy ()
	{
		EventBroadcaster.Instance.RemoveObserver(EventNames.UI_ADD_CHARACTER_CARD);
		EventBroadcaster.Instance.RemoveObserver(EventNames.UI_ADD_CARD_TO_DECK);
		EventBroadcaster.Instance.RemoveObserver(EventNames.PLAYER_TURN_TOGGLED);
	}


	void Update ()
	{
		if(Input.GetMouseButtonDown(0))
		{
			ReleaseSelectedCard();
		}
		else if(Input.GetMouseButtonUp(0))
		{
			mouseDown = false;
		}

		if(mouseDown && selectedCard != null)
		{
			bool inUI = CheckTouchInRectangle();
			bool statusChanged = selectedCard.SetUIStatus( inUI );
			if(statusChanged)
			{
				if(inUI)
				{
					bool converted = GameBoardManager.Instance.EndDragCardOnBoard(selectedCard.CardId);
					if(converted)
						RemoveCard(selectedCard);
				}
				else
				{
					GameBoardManager.Instance.StartDragCardOnBoard(selectedCard.CardId);
				}
			}
		}
	}


	public override void PostLoadProcess ()
	{
		base.PostLoadProcess ();
		cardViewList = new List<SelectableCardView>();
	}

	
	public void AddCard (Parameters cardParams)
	{
		int charID = cardParams.GetIntExtra("character", 1);
		CharacterData charData = CharacterDatabase.Instance.GetData(charID);
		AddCard(charData, 1);
	}



	public void AddCard(CharacterData charData, int cardId)
	{
		if(charData == null)
			return;

		int newId = cardViewList.Count;
		GameObject newObj = GameObject.Instantiate(referenceCardObject) as GameObject;
		newObj.transform.SetParent(this.transform);
		newObj.transform.localScale = Vector3.one;

		SelectableCardView cardView = newObj.GetComponent<SelectableCardView>();
		cardView.SetDetails(newId, charData, cardId);

		RectTransform cardXform = newObj.GetComponent<RectTransform>();
		float cardWidth = cardXform.rect.size.x;
		float cardHeight = cardXform.rect.size.y;
		Vector3 pos = startPos;
		pos.x += (newId - CARD_LIMIT/2) * cardWidth * 0.75f;

		cardXform.anchoredPosition3D = pos;

		cardViewList.Add(cardView);
	}

	public void AddCardToDeck(Parameters cardParams)
	{
		int cardId = cardParams.GetIntExtra("card",-1);
		int cardAmount = cardParams.GetIntExtra("cardamount", 0);

		if(cardId >= 0)
		{
			AddCard(cardId);
		}
		if(cardAmount > 0)
		{
			for(int i=0; i < cardAmount; i++)
			{
				int multiCardId = cardParams.GetIntExtra("card_" + i, -1);
				if(multiCardId >= 0)
				{
					AddCard(multiCardId);
				}
			}
		}
	}

	private void AddCard(int cardId)
	{
		CardData cdata = CardDatabase.Instance.GetData(cardId);
		if(cdata == null)
			return;

		if(cdata.cardType == CardType.CHARACTER)
		{
			CharacterData chardata = CharacterDatabase.Instance.GetData(cdata.dataId);
			AddCard(chardata, cardId);
		}
	}


	public void OnCardSelected(SelectableCardView card)
	{
		ShowCard(card.CharacterID);
		selectedCard = card;
	}


	public void OnCardDrag(SelectableCardView card)
	{
		if(selectedCard != null)
		{
			EventBroadcaster.Instance.PostEvent(EventNames.UI_HIDE_CHARACTER_CARD);
			if(selectedCard != card)
			{
				ReleaseSelectedCard();
			}
		}
		selectedCard = card;
		mouseDown = (selectedCard != null);
	}


	public void OnCardDragEnd(SelectableCardView card)
	{
		if(selectedCard != null)
		{
			bool converted = GameBoardManager.Instance.EndDragCardOnBoard(selectedCard.CardId);
			if(converted)
				RemoveCard(selectedCard);
		}
	}

	public void ShowCard(int characterID)
	{
		Parameters charParams = new Parameters();
		charParams.PutExtra("character", characterID);
		EventBroadcaster.Instance.PostEvent(EventNames.UI_SHOW_CHARACTER_CARD, charParams);
	}


	public SelectableCardView GetCard(int cardId)
	{
		for(int i=0; i < cardViewList.Count; i++)
		{
			if(cardViewList[i].id == cardId)
				return cardViewList[i];
		}
		return null;
	}


	private void ReleaseSelectedCard()
	{
		if(selectedCard == null)
			return;
		
		bool inCard = selectedCard.CheckTouchInRectangle();
		if(inCard)
			return;

		selectedCard.SetFocused(false);
		selectedCard.OnFocusEffect();
		selectedCard = null;
	}


	private void RemoveCard(SelectableCardView card)
	{
		cardViewList.Remove(card);
		if(selectedCard == card)
		{
			ReleaseSelectedCard();
		}

		card.gameObject.SetActive(false);
		UnityEngine.GameObject.Destroy(card.gameObject);
		UpdateCardPositions();
	}


	private void UpdateCardPositions ()
	{
		for(int i=0; i < cardViewList.Count; i++)
		{
			SelectableCardView cardView = cardViewList[i];
			RectTransform cardXform = cardView.gameObject.GetComponent<RectTransform>();
			float cardWidth = cardXform.rect.size.x;
			float cardHeight = cardXform.rect.size.y;
			Vector3 pos = startPos;
			pos.x += (i - CARD_LIMIT/2) * cardWidth * 0.75f;

			cardXform.anchoredPosition3D = pos;
		}
	}


	public void PlayerTurnToggled(Parameters toggledParams)
	{
		int currentTeam = toggledParams.GetIntExtra("currentteam", 1);
		SetActivePosition(currentTeam == 1);
	}

}
