using UnityEngine;
using System.Collections;
using System;

public class ElementSource : MonoBehaviour 
{
	[SerializeField] private Vector3 minPos;
	[SerializeField] private Vector3 maxPos;
	[SerializeField] private ElementSourceCubes[] cubesList;
	[SerializeField] private float showTime = 3f;
	[SerializeField] private GameObject container;

	public Action onElementsTaken = null;

	private float timer = 0f;

	public int ResourceCount
	{
		get { return cubesList.Length; }
	}

	public void SetVisible(bool visible)
	{
		this.gameObject.SetActive(visible);
		this.container.SetActive(visible);
	}


	public void Reset ()
	{
		for(int i=0; i < cubesList.Length; i++)
		{
			cubesList[i].gameObject.SetActive(false);
		}
	}


	public void Show ()
	{
		for(int i=0; i < cubesList.Length; i++)
		{
			cubesList[i].gameObject.SetActive(false);
		}

		int rand = UnityEngine.Random.Range(cubesList.Length-1, cubesList.Length+1);
		for(int i=0; i < rand; i++)
		{
			// 60% chance of getting land elements than water elements
			//ElementType randType = (UnityEngine.Random.Range(0, 99) % 3 < 2) ? ElementType.LAND : ElementType.WATER;
			cubesList[i].Type = ElementType.LAND;

			float randX = UnityEngine.Random.Range(minPos.x, maxPos.x);
			float randY = UnityEngine.Random.Range(minPos.y, maxPos.y);
			cubesList[i].gameObject.transform.localPosition = new Vector3(randX, randY, 0f);

			cubesList[i].gameObject.SetActive(true);
		}

		SetVisible(true);
		timer = showTime;
	}


	public void Hide ()
	{
		SetVisible(false);
		Reset();

		if(onElementsTaken != null)
			onElementsTaken();
	}


	public ElementType[] GetElements ()
	{
		ElementType[] elemTypes = new ElementType[cubesList.Length];
		for(int i=0; i < cubesList.Length; i++)
		{
			elemTypes[i] = cubesList[i].Type;
		}
		return elemTypes;
	}


	public void Update ()
	{
		if(timer > 0f)
		{
			timer -= Time.deltaTime;
			if(timer <= 0f)
			{
				Hide();
			}
		}
	}
}
