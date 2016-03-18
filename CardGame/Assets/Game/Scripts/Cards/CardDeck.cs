using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardDeck 
{
	public const int SPAWN_LIMIT = 4;

	[SerializeField] private List<int> allCards;
	[SerializeField] private List<int> spawnCards;
	[SerializeField] private List<int> availableCards;

	private int team;

	public void SetupDeck (int level, int team)
	{
		this.team = team;
		int[] deckCards;

		if(team == 1)
			deckCards = CardDeckData.GetPlayerCards(level);
		else
			deckCards = CardDeckData.GetOpponentCards(level);

		allCards = new List<int>(deckCards);
		availableCards = new List<int>(deckCards);
		spawnCards = new List<int>();
	}


	public bool HasCards ()
	{
		return (availableCards.Count > 0);
	}


	public int GetCard ()
	{
		if(availableCards.Count <= 0)
			return -1;

		if(spawnCards.Count >= SPAWN_LIMIT)
			return -1;
		
		int cardIdx = UnityEngine.Random.Range(0, availableCards.Count-1);
		int card = availableCards[cardIdx];
		availableCards.RemoveAt(cardIdx);
		spawnCards.Add(card);

		//Debug.Log("Team " + team + ", GetCard " +card);
		return card;
	}


	public int GetRandomSpawnCard()
	{
		int randCard = -1;
		if(spawnCards.Count > 0)
		{
			int randIdx = UnityEngine.Random.Range(0, spawnCards.Count-1);
			randCard = spawnCards[randIdx];
		}
		return randCard;
	}

	public void CardUsed(int cardId)
	{
		int idx = -1;
		for(int i=0; i< spawnCards.Count; i++)
		{
			if(spawnCards[i] == cardId)
				idx = i;
		}
		spawnCards.RemoveAt(idx);
	}

}
