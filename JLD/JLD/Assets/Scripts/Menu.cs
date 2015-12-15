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
    public Button[] model_buttons;
    public Sprite[] images;
    public Button playGame;
    public Button playAgain;
    public Text scoreTxt;
    public Text streakTxt;
    public Text t1, t2, t3;
    public Text[] ttext;

    public Text text_score;
    public Text text_high_score;

    public Text text_wait;
    public Text text_prepare;

    public Button pause_button;
    public Canvas pause_menu;

    public Level.Move current_move;

    int good_button = 0;
    bool buttons_visible = true;
    public float timer = 0;
    bool button_pressed = false;
    public bool good_move = false;

    public int normal_score = 100;
    public int score_multiplier = 1;
    public bool good_streak = false;

    public GameObject perfect_feedbak;

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
                buttons[button_index].GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
                if (timer < (game.press_time / 5))
                    score_multiplier = 1;
                else if (timer < (game.press_time / 5) * 2)
                    score_multiplier = 2;
                else if (timer < (game.press_time / 5) * 3)
                    score_multiplier = 3;
                else if (timer < (game.press_time / 5) * 4)
                    score_multiplier = 4;
                else
                    score_multiplier = 5;
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (i != button_index)
                        buttons[i].gameObject.SetActive(false);
                }

                if (game.level.current_move == game.level.moves.Length - 1)
                {
                    perfect_feedbak.gameObject.SetActive(true);
                }
            }
            else
            {
                good_move = false;
                buttons[button_index].image.color = new Color(1, 0, 0);
                buttons[button_index].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (i != button_index && i != good_button)
                        buttons[i].gameObject.SetActive(false);
                }
                good_streak = false;
                UpdateStreak(-1);
                game.NegativeFeedback();
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
            level.streak = 1;
        }
        else
        {
            level.streak += streak;
        }
        streakTxt.text = "Combo: x" + level.streak.ToString();
    }

    public void UpdateTime(float dt)
    {
        timer -= dt;

        if (timer < game.press_time)
        {
            if (!buttons_visible)
            {
                buttons_visible = true;
                foreach (Button b in buttons)
                {
                    b.image.color = new Color(1, 1, 1);
                    b.gameObject.SetActive(true);
                    b.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                }
            }
        }
        if (timer < 0)
        {
            if (good_move)
            {
                game.level.score += (normal_score * score_multiplier * level.streak);
                UpdateScore();
                game.player_character.SetAnimation(current_move.animation);
            }
            else
            {
                good_streak = false;
                UpdateStreak(-1);
                game.player_character.Reset();
                if (!button_pressed)
                    game.NegativeFeedback();
            }
            score_multiplier = 1;
        }
    }

    public void SetMove(Level.Move new_move)
    {
        current_move = new_move;

        button_pressed = false;

        good_move = false;

        timer = time;

        good_button = UnityEngine.Random.Range(0, buttons.Length);
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == good_button)
                buttons[i].image.sprite = images[current_move.button - 1];
            else
            {
                bool different_image = false;
                do
                {
                    int image_index = UnityEngine.Random.Range(0, images.Length);
                    bool image_used = false;
                    if (i > 0)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if (images[image_index] == buttons[j].image.sprite)
                            {
                                image_used = true;
                                break;
                            }
                        }
                    }
                    if (!image_used)
                    {
                        if (image_index != current_move.button - 1)
                        {
                            buttons[i].image.sprite = images[image_index];
                            different_image = true;
                        }
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
        perfect_feedbak.gameObject.SetActive(false);
    }

    public void ShowModel(bool s)
    {
        foreach (Button b in model_buttons)
        {
            b.gameObject.SetActive(s);
        }

        text_wait.gameObject.SetActive(s);
        text_prepare.gameObject.SetActive(false);
        perfect_feedbak.gameObject.SetActive(false);
    }

    public void ShowStart(bool s)
    {
        playGame.gameObject.SetActive(s);
    }

    public void ShowAgain(bool s)
    {
        playAgain.gameObject.SetActive(s);
        perfect_feedbak.gameObject.SetActive(false);
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
        perfect_feedbak.gameObject.SetActive(false);
    }

    public void ShowFinish(bool s)
    {
        text_score.text = "Score: " + level.score;
        text_high_score.text = "High score: " + game.high_score;

        text_score.gameObject.SetActive(s);
        text_high_score.gameObject.SetActive(s);
        perfect_feedbak.gameObject.SetActive(false);
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
