using UnityEngine;

public class Archer : MonoBehaviour
{
    public int archerLevel = 1;

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

        //////////
        // TODO: Switch to melee weapons when enemies in range and attack, else switch back to bow (if level 2 archer)
        //////////

        /*if (defenderScript.isAttacking == false && defenderScript.targetAttacker != null
            && Vector2.Distance(transform.position, defenderScript.targetAttacker.transform.position) <= defenderScript.minAttackDistance)
        {
            // Debug.Log(name + " is attacking " + defenderScript.targetAttacker.name);
            defenderScript.Attack();
        }*/
    }
}
