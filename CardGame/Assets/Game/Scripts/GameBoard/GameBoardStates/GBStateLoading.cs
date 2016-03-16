using UnityEngine;
using System.Collections;

public class GBStateLoading : GBState
{

	private int loadingCount = 0;



	public override BoardState GetState()
	{
		return BoardState.LOADING;
	}
		
	public override void Start (GameBoard board)
	{
		loadingCount = 0;

		// Make sure no clicks are registered during loading
		board.InputEnable = false;
	}

	public override void Update (GameBoard board)
	{
		
		if(loadingCount == 0)
		{
			IngameSpriteCenter.Instance.AddAtlas("assets1");
			IngameSpriteCenter.Instance.AddAtlas("main_assets");
			
			// Insert loading of cells
			LoadCells(board);
		}
		else if(loadingCount == 1)
		{
			// Setting up player data
			IngameDataCenter.Initialize();

			// Insert loading of character data
			int dataChars = CharacterDatabase.Instance.Count();
			int cards = CardDatabase.Instance.Count();

			Debug.Log("all characters : " + dataChars + ", all cards: " + cards);
		}
		else if(loadingCount == 2)
		{
			// Insert loading of character reference objects
			CharacterHandler.Instance.Initialize();
		}
		else if(loadingCount == 4)
		{
			// Setup UI
			IngameUIManager.Instance.Initialize();
		}
		else if(loadingCount == 5)
		{
			// Insert creation of characters in team 1
			LoadCharacters(board);
		}
		else if(loadingCount == 6)
		{
			// Insert creation of characters in team 2
		}
		else if(loadingCount == 7)
		{
			// End loading
			GameBoardManager.Instance.SetState(BoardState.RESOURCE_ADDING);
		}

		loadingCount++;
	}

	public override void End (GameBoard board)
	{
		IngameDataCenter.Instance.Player1.ResourceData.AddResource(ElementType.LAND, 10);
		IngameDataCenter.Instance.Player2.ResourceData.AddResource(ElementType.LAND, 10);

		IngameDataCenter.Instance.Player1.ResourceData.UpdateUI();
	}


	#region Loading Functions


	private void LoadCells (GameBoard board)
	{
		string cellAreaId = GameBoardManager.Instance.Settings.CellAreaID.ToString();

		Object resObj = Resources.Load("Prefabs/GameBoards/cellArea_" + cellAreaId);
		if(resObj == null)
		{
			Debug.LogWarning("COULD NOT LOAD CELLS : " + cellAreaId);
			return;
		}

		GameObject cellAreaObj = GameObject.Instantiate(resObj) as GameObject;
		cellAreaObj.transform.SetParent(board.transform);
		cellAreaObj.transform.localPosition = Vector3.zero;
		cellAreaObj.name = "cellHandler";

		CellHandler cellHandler = cellAreaObj.GetComponent<CellHandler>();
		cellHandler.InitializeForGame();
		board.BoardCells = cellHandler;
	}


	private void LoadCharacters (GameBoard board)
	{
		for(int i=1; i <= 6; i ++)
		{
			int row = 2;
			int col = i %3;
			int team = 1;

			if(i > 3)
			{
				team = 2;
				row = 5;
			}

			Cell freeCell = board.BoardCells.GetCellAt(row, col);
			if(freeCell != null)
			{
				GameCharacter newChar = CharacterHandler.Instance.CreateCharacterOnCell(i, freeCell);
				newChar.SetTeam(team);
			}
			else{
				Debug.Log("null cell");
			}
		}
	}


	#endregion
}
