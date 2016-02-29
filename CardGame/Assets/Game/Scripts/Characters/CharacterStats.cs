using UnityEngine;
using System.Collections;


[SerializeField]
public class CharacterStats
{

	[SerializeField] public int attack;
	[SerializeField] public int movement;
	[SerializeField] public int life;


	public CharacterStats (CharacterData data)
	{
		attack = data.attack;
		movement = data.movement;
		life = data.life;
	}
}