using UnityEngine;
using System.Collections;


[SerializeField]
public class BoardPlayer : MonoBehaviour 
{
	[SerializeField] private int team;
	[SerializeField] private CharacterTeam characterTeam;
	[SerializeField] private CardDeck cardDeck;

	private bool turnActive = false;



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

	public PlayerResources Resources
	{
		get { return IngameDataCenter.Instance.GetPlayerData(TeamId).ResourceData; }
	}

	public void Setup(int teamNumber, int level)
	{
		team = teamNumber;
		characterTeam = new CharacterTeam(teamNumber);
		cardDeck = new CardDeck();
		cardDeck.SetupDeck(level, teamNumber);
	}

	public void SetTurn(int teamNumber)
	{
		turnActive = teamNumber == TeamId;

		if(turnActive)
		{
			int newCardId = Deck.GetCard();
			if(newCardId >= 0 && TeamId == 1)
			{
				Debug.Log("newCardId " + newCardId);
				// Note a new card was added into the player's deck
				Parameters cardparams = new Parameters();
				cardparams.PutExtra("card", newCardId);
				EventBroadcaster.Instance.PostEvent(EventNames.UI_ADD_CARD_TO_DECK, cardparams);
			}
		}
	}

}
