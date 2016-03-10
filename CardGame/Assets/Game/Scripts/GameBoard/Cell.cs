using UnityEngine;
using System.Collections;


public class Cell : GameTouchElement 
{
	[SerializeField] private SpriteRenderer foreground;

	public int id;
	public int row;
	public int col;
	public ElementType type;

	private BoardObject resider;
	private CellHighlight highlighter;

	public int rowDifference = 0;
	public int colDifference = 0;
	public int Distance = 0;
	public float PythDistance = 0;


	void Awake ()
	{
		highlighter = GetComponent<CellHighlight>();
	}


	public override void OnPress (bool pressed)
	{
		//Debug.Log(name + " OnPress() " + pressed);
		if(pressed)
			this.transform.parent.gameObject.SendMessageUpwards("CellClicked", this, SendMessageOptions.DontRequireReceiver);
	}


	public void AssignDetails(ElementType newType, Sprite sprite)
	{
		type = newType;
		foreground.sprite = sprite;
	}


	public void SetControls(bool controlActive)
	{
		Collider2D collider = GetComponent<Collider2D>();
		if(collider != null)
			collider.enabled = controlActive;
	}


	public bool IsAdjacent (Cell otherCell)
	{
		int rowDiff = Mathf.Abs(otherCell.row - row);
		int colDiff = Mathf.Abs(otherCell.col - col);
		return (rowDiff <= 1 && colDiff <= 1);
	}


	public int GetDistanceFromCell (Cell otherCell)
	{
		if(otherCell == null)
			return 0;

		int rowDiff = Mathf.Abs(otherCell.row - row);
		int colDiff = Mathf.Abs(otherCell.col - col);

		otherCell.rowDifference = rowDiff;
		otherCell.colDifference = colDiff;

		otherCell.Distance = Mathf.Max(rowDiff, colDiff);
		otherCell.PythDistance= Vector2.Distance(Vector2.zero, new Vector2(otherCell.rowDifference, otherCell.colDifference));
		return otherCell.Distance;
	}


	public BoardObject ResidingObject
	{
		get { return resider; }
		set { resider = value; }
	}


	public bool IsVacant ()
	{
		return (resider == null);
	}


	public bool IsWalkable ()
	{
		return (type != ElementType.VOID);
	}


	public void Show ()
	{
		if(IsWalkable())
			foreground.gameObject.SetActive(true);
	}


	public void Hide ()
	{
		foreground.gameObject.SetActive(false);
	}


	public void HighlightCell(bool show, bool useGreen = true)
	{
		if(show && IsWalkable())
			highlighter.Show(useGreen);
		else
			highlighter.Hide();
	}
}
