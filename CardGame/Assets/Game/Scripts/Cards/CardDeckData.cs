using UnityEngine;
using System.Collections;



public class CardDeckData
{
	public static int [] GetOpponentCards(int level)
	{
		int[] cards = new int[30];

		if(level == 1)
		{
			
		}
		else if(level == 2)
		{
			
		}
		else
		{
			
		}
		return GetPlayerCards(level);
	}

	public static int [] GetPlayerCards(int level)
	{
		int[] cards = new int[30];

		if(level == 1)
		{
			for(int i=0; i < cards.Length; i++)
			{
				if(i < 20)
					cards[i] = 0;
				else
					cards[i] = 1;
			}
		}
		else if(level == 2)
		{
			for(int i=0; i < cards.Length; i++)
			{
				if(i < 15)
					cards[i] = 0;
				else if(i < 24)
					cards[i] = 1;
				else 
					cards[i] = 3;
			}
		}
		else
		{
			for(int i=0; i < cards.Length; i++)
			{
				if(i < 15)
					cards[i] = 0;
				else if(i < 24)
					cards[i] = 1;
				else 
					cards[i] = 3;
			}
		}
		return cards;
	}

}
