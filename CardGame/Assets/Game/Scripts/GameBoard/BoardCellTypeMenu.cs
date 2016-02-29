using UnityEngine;
using System.Collections;

public class BoardCellTypeMenu : MonoBehaviour 
{
	[SerializeField] private Sprite[] elementSprites;

	private ElementType lastType = ElementType.VOID;
	private Cell cell;


	public void Show(Cell targetCell)
	{
		if(targetCell != null)
		{
			cell = targetCell;
			this.transform.localPosition = cell.transform.localPosition;
			this.gameObject.SetActive(true);
			Debug.Log("Selected cell: " + cell.id);
		}
	}

	public void ClickedButton(BoardCellTypeButton button)
	{
		if(cell != null)
		{
			cell.AssignDetails(button.type, button.GetSprite());
			lastType = button.type;
		}
		this.gameObject.SetActive(false);
		SendMessageUpwards("FocusCells", SendMessageOptions.DontRequireReceiver);
	}

	public void SelectedElement(ElementType type)
	{
		if(cell != null && (int)type < elementSprites.Length)
		{
			cell.AssignDetails(type, elementSprites[ (int)type ]);
			lastType = type;
		}
		this.gameObject.SetActive(false);
		SendMessageUpwards("FocusCells", SendMessageOptions.DontRequireReceiver);
	}

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.V))
		{
			SelectedElement(ElementType.VOID);
		}
		else if(Input.GetKeyDown(KeyCode.A))
		{
			SelectedElement(ElementType.AIR);
		}
		else if(Input.GetKeyDown(KeyCode.L))
		{
			SelectedElement(ElementType.LAND);
		}
		else if(Input.GetKeyDown(KeyCode.W))
		{
			SelectedElement(ElementType.WATER);
		}
		else if(Input.GetKeyDown(KeyCode.Space))
		{
			SelectedElement(lastType);
		}
	}
}
