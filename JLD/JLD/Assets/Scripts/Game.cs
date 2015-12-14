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

    public void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }

    public void LevelFinished()
    {
        player_character.Reset();
        model_character.Reset();
    }
	
	void Start ()
    {
        level.Load();
        menu.Init();
	}
	
	
	void Update ()
    {
        float dt = Time.deltaTime;
        level.UpdateTime(dt);
        menu.UpdateTime(dt);
	}
}
