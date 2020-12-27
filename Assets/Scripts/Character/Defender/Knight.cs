using UnityEngine;

public class Knight : MonoBehaviour
{
    Defender defenderScript;
    Health health;

    public float attackDistance = 0.125f;

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
            && Vector2.Distance(transform.position, defenderScript.targetAttacker.transform.position) <= attackDistance/*Mathf.Abs(defenderScript.attackOffset.x) + 0.05f*/)
        {
            // Debug.Log(name + " is attacking " + defenderScript.targetAttacker.name);
            defenderScript.Attack();
        }
    }
}
