using UnityEngine;
using System.Collections;


[SerializeField]
public class CharacterData
{

	[SerializeField] public int id;
	[SerializeField] public string name;
	[SerializeField] public string description;

	[SerializeField] public int attack;
	[SerializeField] public int movement;
	[SerializeField] public int life;
	[SerializeField] public ElementType elementType;
	[SerializeField] public int spawnCost;

	[SerializeField] public string characterPrefab;
	[SerializeField] public string atlas;
	[SerializeField] public string cardSprite;

	public CharacterData ()
	{
		id = 0;
		name = "";
		description = "";
		characterPrefab = "";
		atlas = "atlas1";
		cardSprite = "char_";
	}
}

