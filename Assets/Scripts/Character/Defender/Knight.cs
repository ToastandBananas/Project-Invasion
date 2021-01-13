﻿using UnityEngine;

public class Knight : MonoBehaviour
{
    public int knightLevel = 1;

    Defender defenderScript;
    Health health;

    void Start()
    {
        defenderScript = GetComponent<Defender>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (health.isDead)
            this.enabled = false;

        if (defenderScript.isAttacking == false && defenderScript.targetAttacker != null
            && Vector2.Distance(transform.position, defenderScript.targetAttacker.transform.position) <= defenderScript.minAttackDistance)
        {
            // Debug.Log(name + " is attacking " + defenderScript.targetAttacker.name);
            defenderScript.Attack();
        }
    }
}
