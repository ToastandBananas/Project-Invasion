using UnityEngine;

public class Laborer : MonoBehaviour
{
    [HideInInspector] public ResourceDeposit targetResourceDeposit;

    [HideInInspector] public bool isWorking = false;

    AudioManager audioManager;
    Defender defenderScript;
    Health health;
    SquadData squadData;

    int hitsSinceResourceProduced;

    void Start()
    {
        audioManager = AudioManager.instance;
        defenderScript = GetComponent<Defender>();
        health = GetComponent<Health>();
        squadData = GameManager.instance.squadData;

        SetLaborerData();
    }
    
    void Update()
    {
        if (health.isDead)
        {
            StopWorking();
            this.enabled = false;
        }

        // If the Laborer is not mining, but is close enough to his deposit, then start mining
        if (isWorking == false && targetResourceDeposit != null && Vector2.Distance(transform.localPosition, defenderScript.unitPosition) <= defenderScript.minDistanceFromTargetPosition)
        {
            isWorking = true;
            defenderScript.anim.SetBool("isMining", true);
            
            defenderScript.FaceTarget(targetResourceDeposit.transform.position);
        }
    }

    public void SetLaborerData()
    {
        if (squadData.laborerHealth > 0)
            health.SetMaxHealth(health.GetMaxHealth() + squadData.laborerHealth);

        health.SetCurrentHealthToMaxHealth();

        defenderScript.anim.SetFloat("miningSpeed", squadData.laborerMiningSpeedMultiplier);
    }

    public void StopWorking()
    {
        if (targetResourceDeposit != null)
        {
            defenderScript.anim.SetBool("isMining", false);
            targetResourceDeposit.occupied = false;
            targetResourceDeposit.resourceNode.unoccupiedResourceDeposits.Add(targetResourceDeposit);
            targetResourceDeposit = null;
        }

        isWorking = false;
    }

    public void IncreaseHitsSinceLastResourceProduction()
    {
        audioManager.PlayRandomSound(audioManager.pickaxeSounds);

        hitsSinceResourceProduced++;
        if (targetResourceDeposit != null && hitsSinceResourceProduced == targetResourceDeposit.hitsToProduce)
        {
            hitsSinceResourceProduced = 0;
            targetResourceDeposit.ProduceResources();
            TextPopup.CreateResourceGainPopup(transform.position + new Vector3(0f, 0.1f), targetResourceDeposit.goldEarnedEachProductionCycle, targetResourceDeposit.resourceType);
        }
    }
}
