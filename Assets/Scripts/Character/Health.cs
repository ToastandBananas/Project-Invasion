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
    SpriteRenderer spriteRenderer;
    Transform deadCharactersParent;

    void Start()
    {
        TryGetComponent<Attacker>(out attacker);
        TryGetComponent<Defender>(out defender);
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        deadCharactersParent = GameObject.Find("Dead Characters").transform;
    }

    public void DealDamage(float damage)
    {
        health -= damage;

        // If the character is going to die
        if (health <= 0 && isDead == false)
        {
            // TriggerDeathVFX();
            
            FindNewTargetForOpponent();

            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        boxCollider.enabled = false;
        spriteRenderer.sortingOrder = 4;

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

    public void FindNewTargetForOpponent()
    {
        // If this is an attacker who is dying
        if (attacker != null)
        {
            // Find a new target for each defender who was fighting the attacker who died:
            FindNewTargetForDefenders();
        }

        // If this is a defender who is dying
        if (defender != null)
        {
            // Find a new target for each attacker who was fighting the defender who died
            FindNewTargetForAttackers();

            // If there are no more units in the squad and the leader is dead, get rid of the Squad GameObject & free up the space so we can spawn in a new squad
            if (defender.squad != null && defender.squad.units.Count == 0 && defender.squad.leader == null)
            {
                DefenderSpawner.instance.RemoveCell(defender.squad.transform.position);
                Destroy(defender.squad.gameObject);
            }
        }
    }

    void FindNewTargetForDefenders()
    {
        if (attacker.currentDefenderAttacking != null)
        {
            // Remove the dead attacker from the attackersInRange list
            if (attacker.currentDefenderAttacking.squad.attackersInRange.Contains(attacker))
                attacker.currentDefenderAttacking.squad.attackersInRange.Remove(attacker);

            foreach (Defender opponent in attacker.opponents)
            {
                opponent.targetAttacker = null;
                opponent.currentTargetsHealth = null;
                opponent.StopAttacking();
            }

            // Find new target if possible (for the defender that killed this attacker)
            if (attacker.currentDefenderAttacking.squad.attackersInRange.Count > 0)
            {
                foreach (Defender opponent in attacker.opponents)
                {
                    FindNewRandomTargetForDefender(opponent);
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
                // Remove the dead attacker from the attackersInRange list
                if (opponent.squad.attackersInRange.Contains(attacker))
                    opponent.squad.attackersInRange.Remove(attacker);

                opponent.targetAttacker = null;
                opponent.currentTargetsHealth = null;
                opponent.StopAttacking();
            }

            foreach (Defender opponent in attacker.opponents)
            {
                // Find a new target for each defender that was fighting the attacker that died
                if (opponent.squad.attackersInRange.Count > 0)
                    FindNewRandomTargetForDefender(opponent);
                else
                {
                    opponent.squad.AssignUnitPositions();
                    opponent.squad.AssignLeaderPosition();
                    return;
                }
            }
        }
    }

    void FindNewRandomTargetForDefender(Defender theDefender)
    {
        int randomTargetIndex = Random.Range(0, theDefender.squad.attackersInRange.Count);
        if (randomTargetIndex >= theDefender.squad.attackersInRange.Count && randomTargetIndex > 0)
            randomTargetIndex = theDefender.squad.attackersInRange.Count - 1;

        if (theDefender.squad.attackersInRange[randomTargetIndex].opponents.Contains(theDefender) == false)
        {
            theDefender.targetAttacker = theDefender.squad.attackersInRange[randomTargetIndex];
            theDefender.currentTargetsHealth = theDefender.squad.attackersInRange[randomTargetIndex].health;
            theDefender.squad.attackersInRange[randomTargetIndex].opponents.Add(theDefender);
        }
    }

    void FindNewTargetForAttackers()
    {
        if (transform.parent == defender.squad.unitsParent) // Remove the defender from the units list
            defender.squad.units.Remove(defender);
        else if (transform.parent == defender.squad.leaderParent)
        {
            // The leader was killed, so retreat the remaining units in the squad
            defender.squad.leader = null;
            defender.squad.Retreat();
        }

        foreach (Attacker attacker in defender.squad.attackersInRange) // For each attacker in range of the defender who died...
        {
            Defender theCurrentDefenderBeingAttacked = attacker.currentDefenderAttacking;

            if (attacker.opponents.Contains(defender))
                attacker.opponents.Remove(defender);

            // Remove any opponents missing in the list (will happen if another attacker kills the defender)
            for (int i = 0; i < attacker.opponents.Count; i++)
            {
                if (attacker.opponents[i] == null)
                    attacker.opponents.Remove(attacker.opponents[i]);
            }

            if (theCurrentDefenderBeingAttacked == defender || theCurrentDefenderBeingAttacked == null) // If the attacker was fighting the defender who died (or if it became null somehow)
            {
                attacker.currentDefenderAttacking = null;
                attacker.currentTargetsHealth = null;
                attacker.isAttacking = false;

                if (attacker.opponents.Count > 0) // If the attacker already has another defender (opponent) attacking him
                {
                    attacker.currentDefenderAttacking = attacker.opponents[0];
                    attacker.currentTargetsHealth = attacker.opponents[0].health;
                    if (Vector2.Distance(attacker.transform.position, attacker.opponents[0].transform.position) > attacker.minAttackDistance)
                        attacker.StopAttacking();
                }
                else if (defender.squad.units.Count > 0 || defender.squad.leader != null) // If there's still defenders in the squad
                {
                    // Find a new target if possible (for the attacker who killed this defender)
                    int randomTargetIndex = Random.Range(0, defender.squad.units.Count);
                    if (defender.squad.leader != null) randomTargetIndex++;

                    if ((randomTargetIndex == defender.squad.units.Count || defender.squad.units.Count == 0) && attacker.opponents.Contains(defender.squad.leader) == false)
                    {
                        // Attack the squad's leader
                        attacker.currentDefenderAttacking = defender.squad.leader;
                        attacker.currentTargetsHealth = defender.squad.leader.health;
                        attacker.opponents.Add(defender.squad.leader);

                        defender.squad.leader.targetAttacker = attacker;
                        defender.squad.leader.currentTargetsHealth = attacker.health;

                        if (Vector2.Distance(attacker.transform.position, defender.squad.leader.transform.position) > attacker.minAttackDistance)
                            attacker.StopAttacking();
                    }
                    else if (attacker.opponents.Contains(defender.squad.units[randomTargetIndex]) == false)
                    {
                        // Attack a random unit in the squad
                        attacker.currentDefenderAttacking = defender.squad.units[randomTargetIndex];
                        attacker.currentTargetsHealth = defender.squad.units[randomTargetIndex].health;
                        attacker.opponents.Add(defender.squad.units[randomTargetIndex]);

                        defender.squad.units[randomTargetIndex].targetAttacker = defender.targetAttacker;
                        defender.squad.units[randomTargetIndex].currentTargetsHealth = defender.targetAttacker.health;

                        if (Vector2.Distance(attacker.transform.position, defender.squad.units[randomTargetIndex].transform.position) > attacker.minAttackDistance)
                            attacker.StopAttacking();
                    }
                }
            }
            
            // Send in another defender if the attacker can fight multiple opponents
            if (theCurrentDefenderBeingAttacked == defender && attacker.maxOpponents > attacker.opponents.Count)
            {
                foreach (Defender potentialOpponent in defender.squad.units)
                {
                    if (potentialOpponent.targetAttacker == null && attacker.opponents.Contains(potentialOpponent) == false)
                    {
                        potentialOpponent.targetAttacker = attacker;
                        attacker.opponents.Add(potentialOpponent);
                        if (attacker.maxOpponents == attacker.opponents.Count)
                            return;
                    }
                }

                if (attacker.maxOpponents > attacker.opponents.Count && defender.squad.leader != null && attacker.opponents.Contains(defender.squad.leader) == false)
                {
                    defender.squad.leader.targetAttacker = attacker;
                    attacker.opponents.Add(defender.squad.leader);
                }
            }
        }
    }
}
