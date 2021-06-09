using UnityEngine;

public class Ally : MonoBehaviour
{
    [HideInInspector] public Defender defenderScript;
    [HideInInspector] public SquadData squadData;

    public virtual void Start()
    {
        defenderScript = GetComponent<Defender>();
        squadData = GameManager.instance.squadData;
    }
    
    public virtual void FixedUpdate()
    {
        if (defenderScript.isAttacking == false && defenderScript.currentTargetAttacker != null
            && Vector2.Distance(transform.position, defenderScript.currentTargetAttacker.transform.position) <= defenderScript.minAttackDistance)
        {
            // Debug.Log(name + " is attacking " + defenderScript.targetAttacker.name);
            defenderScript.Attack();
        }
    }
}
