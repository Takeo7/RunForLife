﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAnimation : MonoBehaviour {

    public Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("Activate");
    }
}
