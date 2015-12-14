using UnityEngine;
using System.Collections;
using System;

/*
* Class that represents a level; that is a sequence of timed button presses and animations
**/

public class Level : MonoBehaviour
{
    [Serializable]
    public struct Move
    {
        public int button;
        public string animation;
        public float time;
        public float press_time;
        public int score;
    }


    public Game game;
    public Move[] moves;

    public int current_move;
    public float timer;

    //TODO load level here
	public void Load()
    {
        current_move = 0;
        timer = moves[current_move].time;
        // TODO set moves for characters
        game.menu.SetMove(moves[current_move]);
    }

    public void UpdateTime(float dt)
    {
        timer -= dt;
        if (timer < 0)
        {

        }
    }

    void NextMove()
    {
        current_move++;
        if (current_move >= moves.Length)
        {
            game.LevelFinished();
        }
        else
        {
            timer = moves[current_move].time;
            //TODO set moves for characters
            game.menu.SetMove(moves[current_move]);
        }
    }
}
