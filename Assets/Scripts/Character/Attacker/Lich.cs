using UnityEngine;

public class Lich : MonoBehaviour
{
    Attacker attackerScript;
    Health health;

    void Start()
    {
        attackerScript = GetComponent<Attacker>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        if (health.isDead)
            this.enabled = false;

        /*if (attackerScript.isAttacking == false && attackerScript.currentDefenderAttacking != null
            && Vector2.Distance(transform.position, attackerScript.currentDefenderAttacking.transform.position) <= attackerScript.minAttackDistance)
        {
            // Debug.Log(name + " is attacking " + defenderScript.targetAttacker.name);
            attackerScript.Attack();
        }*/
    }

    public void RaiseUndead()
    {

    }

    public void ShootFireball()
    {
        // Put this in own script? AttackerShooter?
    }
}
