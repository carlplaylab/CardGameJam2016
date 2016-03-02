using UnityEngine;
using System.Collections;

/*
 * Holder for event names
 * Created By: NeilDG
 */ 
public class EventNames {

	// Reel Ingame
	public static string SPIN_TRIGGERED = "SPIN_TRIGGERED";
	public static string STOP_TRIGGERED = "STOP_TRIGGERED";
	public static string FORCE_REEL_MOVEMENT = "FORCE_REEL_MOVEMENT";
	public static string FORCE_REEL_ANALYSIS = "FORCE_REEL_ANALYSIS";
	public static string STOP_REEL_EFFECTS = "STOP_REEL_EFFECTS";

	// Logging on UI
	public static string DEBUG_TEXT_CALLED = "DEBUG_TEXT_CALLED";
	public static string DEBUG_INGAME_UI_ACTIVE = "DEBUG_INGAME_UI_ACTIVE";
	public static string DEBUG_SPECIFIC_LINE = "DEBUG_SPECIFIC_LINE";
	public static string DEBUGREEL_FORCETOGGLE = "DEBUGREEL_FORCETOGGLE";

	// Line control
	public static string LINE_CONTROL_ADJUSTED = "LINE_CONTROL_ADJUSTED";
	public static string LINE_BUTTON_CLICKED = "LINE_BUTTON_CLICKED";
	public static string LINE_BUTTON_SHOW = "LINE_BUTTON_SHOW";
	public static string LINE_BUTTON_HIDE = "LINE_BUTTON_HIDE";
	public static string LINE_DISPLAY_SETUP = "LINE_DISPLAY_SETUP";
	public static string LINE_DISPLAY_UPDATE = "LINE_DISPLAY_UPDATE";
	public static string LINE_CONTROL_UPDATE = "LINE_CONTROL_UPDATE";

	// Spin control
	public static string SPIN_CONTROL_ADJUSTED = "SPIN_CONTROL_ADJUSTED";
	public static string SPIN_UI_UPDATE = "SPIN_UI_UPDATE";
	public static string FREE_SPINS_FINISHED = "FREE_SPINS_FINISHED";
	public static string FREE_SPINS_ADDED = "FREE_SPINS_ADDED";
	public static string ON_NEW_SPIN = "ON_NEW_SPIN";
	public static string SHOW_STOPSPIN = "SHOW_STOPSPIN";
	
	// Bet control
	public static string BET_CONTROL_ADJUSTED = "BET_CONTROL_ADJUSTED";
	public static string BET_AUTO_ADJUST = "BET_AUTO_ADJUST";
	public static string BET_UI_UPDATE = "BET_UI_UPDATE";
	public static string GIVE_BONUS_WINNINGS = "GIVE_BONUS_WINNINGS";
	public static string BETS_INITIALIZED = "BETS_INITIALIZED";
	
	// Slot machine state control
	public static string UPDATE_SLOTMACHINE_STATE = "UPDATE_SLOTMACHINE_STATE";
	public static string LEAVE_SLOTMACHINE = "LEAVE_SLOTMACHINE";

	// Results Analysis
	public static string COMPLETED_RESULT_ANALYSIS = "COMPLETED_RESULT_ANALYSIS";
	public static string LOG_RESULT = "LOG_RESULT";
	public static string LOG_BONUS_RESULT = "LOG_BONUS_RESULT";
	public static string LOG_VIEWER_REDRAW = "LOG_VIEWER_REDRAW";

	//UI Event Names
	public const string UI_GAMEINFO_SETUP = "UI_GAMEINFO_SETUP";
	public const string ON_UPDATE_COIN_VALUE = "ON_UPDATE_COIN_VALUE";
	public const string ON_MINIGAME_HIDDEN = "ON_MINIGAME_HIDDEN";
	public const string ON_COMMENDATION_HIDDEN = "ON_COMMENDATION_HIDDEN";
	public const string SET_CONTROLS_ACTIVE = "SET_CONTROLS_ACTIVE";
	public const string ON_DOUBLEXP_PURCHASED = "ON_DOUBLEXP_PURCHASED";
	public const string UI_MINIGAME_UPDATE = "UI_MINIGAME_UPDATE";
	public const string LEVEL_UP_HIDDEN = "LEVEL_UP_HIDDEN";
	public const string SLOT_UNLOCKED_HIDDEN = "SLOT_UNLOCKED_HIDDEN";
	public const string FREESPIN_RESULTS_HIDDEN = "FREESPIN_RESULTS_HIDDEN";
	public const string ON_UPDATE_SALE_DISPLAY = "ON_UPDATE_SALE_DISPLAY";
	public const string ON_ACTIVATE_DAILY_DOUBLE_PRICE = "ON_ACTIVATE_DAILY_DOUBLE_PRICE";
	public const string INGAME_PLAY_COIN_PARTICLES = "INGAME_PLAY_COIN_PARTICLES";
	public const string FREESPIN_POPUP_HIDDEN = "FREESPIN_POPUP_HIDDEN";
	public const string FREESPIN_START_HIDDEN = "FREESPIN_START_HIDDEN";
	public const string FREESPIN_GAME_HIDDEN = "FREESPIN_GAME_HIDDEN";
	public const string FREESPIN_GAMERESULT_HIDDEN = "FREESPIN_GAMERESULT_HIDDEN";
	public const string INITIALIZE_SLOTS_CONTROL = "INITIALIZE_SLOTS_CONTROL";
	public const string TRIGGER_COIN_EFFECTS = "TRIGGER_COIN_EFFECTS";
	public const string RATE_US_HIDDEN = "RATE_US_HIDDEN";
	public const string DOUBLE_EXP_PREVIEW_HIDDEN = "DOUBLE_EXP_PREVIEW_HIDDEN";
	public const string UPDATE_FAVORITE_ICON_DISPLAY = "UPDATE_FAVORITE_ICON_DISPLAY";
	public const string SHOW_SLOTS_CONTROL_OVERLAY = "SHOW_SLOTS_CONTROL_OVERLAY";
	public const string HIDE_SLOTS_CONTROL_OVERLAY = "HIDE_SLOTS_CONTROL_OVERLAY";
	public const string ON_UPDATE_AVATAR_SIDE_ITEM = "ON_UPDATE_AVATAR_SIDE_ITEM";

