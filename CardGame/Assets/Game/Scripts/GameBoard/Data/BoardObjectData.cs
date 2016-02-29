using UnityEngine;
using System.Collections;



public enum BoardObjectType
{
	CHARACTER = 0,
	BLOCKER = 1
}

public class BoardObjectData : MonoBehaviour
{
	[SerializeField] public ElementType elementType;
	[SerializeField] public BoardObjectType objectType;
	[SerializeField] public int movementSpace;
	[SerializeField] public bool moveable;
}
