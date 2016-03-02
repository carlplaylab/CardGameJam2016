using UnityEngine;
using System.Collections;

public class ElementCounterDisplay : CounterDisplay 
{
	[SerializeField] private ElementType elementType;


	public ElementType Element
	{
		get { return elementType; }
	}

}
