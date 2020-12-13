using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] GameObject deathVFX;
    public bool isDead = false;

    Attacker attacker;
    Defender defender;
    Animator anim;
    BoxCollider2D boxCollider;
    Transform deadCharactersParent;

    void Start()
    {
        TryGetComponent<Attacker>(out attacker);
        TryGetComponent<Defender>(out defender);
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        deadCharactersParent = GameObject.Find("Dead Characters").transform;
    }

    public void DealDamage(float damage)
    {
        health -= damage;

        // If the character is going to die
        if (health <= 0 && isDead == false)
        {
            TriggerDeathVFX();

            FindNewTargetForOpponent();

            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        boxCollider.enabled = false;

        float randomRotation = Random.Range(-70f, 70f);
        transform.eulerAngles = new Vector3(0, 0, randomRotation);
        transform.SetParent(deadCharactersParent);
    }

    void TriggerDeathVFX()
    {
        if (deathVFX == false) return;

        GameObject deathVFXObject = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(deathVFXObject, 1.5f);
    }

    void FindNewTargetForOpponent()
    {
        // If this is an attacker who is dying
        if (attacker != null)
        {
            attacker.currentTargetsSquad.attackerCount--;
            if (attacker.currentDefenderAttacking != null)
            {
                if (attacker.currentDefenderAttacking.squad.attackersInRange.Contains(attacker))
                    attacker.currentDefenderAttacking.squad.attackersInRange.Remove(attacker);

                foreach (Defender opponent in attacker.opponents)
                {
                    opponent.targetAttacker = null;
                    opponent.currentTargetsHealth = null;
                    opponent.isAttacking = false;
                }

                // Find new target if possible (for the defender that killed this attacker)
                if (attacker.currentDefenderAttacking.squad.attackersInRange.Count > 0)
                {
                    foreach (Defender opponent in attacker.opponents)
                    {
                        int randomTargetIndex = Random.Range(0, opponent.squad.attackersInRange.Count);
                        opponent.targetAttacker = opponent.squad.attackersInRange[randomTargetIndex];
                        opponent.currentTargetsHealth = opponent.squad.attackersInRange[randomTargetIndex].health;
                        opponent.squad.attackersInRange[randomTargetIndex].opponents.Add(opponent);
                    }
                }
                else // If there are no more attackers nearby, reorganize the squad
                {
                    attacker.currentDefenderAttacking.squad.AssignUnitPositions();
                    attacker.currentDefenderAttacking.squad.AssignLeaderPosition();
                }
            }
            else
            {
                foreach (Defender opponent in attacker.opponents)
                {
                    opponent.targetAttacker = null;
                    opponent.currentTargetsHealth = null;
                    opponent.isAttacking = false;
                }
            }
        }

        // If this is a defender who is dying
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

            if (defender.targetAttacker != null)
            {
                defender.targetAttacker.currentTarget = null;
                defender.targetAttacker.currentDefenderAttacking = null;
                defender.targetAttacker.currentTargetsHealth = null;
                defender.targetAttacker.isAttacking = false;

                if (defender.targetAttacker.opponents.Contains(defender))
                    defender.targetAttacker.opponents.Remove(defender);

                // Remove any opponents missing in the list (will happen if another attacker kills the defender)
                for (int i = 0; i < defender.targetAttacker.opponents.Count; i++)
                {
                    if (defender.targetAttacker.opponents[i] == null)
                        defender.targetAttacker.opponents.Remove(defender.targetAttacker.opponents[i]);
                }

                if (defender.targetAttacker.opponents.Count > 0) // If the attacker already has another defender (opponent) attacking him
                {
                    defender.targetAttacker.currentTarget = defender.targetAttacker.opponents[0].gameObject;
                    defender.targetAttacker.currentDefenderAttacking = defender.targetAttacker.opponents[0];
                    defender.targetAttacker.currentTargetsHealth = defender.targetAttacker.opponents[0].health;
                }
                else if (defender.squad.units.Count > 0 || defender.squad.leader != null)
                {
                    // Find a new target if possible (for the attacker who killed this defender)
                    int randomTargetIndex = Random.Range(0, defender.squad.units.Count);
                    if (defender.squad.leader != null) randomTargetIndex++;

                    if (randomTargetIndex == defender.squad.units.Count || defender.squad.units.Count == 0)
                    {
                        // Attack the squad's leader
                        defender.targetAttacker.currentTarget = defender.squad.leader.gameObject;
                        defender.targetAttacker.currentDefenderAttacking = defender.squad.leader;
                        defender.targetAttacker.currentTargetsHealth = defender.squad.leader.health;
                        defender.targetAttacker.opponents.Add(defender.squad.leader);

                        defender.squad.leader.targetAttacker = defender.targetAttacker;
                        defender.squad.leader.currentTargetsHealth = defender.targetAttacker.health;
                    }
                    else
                    {
                        // Attack a random unit in the squad
                        defender.targetAttacker.currentTarget = defender.squad.units[randomTargetIndex].gameObject;
                        defender.targetAttacker.currentDefenderAttacking = defender.squad.units[randomTargetIndex];
                        defender.targetAttacker.currentTargetsHealth = defender.squad.units[randomTargetIndex].health;
                        defender.targetAttacker.opponents.Add(defender.squad.units[randomTargetIndex]);

                        defender.squad.units[randomTargetIndex].targetAttacker = defender.targetAttacker;
                        defender.squad.units[randomTargetIndex].currentTargetsHealth = defender.targetAttacker.health;
                    }
                }
            }

            // If there are no more units in the squad and the leader is dead, get rid of the Squad GameObject
            if (defender.squad.units.Count == 0 && defender.squad.leader == null)
            {
                DefenderSpawner.instance.RemoveCell(defender.squad.transform.position);
                Destroy(defender.squad.gameObject);
            }
        }
    }
}
