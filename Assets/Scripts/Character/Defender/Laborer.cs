using UnityEngine;

public class Laborer : MonoBehaviour
{
    Defender defenderScript;
    Health health;
    SquadData squadData;

    void Start()
    {
        defenderScript = GetComponent<Defender>();
        health = GetComponent<Health>();
        squadData = GameManager.instance.squadData;

        SetLaborerData();
    }
    
    void Update()
    {
        if (health.isDead)
            this.enabled = false;

        // If not mining, walk towards a mining deposit and start mining

    }

    public void SetLaborerData()
    {
        if (squadData.laborerHealth > 0)
            health.SetMaxHealth(health.GetMaxHealth() + squadData.laborerHealth);

        health.SetCurrentHealthToMaxHealth();
    }
}
