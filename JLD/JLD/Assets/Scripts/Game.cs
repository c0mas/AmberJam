using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/*
* Main game class; put all stuff we want to access here
**/

public class Game : MonoBehaviour
{
    public Level level;
    public Menu menu;
    public Character player_character;
    public Character[] model_characters;
    public GameObject globe;
    public Light light;
    public int counter = -1;

    private float readyTimer = 3.0f;

    public int min_moves = 2;
    public int max_moves = 3;
    public float press_time = 1.5f;
    public int score = 1;
    GameObject[] floor = new GameObject[100];
    GameObject[] ceil = new GameObject[100];
    GameObject[] bwall = new GameObject[100];

    public Material[] globeMaterials;
    float[] globeOn = {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};
    float[] globeOff = {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};
    float[] globeDt = { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
    int[] globeSt = { 0, 0, 0, 0, 0, 0 };
    float[] lightCol = { 1.0f, 0.0f, 0.0f };
    int[] upDownC = { 1, 0 };
    int colorS = 0;
    float colorD = 1.0f;

    float failTimer = -1.0f;

    public float game_play_time = 120;
    public float global_time;

    public float globeTimer = 0.0f;
    public float globeAS = 0.0f;
    public Vector3 globeAx;
    public int high_score = 0;

    public AudioSource music;
    public AudioClip[] songs;
    public int song_index = 0;

    public bool paused = false;

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

        song_index = UnityEngine.Random.Range(0, songs.Length);
        music.clip = songs[song_index];
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            player_character.gameObject.GetComponent<Animator>().enabled = false;
            for (int i = 0; i < model_characters.Length; i++)
                model_characters[i].gameObject.GetComponent<Animator>().enabled = false;
            menu.pause_button.gameObject.SetActive(false);
            menu.pause_menu.gameObject.SetActive(true);
        }
        else
        {
            player_character.gameObject.GetComponent<Animator>().enabled = true;
            for (int i = 0; i < model_characters.Length; i++)
                model_characters[i].gameObject.GetComponent<Animator>().enabled = true;
            menu.pause_button.gameObject.SetActive(true);
            menu.pause_menu.gameObject.SetActive(false);
        }

        paused = pause;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Intro");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void NegativeFeedback()
    {
        Handheld.Vibrate();
        FailFeedback();
    }

    public void WatchFinished()
    {
        player_character.Reset();
        for (int i = 0; i < model_characters.Length; i++)
            model_characters[i].Reset();
        SetGameState(GameState.Playing);
    }

