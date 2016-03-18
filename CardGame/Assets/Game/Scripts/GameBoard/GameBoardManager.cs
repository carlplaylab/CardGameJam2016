using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GameBoardManager : MonoBehaviour 
{

	private static GameBoardManager instance;
	public static GameBoardManager Instance
	{
		get { return instance; }
	}

	private GameSettings gameSettings;
	private GameBoard gameBoard;
	private ElementSource elementUI;
	private Dictionary<BoardState, GBState> gameStates;
	private GBState currentState;

	private bool initialized = false;
	private int currentTeam = 1;


	public GameSettings Settings
	{
		get { return gameSettings; }
	}

	public int CurrentTeam
	{
		get { return currentTeam; }
	}


	public ElementSource ElementUI
	{
		get { return elementUI; }
	}


	public GameBoard  Board
	{
		get { return gameBoard; }
	}


	void Awake ()
	{
		instance = this;
	}


	void Start ()
	{
		Initialize();
	}


	void OnDestroy ()
	{
		CharacterDatabase.Instance.Destroy();
		IngameDataCenter.Instance.Destroy();
		IngameSpriteCenter.Instance.Destroy();
	}

	private void Initialize ()
	{
		if(initialized)
			return;

		gameSettings = GetComponentInChildren<GameSettings>();

		gameBoard = GetComponentInChildren<GameBoard>();
		gameBoard.onPlayerMoveEnded = OnPlayerTurnEnd;

		elementUI = GetComponentInChildren<ElementSource>();
		elementUI.SetVisible(false);

		gameStates = new Dictionary<BoardState, GBState>();
		GBStateLoading loadState = new GBStateLoading();
		GBStateResourceAdding resourceState = new GBStateResourceAdding();
		GBStatePlaying playState = new GBStatePlaying();
		GBStateOponentsTurn oponentState = new GBStateOponentsTurn();
		gameStates.Add(BoardState.LOADING, (GBState)loadState);
		gameStates.Add(BoardState.RESOURCE_ADDING, (GBState)resourceState);
		gameStates.Add(BoardState.PLAYER_TURN, (GBState)playState);
		gameStates.Add(BoardState.OPONENTS_TURN, (GBState)oponentState);

		currentState = (GBState)loadState;
		currentState.Start(gameBoard);

		initialized = true;
	}


	public void SetState(BoardState newState)
	{
		if(currentState == null)
		{
			currentState = gameStates[newState];
			return;
		}

		if(currentState.GetState() == newState)
		{
			return;
		}

		//Debug.Log("SetState : " + newState);

		currentState.End(gameBoard);
		currentState = gameStates[newState];
		currentState.Start(gameBoard);
	}


	void Update ()
	{
		if(currentState != null)
			currentState.Update(gameBoard);
	}


	public void OnPlayerTurnEnd ()
	{
		currentTeam++;
		if(currentTeam > 2)
		{
			currentTeam = 1;
		}

		gameBoard.SetTeam(CurrentTeam);
		if(currentTeam == 1)
		{
			SetState(BoardState.PLAYER_TURN);
		}
		else 
		{
			SetState(BoardState.OPONENTS_TURN);
		}

		Parameters playerTurnParams = new Parameters();
		playerTurnParams.PutExtra("currentteam", currentTeam);
		EventBroadcaster.Instance.PostEvent(EventNames.PLAYER_TURN_TOGGLED, playerTurnParams);
	}


	public void StartDragCardOnBoard( int cardId )
	{
		currentState.StartDragCardOnBoard(gameBoard, cardId);
	}
		
	public bool EndDragCardOnBoard( int cardId )
	{
		// return true if card was converted to character
		bool characterAdded = currentState.EndDragCardOnBoard(gameBoard, cardId);
		if(characterAdded)
		{
			Invoke( "OnPlayerTurnEnd", 0.05f);
			return true;
		}
		return false;
	}

}
