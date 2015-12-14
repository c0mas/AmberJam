using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
* Main menu
**/

public class Menu : MonoBehaviour
{
    public Game game;
    public Button[] buttons;

    public Level.Move current_move;

	public void Init()
    {

    }

    public void PressButton(int button_index)
    {
        if (button_index != current_move.button)
        {
            //bad stuff
            return;
        }

        if (game.level.timer < (current_move.time - current_move.press_time))
        {
            //good_stuff
            game.player_character.SetAnimation(current_move.animation);
        }
    }

    public void UpdateTime(float dt)
    {
       
    }

    public void SetMove(Level.Move new_move)
    {
        current_move = new_move;
        //TODO reset button
    }
}
