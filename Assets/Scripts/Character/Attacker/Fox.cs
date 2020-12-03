﻿using UnityEngine;

public class Fox : MonoBehaviour
{
    Attacker attackerScript;
    Animator anim;

    void Start()
    {
        attackerScript = GetComponent<Attacker>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (attackerScript.isAttacking == false && attackerScript.currentTarget != null
            && Vector2.Distance(transform.position, attackerScript.currentTarget.transform.position) <= attackerScript.attackOffset.x)
        {
            attackerScript.Attack();
        }
    }

    /*void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherObject = collision.gameObject;

        // If this is a gravestone, jump over it
        if (otherObject.GetComponent<Gravestone>())
            anim.SetTrigger("jumpTrigger");
        else if (otherObject.GetComponent<Defender>() && collision.gameObject == attackerScript.currentTarget)
            attackerScript.Attack();
    }*/
}
