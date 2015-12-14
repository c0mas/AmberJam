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
    GameObject[] floor = new GameObject[100];
    GameObject[] ceil = new GameObject[100];

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
        InitFloor();
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
        float[] spectrum = AudioListener.GetSpectrumData(1024, 0, FFTWindow.Hamming);
        int i, j;
        for (i = 0; i < 10; i++)
        {
            float n = spectrum[i * 2] * 30.0f;
            for (j = 0; j < 10; j++)
            {
                Color c;
                if (j < n)
                {
                    c = new Color(1.0f, 1.0f, 0.0f, 1.0f);
                }
                else
                {
                    c = new Color(0.3f, 0.3f, 0.0f, 1.0f);
                }
                GameObject o = floor[i * 10 + j];
                MeshRenderer renderer;
                renderer = o.GetComponent<MeshRenderer>() as MeshRenderer;
                renderer.material.color = c;

                o = ceil[i * 10 + j];
                renderer = o.GetComponent<MeshRenderer>() as MeshRenderer;
                renderer.material.color = c;
            }
        }
	}

    public GameObject prefab;
    public float gridX = 5f;
    public float gridY = 5f;
    public float spacingX = 1.3f;
    public float spacingY = 1.5f;

    void InitFloor()
    {
        Material gglow = Resources.Load("Material1", typeof(Material)) as Material;

        int count = 0;
        for (float x = -gridX; x < gridX; x += 1.0f)
        {
            for (float y = 0; y < gridY * 2.0f; y += 1.0f)
            {
                Vector3 pos = new Vector3(x * spacingX, -1.0f, y * spacingY);
                GameObject o = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
                floor[count] = o;
                pos = new Vector3(x * spacingX, 7.0f, y * spacingY);
                o = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
                ceil[count] = o;
                o.GetComponent<Renderer>().transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 180.0f); //.localScale = new Vector3(1.0f, -1.0f, 1.0f);
                count++;
               // renderer.material.SetColor("_Emission", Color.white);
            }
        }
    }
}
