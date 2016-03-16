using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardDeck 
{

	[SerializeField] private List<int> allCards;
	[SerializeField] private List<int> availableCards;



	public void SetupDeck (int level, int team)
	{
		int[] deckCards;
		if(team == 1)
			deckCards = CardDeckData.GetPlayerCards(level);
		else
			deckCards = CardDeckData.GetOpponentCards(level);

		allCards = new List<int>(deckCards);
		availableCards = new List<int>(deckCards);
	}


	public bool HasCards ()
	{
		return (availableCards.Count > 0);
	}

	public int GetCard ()
	{
		if(availableCards.Count <= 0)
			return -1;
		
		int cardIdx = UnityEngine.Random.Range(0, availableCards.Count-1);
		int card = availableCards[cardIdx];
		availableCards.RemoveAt(cardIdx);

		return card;
	}



}
