using UnityEngine;
using System.Collections;

/*
* Main game class; put all stuff we want to access here
**/

public class Game : MonoBehaviour
{
    public Level level;
    public Menu menu;
    public Character player_character;
    public Character model_character;
    public int counter = -1;

    private float readyTimer = 3.0f;

    public int min_moves = 2;
    public int max_moves = 4;
    public float press_time = 1.2f;
    public int score = 1;

    public enum GameState
    {
        WaitForStart,
        GettingReady,
        Watching,
        Playing,
        Finish
    }

    public GameState mState = GameState.WaitForStart;

    public void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }

    public void WatchFinished()
    {
        player_character.Reset();
        model_character.Reset();
        SetGameState(GameState.Playing);
    }

    public void LevelFinished()
    {
        player_character.Reset();
        model_character.Reset();
        SetGameState(GameState.Finish);
    }

    public void SetGameState(GameState state)
    {
        //if (mState == state)
        //    return;
        switch(state)
        {
            case GameState.WaitForStart:
                {
                    menu.ShowGameplay(false);
                    menu.ShowCounter(-1);
                    menu.ShowStart(true);
                    menu.ShowAgain(false);
                    break;
                }
            case GameState.GettingReady:
                {
                    level.Load();
                    readyTimer = 3.0f;
                    menu.ShowGameplay(false);
                    menu.ShowCounter(2);
                    menu.ShowStart(false);
                    menu.ShowAgain(false);
                    break;
                }
            case GameState.Watching:
                {
                    menu.ShowGameplay(false);
                    menu.ShowCounter(-1);
                    menu.ShowStart(false);
                    menu.ShowAgain(false);
                    level.InitWatching();
                    break;
                }
            case GameState.Playing:
                {
                    menu.ShowGameplay(true);
                    menu.ShowCounter(-1);
                    menu.ShowStart(false);
                    menu.ShowAgain(false);
                    level.InitPlaying();
                    break;
                }
            case GameState.Finish:
                {
                    menu.ShowGameplay(false);
                    menu.ShowCounter(-1);
                    menu.ShowStart(false);
                    menu.ShowAgain(true);
                    break;
                }
        }

        mState = state;
    }
	
	void Start ()
    {
        menu.Init();
        SetGameState(GameState.WaitForStart);
	}

	void Update ()
    {
        switch (mState)
        {
            case GameState.GettingReady:
                {
                    float dt = Time.deltaTime;
                    readyTimer -= dt;
                    counter = Mathf.FloorToInt(readyTimer);
                    menu.ShowCounter(counter);
                    if (readyTimer < 0)
                        SetGameState(GameState.Watching);
                    break;
                }
            case GameState.Watching:
                {
                    float dt = Time.deltaTime;
                    level.UpdateTime(dt);
                    //menu.UpdateTime(dt);
                    break;
                }
            case GameState.Playing:
                {
                    float dt = Time.deltaTime;
                    menu.UpdateTime(dt);
                    level.UpdateTime(dt);
                    break;
                }
        }
	}
}
