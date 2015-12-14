using UnityEngine;
using System.Collections;

/*
* Class that controls the character
**/

public class Character : MonoBehaviour
{
    public Game game;


    public void SetAnimation(string animation)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        for (int i = 0; i < 5; i++)
            animator.SetBool("dance" + i.ToString(), false);
        animator.SetBool(animation, true);
    }

    public void Reset()
    {
        Animator animator = gameObject.GetComponent<Animator>();
        for (int i = 1; i <= 5; i++)
            animator.SetBool("dance" + i.ToString(), false);
    }

    public float GetMoveTime(int move)
    {
        Animator animator = gameObject.GetComponent<Animator>();
        return animator.runtimeAnimatorController.animationClips[move].length;
    }
}
