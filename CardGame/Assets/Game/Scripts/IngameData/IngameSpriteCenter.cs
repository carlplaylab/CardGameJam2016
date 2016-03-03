using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class IngameSpriteCenter  
{
	
	private static IngameSpriteCenter instance;
	public static IngameSpriteCenter Instance
	{
		get 
		{ 
			if(instance == null)
			{
				instance = new IngameSpriteCenter();
			}
			return instance; 
		}
	}

	private List<string> atlasList;
	private Dictionary<string, Sprite> spriteList;



	public IngameSpriteCenter ()
	{
		atlasList = new List<string>();
		spriteList = new Dictionary<string, Sprite>();
	}


	public void AddAtlas(string atlas)
	{
		if(atlasList.Contains(atlas))
			return;

		atlasList.Add(atlas);

		Sprite[] sprites = Resources.LoadAll<Sprite>("Atlas/" + atlas);
		for(int i=0; i < sprites.Length; i++)
		{
			spriteList.Add(sprites[i].name, sprites[i]);
		}
	}


	public Sprite GetSprite(string spriteName)
	{
		if(spriteList != null && spriteList.ContainsKey(spriteName))
			return spriteList[spriteName];
		else
			return null;
	}


	public void Destroy ()
	{
		if(atlasList != null)
			atlasList.Clear();
		
		if(spriteList != null)
			spriteList.Clear();

		atlasList = null;
		spriteList = null;
		instance = null;
	}
}
