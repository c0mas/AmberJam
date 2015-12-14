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
    public bool anim_set = false;

    //TODO load level here
	public void Load()
    {
        current_move = 0;
        timer = moves[current_move].time;
        game.model_character.SetAnimation(moves[current_move].animation);
        game.menu.SetMove(moves[current_move]);
        anim_set = false;
    }

    public void UpdateTime(float dt)
    {
        timer -= dt;
        if (timer < moves[current_move].time - (moves[current_move].press_time / 2))
        {
            if (!anim_set)
            {
                game.model_character.SetAnimation(moves[current_move].animation);
                anim_set = true;
            }
        }
        if (timer < 0)
        {
            NextMove();
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
            anim_set = false;
            game.menu.SetMove(moves[current_move]);
        }
    }
}
