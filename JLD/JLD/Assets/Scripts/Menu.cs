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
    public Button playGame;
    public Button playAgain;
    public Text scoreTxt;
    public Text streakTxt;
    public Text t1, t2, t3;
    public Text[] ttext;

    public Level.Move current_move;
    private bool moveFailed = false;

    private bool[] buttons_to_press;

	public void Init()
    {
        buttons_to_press = null;
        buttons_to_press = new bool[buttons.Length];
        for (int i = 0; i < buttons_to_press.Length; i++)
        {
            buttons_to_press[i] = false;
        }
    }

    public void PressButton(int button_index)
    {
        bool button_exists = false;
        for (int i = 0; i < current_move.buttons.Length; i++)
        {
            if (button_index == current_move.buttons[i])
            {
                button_exists = true;
                if (game.level.timer > (current_move.time - current_move.press_time))
                    buttons_to_press[current_move.buttons[i]] = true;
                break;
            }
        }
        if (!button_exists)
        {
            //bad stuff
            moveFailed = true;
            UpdateStreak(-1);
            return;
        }

        bool all_buttons_pressed = true;
        for (int i = 0; i < current_move.buttons.Length; i++)
        {
            if (buttons_to_press[current_move.buttons[i]] == false)
            {
                all_buttons_pressed = false;
                break;
            }
        }

        if (game.level.timer > (current_move.time - current_move.press_time))
        {
            if (all_buttons_pressed)
            {
                //good_stuff
                game.player_character.SetAnimation(current_move.animation);
                level.score += current_move.score;
                UpdateStreak(1);
                UpdateScore();
            }
        }
        else
        {
            if (all_buttons_pressed)
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
        if (game.level.timer < (current_move.time - current_move.press_time))
        {
            for (int i = 0; i < current_move.buttons.Length; i++)
            {
                if (buttons_to_press[current_move.buttons[i]] == false)
                {
                    moveFailed = true;
                    break;
                }
            }
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
            UpdateStreak(-1);
            c = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        }

        for (int i = 0; i < current_move.buttons.Length; i++)
            buttons[current_move.buttons[i]].image.color = c;
    }

    public void SetMove(Level.Move new_move)
    {
        current_move = new_move;
        moveFailed = false;

        for (int i = 0; i < buttons_to_press.Length; i++)
        {
            buttons_to_press[i] = false;
        }

        //TODO reset button
    }

    public void ShowGameplay(bool s)
    {
        foreach (Button b in buttons)
        {
            b.gameObject.SetActive(s);
        }
        scoreTxt.gameObject.SetActive(s);
        streakTxt.gameObject.SetActive(s);

    }

    public void ShowStart(bool s)
    {
        playGame.gameObject.SetActive(s);
    }

    public void ShowAgain(bool s)
    {
        playAgain.gameObject.SetActive(s);
    }

    public void ShowCounter(int id)
    {
        int i;
        for (i = 0; i < 3; i++)
        {
            if (id == i)
                ttext[i].gameObject.SetActive(true);
            else
                ttext[i].gameObject.SetActive(false);
        }
    }

    public void PlayGame()
    {
        game.SetGameState(Game.GameState.GettingReady);
    }

    public void PlayAgain()
    {
        game.SetGameState(Game.GameState.GettingReady);
    }
}
