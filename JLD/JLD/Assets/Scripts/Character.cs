﻿using UnityEngine;
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
        for (int i = 1; i <= 5; i++)
            animator.SetBool("dance" + i.ToString(), false);
        if (animation == "dance5")
        {
            transform.rotation = Quaternion.Euler(0, 150, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
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