    public void LevelFinished()
    {
        player_character.Reset();
        for (int i = 0; i < model_characters.Length; i++)
            model_characters[i].Reset();
        if (global_time < 0)
        {
            if (high_score < level.score)
                high_score = level.score;
            SetGameState(GameState.Finish);
        }
        else
        {
            if (max_moves < 6)
            {
                min_moves++;
                max_moves++;
            }
            if (press_time > 0.75f)
                press_time -= 0.1f;
            level.ResetMoves();
            if (menu.good_streak)
                menu.UpdateStreak(1);
            else
                menu.UpdateStreak(-1);
            SetGameState(GameState.Watching);
        }
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
                    menu.ShowModel(false);
                    menu.ShowCounter(-1);
                    menu.ShowStart(true);
                    menu.ShowAgain(false);
                    menu.ShowFinish(false);
                    music.Stop();
                    break;
                }
            case GameState.GettingReady:
                {
                    min_moves = 1;
                    max_moves = 2;
                    press_time = 1.5f;

                    level.Load();
                    readyTimer = 3.0f;
                    menu.ShowGameplay(false);
                    menu.ShowModel(false);
                    menu.ShowCounter(2);
                    menu.ShowStart(false);
                    menu.ShowAgain(false);
                    menu.ShowFinish(false);

                    level.score = 0;
                    level.streak = 1;

                    menu.UpdateStreak(-1);

                    int si = Random.Range(0, songs.Length);
                    while (si == song_index)
                        si = Random.Range(0, songs.Length);
                    song_index = si;
                    music.clip = songs[song_index];

                    music.Play();
                    break;
                }
            case GameState.Watching:
                {
                    menu.ShowGameplay(false);
                    menu.scoreTxt.gameObject.SetActive(true);
                    menu.streakTxt.gameObject.SetActive(true);
                    menu.ShowModel(true);
                    menu.ShowCounter(-1);
                    menu.ShowStart(false);
                    menu.ShowAgain(false);
                    menu.ShowFinish(false);
                    level.InitWatching();
                    break;
                }
            case GameState.Playing:
                {
                    menu.ShowGameplay(true);
                    menu.ShowModel(false);
                    menu.ShowCounter(-1);
                    menu.ShowStart(false);
                    menu.ShowAgain(false);
                    menu.ShowFinish(false);
                    level.InitPlaying();
                    break;
                }
            case GameState.Finish:
                {
                    menu.ShowGameplay(false);
                    menu.ShowModel(false);
                    menu.ShowCounter(-1);
                    menu.ShowStart(false);
                    menu.ShowAgain(true);
                    menu.ShowFinish(true);
                    music.Stop();
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

    public void FailFeedback()
    {
        failTimer = 0.25f;
    }

	void Update ()
    {
        if (paused)
            return;

        lightCol[upDownC[colorS]] += Time.deltaTime * colorD * 3.0f;
        if (lightCol[upDownC[colorS]] < 0.0f || lightCol[upDownC[colorS]] > 1.0f)
        {
            upDownC[colorS]++;
            upDownC[colorS] %= 3;
            colorS++;
            colorS %= 2;
            colorD = -colorD;
        }
        light.color = new Color(lightCol[0], lightCol[1], lightCol[2]);


        switch (mState)
        {
            case GameState.GettingReady:
                {
                    float dt = Time.deltaTime;
                    readyTimer -= dt;
                    counter = Mathf.FloorToInt(readyTimer);
                    menu.ShowCounter(counter);
                    global_time = game_play_time;
                    if (readyTimer < 0)
                        SetGameState(GameState.Watching);
                    break;
                }
            case GameState.Watching:
                {
                    float dt = Time.deltaTime;
                    global_time -= dt;
                    level.UpdateTime(dt);
                    //menu.UpdateTime(dt);
                    break;
                }
            case GameState.Playing:
                {
                    float dt = Time.deltaTime;
                    global_time -= dt;
                    menu.UpdateTime(dt);
                    level.UpdateTime(dt);
                    break;
                }
        }

        int k;
        globeTimer -= Time.deltaTime;
        if (globeTimer < 0.0f)
        {
            float x = UnityEngine.Random.Range(0.0f, 1.0f);
            float y = UnityEngine.Random.Range(0.0f, 1.0f);
            float z = UnityEngine.Random.Range(0.0f, 1.0f);
            globeAx = new Vector3(x, y, z);
            globeAx.Normalize();
            globeTimer = UnityEngine.Random.Range(1.0f, 3.0f);
            globeAS = UnityEngine.Random.Range(1.0f, 5.0f);

            if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.2f)
            {
                for (k = 0; k < 6; k++)
                {
                    globeDt[k] = -1.0f;
                    globeSt[k] = 1;
                    if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.2f)
                    {
                        globeOn[k] = UnityEngine.Random.Range(0.05f, 0.2f);
                        globeOff[k] = UnityEngine.Random.Range(0.05f, 0.2f);
                    }
                    else
                    {
                        globeOn[k] = -1.0f;
                    }
                }
            }
            else
            {
                globeAS = UnityEngine.Random.Range(5.0f, 7.0f);
                for (k = 0; k < 6; k++)
                {
                    globeDt[k] = -1.0f;
                    globeSt[k] = 1;
                    globeOn[k] = 5.0f;
                    globeOff[k] = 0.0f;
                }
            }
        }
        for (k = 0; k < 6; k++)
        {
            if (globeOn[k] > 0.0f)
            {
                globeDt[k] -= Time.deltaTime;
                if (globeDt[k] < 0.0f)
                {
                    if (globeSt[k] == 1)
                    {
                        globeSt[k] = 0;
                        globeDt[k] = globeOff[k];
                    }
                    else
                    {
                        globeSt[k] = 1;
                        globeDt[k] = globeOn[k];
                    }
                }
                if (globeSt[k] == 1)
                {
                    globeMaterials[k].SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0.5f));
                }
                else
                {
                    globeMaterials[k].SetColor("_TintColor", new Color(0.0f, 0.0f, 0.0f, 0.0f));
                }
            }
            else
            {
                globeMaterials[k].SetColor("_TintColor", new Color(0.0f, 0.0f, 0.0f, 0.0f));
            }
        }

        globe.transform.RotateAround(globeAx, globeAS * Time.deltaTime);

        int i, j;
        if (failTimer <= 0.0f)
        {
            float[] spectrum = AudioListener.GetSpectrumData(1024, 0, FFTWindow.Hamming);
           
            for (i = 0; i < 10; i++)
            {
                float n = spectrum[i * 2] * 30.0f;
                for (j = 0; j < 10; j++)
                {
                    Color c;
                    if (j < n - 1.0f)
                    {
                        c = new Color(0.7f, 0.7f, 0.0f, 1.0f);
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
            
                for (j = 0; j < 5; j++)
                {
                    Color c;
                    if (j < n - 1.0f)
                    {
                        c = new Color(0.0f, 0.7f, 0.0f, 1.0f);
                    }
                    else
                    {
                        c = new Color(0.0f, 0.3f, 0.0f, 1.0f);
                    }
                    GameObject o = bwall[i * 10 + 4 - j];
                    MeshRenderer renderer;
                    renderer = o.GetComponent<MeshRenderer>() as MeshRenderer;
                    renderer.material.color = c;

                    o = bwall[i * 10 + j + 5];
                    renderer = o.GetComponent<MeshRenderer>() as MeshRenderer;
                    renderer.material.color = c;
                }
            }
        }
        if (failTimer > 0.0f)
        {
            failTimer -= Time.deltaTime;
            float sq = failTimer / 0.125f;
            float s = sq - Mathf.Floor(sq);
            Color c;
            if (s < 0.5f)
            {
                c = new Color(0.3f, 0.0f, 0.0f, 1.0f);
            }
            else
            {
                c = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
            for (j = 0; j < 100; j++)
            {
                GameObject o = floor[j];
                MeshRenderer renderer;
                renderer = o.GetComponent<MeshRenderer>() as MeshRenderer;
                renderer.material.color = c;
            }
            for (j = 0; j < 100; j++)
            {
                GameObject o = ceil[j];
                MeshRenderer renderer;
                renderer = o.GetComponent<MeshRenderer>() as MeshRenderer;
                renderer.material.color = c;
            }
            for (j = 0; j < 100; j++)
            {
                GameObject o = bwall[j];
                MeshRenderer renderer;
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
                Vector3 pos = new Vector3(x * spacingX, -3.0f, y * spacingY);
                GameObject o = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
                floor[count] = o;
                pos = new Vector3(x * spacingX, 7.0f, y * spacingY);
                o = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
                ceil[count] = o;
                o.GetComponent<Renderer>().transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 180.0f); //.localScale = new Vector3(1.0f, -1.0f, 1.0f);
                count++;
            }
        }
        count = 0;
        for (float x = -gridX; x < gridX; x += 1.0f)
        {
            for (int iy = 0; iy < 10; iy++)
            {
                Vector3 pos = new Vector3(x * spacingX, iy * 1.0f - 2.45f, gridY * 2.0f * spacingY);
                GameObject o = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
                o.GetComponent<Renderer>().transform.localScale = new Vector3(0.1f, 0.01f, 0.07f);
                o.GetComponent<Renderer>().transform.Rotate(new Vector3(1.0f, 0.0f, 0.0f), -90.0f);
                bwall[count] = o;
                count++;
            }
        }
    }
}
