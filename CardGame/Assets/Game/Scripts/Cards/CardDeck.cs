using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CardDeck 
{
	public const int SPAWN_LIMIT = 5;

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
		{
			//Debug.Log("Team " + team + ", Spawn COunt " + spawnCards.Count);
			return -1;
		}

		Dictionary<int, int> spawnCardCounters = new Dictionary<int, int>();
		for(int i=0; i < spawnCards.Count; i++)
		{
			int spawnID = spawnCards[i];
			if( !spawnCardCounters.ContainsKey(spawnID) )
			{
				spawnCardCounters.Add(spawnID, 1);
			}
			else
			{
				int count = spawnCardCounters[spawnID];
				count++;
				spawnCardCounters[spawnID] = count;
			}
		}

		int maxID = -1;
		int maxCount = 0;
		foreach(int key in spawnCardCounters.Keys)
		{
			if(maxCount < spawnCardCounters[key])
			{
				maxCount = spawnCardCounters[key];
				maxID = key;
			}
		}

		int card = -1;
		if(maxID >= 0 && maxCount >= 3)
		{
			List<int> diffAvailable = new List<int>();
			List<int> diffAvailableIndexes = new List<int>();
			for(int i=0; i < availableCards.Count; i++)
			{
				if(availableCards[i] != maxID)
				{
					diffAvailable.Add( availableCards[i] );
					diffAvailableIndexes.Add( i );
				}
			}

			if(diffAvailable.Count > 0)
			{
				int cardIdx = UnityEngine.Random.Range(0, diffAvailable.Count-1);

				card = diffAvailable[cardIdx];
				int availableIdx = diffAvailableIndexes[cardIdx];
				availableCards.RemoveAt(availableIdx);
				spawnCards.Add(card);
			}
		}

		if(card < 0)
		{
			int cardIdx = UnityEngine.Random.Range(0, availableCards.Count-1);
			card = availableCards[cardIdx];
			availableCards.RemoveAt(cardIdx);
			spawnCards.Add(card);
		}

		//Debug.Log("Team " + team + ", CardsLeft " + availableCards.Count);
		return card;
	}

	public int GetCardsNeeded ()
	{
		return (SPAWN_LIMIT - spawnCards.Count);
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

	public void LogSpawnCards ()
	{
		string spawn = "Spawn Cards : " ;
		for(int i=0; i<spawnCards.Count; i++)
		{
			CardData cdata = CardDatabase.Instance.GetData( spawnCards[i] );
			spawn += cdata.id + " " + cdata.name + ", ";
		}
		Debug.Log(spawn);
	}

}
