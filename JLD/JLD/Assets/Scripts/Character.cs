using UnityEngine;
using System.Collections;

/*
* Class that controls the character
**/

public class Character : MonoBehaviour
{
    public Game game;

    public string last_animation = "";

    public void SetAnimation(string animation)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        if (last_animation != "")
            animator.SetBool(last_animation, false);
        animator.SetBool(animation, true);
        last_animation = animation;
    }

    public void Reset()
    {
        Animator animator = gameObject.GetComponent<Animator>();
        if (last_animation != "")
            animator.SetBool(last_animation, false);
        last_animation = "";
    }

    public float GetMoveTime(int move)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        return animator.runtimeAnimatorController.animationClips[move].length;
    }
}
