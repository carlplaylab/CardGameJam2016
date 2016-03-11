using UnityEngine;
using System.Collections;

public class SortLayerModifier : MonoBehaviour 
{
	[SerializeField] private int sortingLayer = 0;
	[SerializeField] private string sortingLayerName;

	private Renderer r;

	public int SortingLayer
	{
		get { return this.sortingLayer; }
	}

	public string SortingLayerName
	{
		get { return this.sortingLayerName; }
	}

	void Awake()
	{
		this.r = this.GetComponent<Renderer>();
		this.SortLayer();
	}

	public void SetSortingLayer(int newLayer)
	{
		this.sortingLayer = newLayer;
		this.SortLayer();
	}

	[ContextMenu("Sort Layer")]
	public void SortLayer()
	{
		if(this.r != null)
		{
			this.r.sortingLayerName = this.sortingLayerName;
			this.r.sortingOrder = this.sortingLayer;
		}
	}
}