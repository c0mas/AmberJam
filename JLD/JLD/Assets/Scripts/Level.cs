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
    }


    public Game game;
    public Move[] moves;

    public int current_move;
    public float timer;
	public bool anim_set = false;
    public bool anim_reset = false;
    public int score;
    public int streak;

    public bool watch = false;

    //TODO load level here
	public void Load()
    {
        current_move = 0;
        score = 0;
        streak = 0;

        game.player_character.Reset();
        for (int i = 0; i < game.model_characters.Length; i++)
            game.model_characters[i].Reset();

        //generate moves
        moves = null;
        int nr_moves = UnityEngine.Random.Range(game.min_moves, game.max_moves);
        moves = new Move[nr_moves];
        for (int i = 0; i < nr_moves; i++)
        {
            moves[i] = new Move();
            int move = 1;// UnityEngine.Random.Range(1, 6);
            moves[i].button = move;
            moves[i].animation = "dance" + move.ToString();
            moves[i].time = game.model_characters[0].GetMoveTime(move);

        }

        if (moves == null || moves.Length == 0)
            return;

        timer = moves[current_move].time;
        anim_set = false;
        anim_reset = false;
    }

    public void UpdateTime(float dt)
    {
        if (watch)
            UpdateWatch(dt);
        else
            UpdatePlay(dt);
    }

    public void UpdatePlay(float dt)
    {
        if (moves == null || moves.Length == 0)
            return;

        timer -= dt;
        if (timer < game.menu.time - 0.1f)
        {
            if (!anim_reset)
            {
                game.player_character.Reset();
                anim_reset = true;
            }
        }
        if (timer < 0)
        {
            NextMove();
        }
    }


    public void UpdateWatch(float dt)
    {
        timer -= dt;
        if (timer < moves[current_move].time - (moves[current_move].time / 2))
        {
            if (!anim_reset)
            {
                for (int i = 0; i < game.model_characters.Length; i++)
                    game.model_characters[i].Reset();
                anim_reset = true;
            }
        }
        
        if (timer < 0)
        {
            current_move++;
            if (current_move >= moves.Length)
            {
                current_move--;
                watch = false;
                game.WatchFinished();
            }
            else
            {
                timer = moves[current_move].time;
                for (int i = 0; i < game.model_characters.Length; i++)
                {
                    game.model_characters[i].Reset();
                    game.model_characters[i].SetAnimation(moves[current_move].animation);
                }
                anim_set = false;
                anim_reset = false;
            }
        }
    }

    void NextMove()
    {
        current_move++;
        if (current_move >= moves.Length)
        {
            if (current_move == moves.Length)
            {
                timer = moves[current_move - 1].time;
                game.menu.time = 1000;
                game.menu.SetMove(moves[current_move - 1]);
            }
            else
                game.LevelFinished();
        }
        else
        {
            timer = moves[current_move].time;
            game.menu.time = timer;
            game.menu.SetMove(moves[current_move]);
            //game.player_character.Reset();
            anim_set = false;
            anim_reset = false;
        }
    }

    public void InitWatching()
    {
        current_move = 0;
        score = 0;
        streak = 0;

        watch = true;

        game.player_character.Reset();
        for (int i = 0; i < game.model_characters.Length; i++)
        {
            game.model_characters[i].Reset();
            game.model_characters[i].SetAnimation(moves[current_move].animation);
        }
    }

    public void InitPlaying()
    {
        current_move = 0;
        score = 0;
        streak = 0;

        watch = false;

        game.player_character.Reset();
        for (int i = 0; i < game.model_characters.Length; i++)
            game.model_characters[i].Reset();

        timer = game.press_time;
        game.menu.time = game.press_time;
        game.menu.SetMove(moves[current_move]);
    }
}
