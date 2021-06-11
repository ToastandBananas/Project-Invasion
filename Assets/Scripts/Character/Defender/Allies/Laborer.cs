using UnityEngine;

public class Laborer : Ally
{
    [HideInInspector] public ResourceDeposit targetResourceDeposit;

    [HideInInspector] public bool isWorking = false;

    AudioManager audioManager;

    int hitsSinceResourceProduced;

    public override void Start()
    {
        base.Start();

        audioManager = AudioManager.instance;

        SetLaborerData();
    }
    
    public override void FixedUpdate()
    {
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
            defenderScript.health.SetMaxHealth(defenderScript.health.GetMaxHealth() + squadData.laborerHealth);

        defenderScript.health.SetCurrentHealthToMaxHealth();

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
