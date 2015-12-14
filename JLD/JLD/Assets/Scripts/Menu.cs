using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
* Main menu
**/

public class Menu : MonoBehaviour
{
    public Game game;
    public Level level;
    public Button[] buttons;
    public Text scoreTxt;
    public Text streakTxt;

    public Level.Move current_move;
    private bool moveFailed = false;

	public void Init()
    {

    }

    public void PressButton(int button_index)
    {
        if (button_index != current_move.button)
        {
            //bad stuff
            moveFailed = true;
            UpdateStreak(-1);
            return;
        }

       if (game.level.timer > (current_move.time - current_move.press_time))
        {
            //good_stuff
            game.player_character.SetAnimation(current_move.animation);
  level.score += current_move.score;
            UpdateStreak(1);
            UpdateScore();
        }
        else
        {
            moveFailed = true;
            UpdateStreak(-1);
        }
    }

    public void UpdateScore()
    {
        scoreTxt.text = "Score: " + level.score.ToString();
    }

    public void UpdateStreak(int streak)
    {
        if (streak < 0)
        {
            level.streak = 0;
        }
        else
        {
            level.streak += streak;
        }
        streakTxt.text = "Streak: " + level.streak.ToString();
    }

    public void UpdateTime(float dt)
    {
     Color c = new Color(1.0f, 1.0f, 1.0f, 0.25f);
        foreach(Button b in buttons)
        {
            b.image.color = c;
        }
        if (!moveFailed)
        {
            if (game.level.timer > (current_move.time - current_move.press_time))
            {
                float procent = (current_move.time - level.timer) / current_move.press_time;
                float alpha = procent * 2.0f;
                if (alpha > 1.0f)
                    alpha = 2.0f - alpha;
                c = new Color(0.0f, 1.0f, 0.0f, 0.25f + alpha * (0.75f / 0.5f)); //alpha;
            }
        }
        else
        {
            c = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        }

        buttons[current_move.button].image.color = c;
    }

    public void SetMove(Level.Move new_move)
    {
        current_move = new_move;
        moveFailed = false;
        //TODO reset button
    }
}
