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
	private Dictionary<BoardState, GBState> gameStates;
	private GBState currentState;
	private bool initialized = false;



	public GameSettings Settings
	{
		get { return gameSettings; }
	}


	void Awake ()
	{
		instance = this;

		gameSettings = GetComponentInChildren<GameSettings>();
		gameBoard = GetComponentInChildren<GameBoard>();
	}


	void Start ()
	{
		Initialize();
	}


	private void Initialize ()
	{
		if(initialized)
			return;

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

		currentState.End(gameBoard);
		currentState = gameStates[newState];
		currentState.Start(gameBoard);
	}


	void Update ()
	{
		if(currentState != null)
			currentState.Update(gameBoard);
	}
}
