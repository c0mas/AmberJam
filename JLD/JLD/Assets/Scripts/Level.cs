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
    public int score;
    public int streak;

    //TODO load level here
	public void Load()
    {
        current_move = 0;
        score = 0;
        streak = 0;

        game.player_character.Reset();
        game.model_character.Reset();

        if (moves == null || moves.Length == 0)
            return;

        timer = moves[current_move].time;
        game.menu.SetMove(moves[current_move]);
        anim_set = false;
    }

    public void UpdateTime(float dt)
    {
        if (moves == null || moves.Length == 0)
            return;

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
            current_move--;
            game.LevelFinished();
        }
        //else
        {
            timer = moves[current_move].time;
            //TODO set moves for characters
            game.menu.SetMove(moves[current_move]);
        }
    }
}
