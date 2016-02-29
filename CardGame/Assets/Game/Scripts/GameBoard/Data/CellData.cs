using UnityEngine;
using System.Collections;


public enum ElementType
{
	VOID = 0,
	AIR = 1,
	LAND = 2,
	WATER = 3
}


public class CellData : MonoBehaviour 
{

	[SerializeField] public GameObject referenceCell;

	[SerializeField] public Sprite spriteAir;
	[SerializeField] public Sprite spriteLand;
	[SerializeField] public Sprite spriteWater;
	[SerializeField] public Sprite spriteVoid;

	[SerializeField] public float cellHeight;
	[SerializeField] public float cellWidth;


	public GameObject CreateCell (Transform parent, Vector3 pos)
	{
		GameObject newCellObj = GameObject.Instantiate(referenceCell);
		newCellObj.transform.SetParent(parent);
		newCellObj.transform.localPosition = pos;
		newCellObj.transform.localScale = Vector3.one;
		return newCellObj;
	}

}
