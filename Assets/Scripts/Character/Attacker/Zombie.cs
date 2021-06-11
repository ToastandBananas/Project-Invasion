using UnityEngine;

public class Zombie : Enemy
{
    [Range(0f, 1f)] public float reanimatePercentChance;
    public float minResurrectWaitTime = 4f;
    public float maxResurrectWaitTime = 10f;

    [HideInInspector] public bool hasBeenResurrected;

    [HideInInspector] public AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;
    }

    public override void Update()
    {
        if (health.isDead)
            TryReanimate();

        if (attackerScript.isAttacking == false)
        {
            if (attackerScript.currentTargetDefender != null && Vector2.Distance(transform.position, attackerScript.currentTargetDefender.transform.position) <= attackerScript.minAttackDistance)
            {
                attackerScript.Attack();
            }
            else if (attackerScript.currentTargetDefender != null && attackerScript.currentTargetDefender.isRetreating)
            {
                if (attackerScript.opponents.Contains(attackerScript.currentTargetDefender))
                    attackerScript.opponents.Remove(attackerScript.currentTargetDefender);

                attackerScript.GetNewTarget();
            }
        }
    }

    public void TryReanimate()
    {
        if (hasBeenResurrected == false)
        {
            float random = Random.Range(0f, 1f);
            if (random <= reanimatePercentChance)
            {
                hasBeenResurrected = true;
                StartCoroutine(attackerScript.health.Resurrect(Random.Range(minResurrectWaitTime, maxResurrectWaitTime), this));
            }
            else
                this.enabled = false;
        }
        else
            this.enabled = false;
    }

    public void PlayZombieBiteSound()
    {
        audioManager.PlayRandomSound(audioManager.zombieBiteSounds);
    }
}