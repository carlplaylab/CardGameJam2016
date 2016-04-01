using UnityEngine;
using System.Collections;



public class CardDeckData
{
	public static int [] GetOpponentCards(int level)
	{
		int[] cards =  new int[100];

		if(level == 1)
		{
			for(int i=0; i < cards.Length; i++)
			{
				if(i < 50)
					cards[i] = 6;
				else
					cards[i] = 7;
			}
		}
		else if(level == 2)
		{
			for(int i=0; i < cards.Length; i++)
			{
				if(i < 50)
					cards[i] = 6;
				else if(i < 75)
					cards[i] = 7;
				else 
					cards[i] = 8;
			}
		}
		else
		{
			for(int i=0; i < cards.Length; i++)
			{
				if(i < 40)
					cards[i] = 6;
				else if(i < 60)
					cards[i] = 7;
				else if(i < 80)
					cards[i] = 8;
				else 
					cards[i] = 9;
			}
		}
		return cards;
	}

	public static int [] GetPlayerCards(int level) 
	{
		int[] cards = new int[40];

		if(level == 1)
		{
			for(int i=0; i < cards.Length; i++)
			{
				if(i < 15)
					cards[i] = 2;
				else if(i < 20)
					cards[i] = 3;
				else
					cards[i] = 10;
			}
		}
		else if(level == 2)
		{
			for(int i=0; i < cards.Length; i++)
			{
				if(i < 10)
					cards[i] = 2;
				else if(i < 14)
					cards[i] = 3;
				else if(i < 20)
					cards[i] = 10;
				else if(i < 24)
					cards[i] = 4;
				else
					cards[i] = 11;
			}
		}
		else
		{
			for(int i=0; i < cards.Length; i++)
			{
				if(i < 10)
					cards[i] = 2;
				else if(i < 14)
					cards[i] = 3;
				else if(i < 20)
					cards[i] = 10;
				else if(i < 24)
					cards[i] = 4;
				else
					cards[i] = 11;
			}
		}
		return cards;
	}

}
