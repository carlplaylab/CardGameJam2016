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
	}


	void OnDestroy ()
	{
		EventBroadcaster.Instance.RemoveObserver(EventNames.UI_ADD_CHARACTER_CARD);
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
					GameBoardManager.Instance.EndDragCardOnBoard(selectedCard.CharacterID);
				else
					GameBoardManager.Instance.StartDragCardOnBoard(selectedCard.CharacterID);
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
		AddCard(charData);
	}


	public void AddCard(CharacterData charData)
	{
		if(charData == null)
			return;

		int newId = cardViewList.Count;
		GameObject newObj = GameObject.Instantiate(referenceCardObject) as GameObject;
		newObj.transform.SetParent(this.transform);
		newObj.transform.localScale = Vector3.one;

		SelectableCardView cardView = newObj.GetComponent<SelectableCardView>();
		cardView.SetDetails(newId, charData);

		RectTransform cardXform = newObj.GetComponent<RectTransform>();
		float cardWidth = cardXform.rect.size.x;
		float cardHeight = cardXform.rect.size.y;
		Vector3 pos = startPos;
		pos.x += (newId - CARD_LIMIT/2) * cardWidth * 0.75f;

		cardXform.anchoredPosition3D = pos;

		cardViewList.Add(cardView);
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
			GameBoardManager.Instance.EndDragCardOnBoard(selectedCard.CharacterID);
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
}
