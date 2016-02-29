using UnityEngine;
using System.Collections;

public class CellHighlight : MonoBehaviour 
{
	[SerializeField] private SpriteRenderer spriteObject;
	[SerializeField] private Sprite greenSprite;
	[SerializeField] private Sprite redSprite;


	void Awake ()
	{
		Hide() ;
	}


	public void Hide ()
	{
		spriteObject.gameObject.SetActive(false);
	}


	public void Show (bool useGreen)
	{
		if(useGreen)
			spriteObject.sprite = greenSprite;
		else 
			spriteObject.sprite = redSprite;

		spriteObject.gameObject.SetActive(true);
	}


}
