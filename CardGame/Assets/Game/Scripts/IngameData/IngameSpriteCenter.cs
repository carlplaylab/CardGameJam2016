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


	// SPECIAL FUNCTIONS
	public Sprite GetIconSprite(ElementType eType)
	{
		string sprName = "cell_void";
		switch(eType)
		{
		case ElementType.AIR: 
			sprName = "icon_air";
			break;
		case ElementType.LAND: 
			sprName = "icon_land";
			break;
		case ElementType.WATER: 
			sprName = "icon_water";
			break;
		default:
			break;
		}

		return GetSprite(sprName);
	}


	public Sprite GetButtonSprite(ElementType eType)
	{
		string sprName = "button_metal";
		switch(eType)
		{
		case ElementType.AIR: 
			sprName = "button_air";
			break;
		case ElementType.LAND: 
			sprName = "button_wood";
			break;
		case ElementType.WATER: 
			sprName = "button_water";
			break;
		default:
			break;
		}

		return GetSprite(sprName);
	}

	public Sprite GetBaseSprite(ElementType eType)
	{
		string sprName = "base_land";
		switch(eType)
		{
		case ElementType.AIR: 
			sprName = "base_air";
			break;
		case ElementType.LAND: 
			sprName = "base_land";
			break;
		case ElementType.WATER: 
			sprName = "base_water";
			break;
		default:
			break;
		}

		return GetSprite(sprName);
	}
}
