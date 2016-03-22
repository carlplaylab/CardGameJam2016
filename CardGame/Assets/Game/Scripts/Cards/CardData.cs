using UnityEngine;
using System.Collections;


public enum CardType
{
	CHARACTER = 0,
	SPELL = 1
}


[SerializeField]
public class CardData
{
	public int id;
	public string name;
	public string description;
	public CardType cardType;
	public ElementType elementType;
	public int cost;
	public string cardSprite;

	public int characterId;


	public CardData ()
	{
		id = 0;
		name = "new card";
		description = "its a new card!";
		cardType = CardType.CHARACTER;
		elementType = ElementType.LAND;
		cost = 1;
		cardSprite = "";
		characterId = 1;
	}
}
