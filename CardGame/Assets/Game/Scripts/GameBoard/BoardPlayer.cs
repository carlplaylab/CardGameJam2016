using UnityEngine;
using System.Collections;


[SerializeField]
public class BoardPlayer : MonoBehaviour 
{
	[SerializeField] private int team;
	[SerializeField] private CharacterTeam characterTeam;
	[SerializeField] private CardDeck cardDeck;

	private bool turnActive = false;
	private OpponentAI brain;


	public int TeamId
	{
		get { return team; }
	}

	public bool TurnActive
	{
		get { return turnActive; }
	}

	public CharacterTeam Team
	{
		get { return characterTeam; }
	}

	public CardDeck Deck
	{
		get { return cardDeck; }
	}

	public PlayerIngameData IngameData
	{
		get { return IngameDataCenter.Instance.GetPlayerData(TeamId); }
	}

	public void Setup(int teamNumber, int level)
	{
		team = teamNumber;
		characterTeam = new CharacterTeam(teamNumber);
		cardDeck = new CardDeck();
		cardDeck.SetupDeck(level, teamNumber);

		brain = GetComponent<OpponentAI>();
		if(brain != null)
			brain.Setup();
	}

	public void SetInitialCards(int cardAmount)
	{
		if(TeamId == 1)
		{
			Parameters cardparams = new Parameters();
			cardparams.PutExtra("card", -1);
			cardparams.PutExtra("cardamount", cardAmount);
			for(int i=0; i < cardAmount; i++)
			{
				int newCardId = Deck.GetCard();
				cardparams.PutExtra("card_" + i, newCardId);
			}
			EventBroadcaster.Instance.PostEvent(EventNames.UI_ADD_CARD_TO_DECK, cardparams);
		}
		else
		{
			for(int i=0; i < cardAmount; i++)
			{
				Deck.GetCard();
			}
		}
	}

	public void SetTurn(int teamNumber)
	{
		turnActive = teamNumber == TeamId;

		if(turnActive)
		{
			int newCardId = Deck.GetCard();
			if(newCardId >= 0 && TeamId == 1)
			{
				// Note a new card was added into the player's deck
				Parameters cardparams = new Parameters();
				cardparams.PutExtra("card", newCardId);
				EventBroadcaster.Instance.PostEvent(EventNames.UI_ADD_CARD_TO_DECK, cardparams);
			}
		}

		if(brain != null)
			brain.SetTurnActive(turnActive);
	}

}
