using UnityEngine;
using System.Collections;

public class ElementSourceCubes : MonoBehaviour 
{

	[SerializeField] SpriteRenderer bg;
	[SerializeField] SpriteRenderer icon;

	private ElementType type;


	public ElementType Type
	{
		get 
		{ 
			return type; 
		}
		set
		{
			type = value;
			bg.sprite = IngameSpriteCenter.Instance.GetButtonSprite(type);
			icon.sprite = IngameSpriteCenter.Instance.GetIconSprite(type);
		}
	}


}