	// Minigame
	public const string TRIGGER_MINI_GAME_LOADING = "TRIGGER_MINI_GAME_LOADING";
	public const string TRIGGER_MINI_GAME = "TRIGGER_MINI_GAME";
	public const string HIDE_MINI_GAME = "HIDE_MINI_GAME";
	public const string MINIGAME_INTRO_HIDDEN = "MINIGAME_INTRO_HIDDEN";
	public const string REEL_ITEM_GAME_INTRO_HIDDEN = "REEL_ITEM_GAME_INTRO_HIDDEN";
	public const string REEL_ITEM_GAME_RESULT_HIDDEN = "REEL_ITEM_GAME_RESULT_HIDDEN";
	public const string REEL_ITEM_GAME_HIDDEN = "REEL_ITEM_GAME_HIDDEN";

	//UI Events (Not-ingame)
	public const string TO_GAME_LOBBY_ON_RECONNECT = "TO_GAME_LOBBY_ON_RECONNECT";
	public const string INGAME_WINDOW_CLOSED = "INGAME_WINDOW_CLOSED";
	public const string CLOSE_SLOTMACHINE_TUTORIAL = "CLOSE_SLOTMACHINE_TUTORIAL";
	public const string BACK_BUTTON_CLICKED = "BACK_BUTTON_CLICKED";

	// Effects
	public const string INITIALIZE_EFFECTS = "INITIALIZE_EFFECTS";
	public const string SHOW_5_OF_A_KIND = "SHOW_5_OF_A_KIND";
	public const string SHOW_BIG_WIN = "SHOW_BIG_WIN";
	public const string SHOW_MEGA_WIN = "SHOW_MEGA_WIN";
	public const string SHOW_ULTRA_WIN = "SHOW_ULTRA_WIN";
	public const string SHOW_REGULAR_WIN = "SHOW_REGULAR_WIN";
	public const string SHOW_FREESPIN_WIN = "SHOW_FREESPIN_WIN";
	public const string REEL_ANIMATION_EFFECTS_ENDED = "REEL_ANIMATION_EFFECTS_ENDED";
	//public const string CHECK_SHOW_RATE_US = "CHECK_SHOW_RATE_US";

	// Pause
	public const string TRIGGER_PAUSE = "TRIGGER_PAUSE";
	public const string TRIGGER_RESUME = "TRIGGER_RESUME";

	//Social Features
	public const string UPDATE_INTERACTION_CONTENT = "REFRESH_COLLECT_GIFT_CONTENT";
	public const string UPDATE_GIFT_NOTIFICATION = "UPDATE_GIFT_NOTIFICATION";
	
	//Info Screen
	public const string ON_4_PAGE_INFO = "ON_4_PAGE_INFO";
	public const string ON_5_PAGE_INFO = "ON_5_PAGE_INFO";
	public const string ON_INFO_PAGE_SWITCH = "ON_INFO_PAGE_SWITCH";

	//Tournament
	public const string ON_TOURNAMENT_RETRIEVE_SUCCESS = "ON_TOURNAMENT_RETRIEVE_SUCCESS";
	public const string ON_TOURNAMENT_RETRIEVE_FAIL = "ON_TOURNAMENT_RETRIEVE_FAIL";
	public const string ON_TOURNAMENT_SELECTED_TEST = "ON_TOURNAMENT_SELECTED_TEST"; //only applicable to TournamentWorkspace scene
	public const string ON_TOURNAMENT_WINNING_RECEIVED = "ON_TOURNAMENT_WINNING_RECEIVED";
	public const string ON_TOURNAMENT_FINISHED = "ON_TOURNAMENT_FINISHED";
	public const string ON_TOURNAMENT_LIST_REFRESH = "ON_TOURNAMENT_LIST_REFRESH";
	public const string ON_TOURNAMENT_AUTO_JOIN = "ON_TOURNAMENT_AUTO_JOIN";
	public const string ON_TOURNAMENT_COLLAPSE = "ON_TOURNAMENT_COLLAPSE";
	public const string ON_TOURNAMENT_EXPAND = "ON_TOURNAMENT_EXPAND";
	public const string ON_TOURNAMENT_SHOW = "ON_TOURNAMENT_SHOW";
	public const string ON_TOURNAMENT_HIDE = "ON_TOURNAMENT_HIDE";

	public const string ON_CHANGE_TO_COIN_TRANSACTION = "ON_CHANGE_TO_COIN_TRANSACTION";
	public const string ON_CHANGE_TO_CREDIT_TRANSACTION = "ON_CHANGE_TO_CREDIT_TRANSACTION";

	//Swrve Trigger Prefs
	public const string ON_TRIGGER_DOUBLE_EXP_PREVIEW = "ON_TRIGGER_DOUBLE_EXP_PREVIEW";
}