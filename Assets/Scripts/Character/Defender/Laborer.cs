using UnityEngine;

public class Laborer : MonoBehaviour
{
    public GoldDeposit targetGoldDeposit;

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
        if (isWorking == false && targetGoldDeposit != null && Vector2.Distance(transform.localPosition, defenderScript.unitPosition) <= defenderScript.minDistanceFromPosition)
        {
            isWorking = true;
            defenderScript.anim.SetBool("isMining", true);
            
            defenderScript.FaceTarget(targetGoldDeposit.transform.position);
        }
    }

    public void SetLaborerData()
    {
        if (squadData.laborerHealth > 0)
            health.SetMaxHealth(health.GetMaxHealth() + squadData.laborerHealth);

        health.SetCurrentHealthToMaxHealth();
    }

    public void StopWorking()
    {
        if (targetGoldDeposit != null)
        {
            defenderScript.anim.SetBool("isMining", false);
            targetGoldDeposit.occupied = false;
            targetGoldDeposit.resourceNode.unoccupiedDeposits.Add(targetGoldDeposit);
            targetGoldDeposit = null;
        }

        isWorking = false;
    }

    public void IncreaseHitsSinceLastResourceProduction()
    {
        audioManager.PlayRandomSound(audioManager.pickaxeSounds);

        hitsSinceResourceProduced++;
        if (targetGoldDeposit != null && hitsSinceResourceProduced == targetGoldDeposit.hitsToProduceGold)
        {
            hitsSinceResourceProduced = 0;
            targetGoldDeposit.ProduceGold();
        }
    }
}
