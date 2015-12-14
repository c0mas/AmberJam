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
    public Sprite[] images;
    public Button playGame;
    public Button playAgain;
    public Text scoreTxt;
    public Text streakTxt;
    public Text t1, t2, t3;
    public Text[] ttext;

    public Level.Move current_move;

    int good_button = 0;
    bool buttons_visible = true;
    public float timer;
    bool button_pressed = false;
    public bool good_move = false;

    public float time;


	public void Init()
    {
        
    }

    public void PressButton(int button_index)
    {
        if (!button_pressed)
        {
            button_pressed = true;
            if (button_index == good_button)
            {
                good_move = true;
                buttons[button_index].image.color = new Color(0, 1, 0);
            }
            else
            {
                good_move = false;
                buttons[button_index].image.color = new Color(1, 0, 0);
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
        timer -= dt;

        if (timer < time - game.press_time)
        {
            if (!buttons_visible)
            {
                buttons_visible = true;
                foreach (Button b in buttons)
                {
                    b.image.color = new Color(1, 1, 1);
                    b.gameObject.SetActive(true);
                }
            }
        }
        if (timer < 0)
        {
            if (good_move)
            {
                game.level.score++;
                UpdateScore();
                UpdateStreak(1);
                game.player_character.SetAnimation(current_move.animation);
            }
            else
            {
                game.player_character.Reset();
            }
        }
    }

    public void SetMove(Level.Move new_move)
    {
        current_move = new_move;

        button_pressed = false;

        good_move = false;

        good_button = UnityEngine.Random.Range(0, buttons.Length);
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == good_button)
                buttons[i].image.sprite = images[current_move.button];
            else
            {
                bool different_image = false;
                do
                {
                    int image_index = UnityEngine.Random.Range(0, images.Length);
                    if (image_index != current_move.button)
                    {
                        buttons[i].image.sprite = images[image_index];
                        different_image = true;
                    }
                }
                while (different_image == false);
            }
        }

        buttons_visible = false;
        foreach (Button b in buttons)
            b.gameObject.SetActive(false);

        UpdateScore();
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
