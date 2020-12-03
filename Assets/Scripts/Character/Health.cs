using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] GameObject deathVFX;

    Attacker attacker;
    Defender defender;

    void Start()
    {
        TryGetComponent<Attacker>(out attacker);
        TryGetComponent<Defender>(out defender);
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            TriggerDeathVFX();

            if (attacker != null)
            {
                attacker.currentTargetsSquad.attackerCount--;
                attacker.currentTarget.GetComponent<Defender>().attackerBeingAttackedBy = null;
            }

            if (defender != null)
            {
                if (transform.parent == defender.squad.unitsParent) // Remove the defender from the units list
                    defender.squad.units.Remove(defender);
                else if (transform.parent == defender.squad.leaderParent)
                {
                    // The leader was killed, so retreat the remaining units in the squad
                    defender.squad.leader = null;
                    defender.squad.Retreat();
                }

                defender.attackerBeingAttackedBy.currentTarget = null;

                if (defender.squad.units.Count <= 0)
                    Destroy(defender.squad);
            }

            Destroy(gameObject);
        }
    }

    void TriggerDeathVFX()
    {
        if (deathVFX == false) return;

        GameObject deathVFXObject = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(deathVFXObject, 1.5f);
    }
}
