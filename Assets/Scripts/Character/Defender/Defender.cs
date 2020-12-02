using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] int goldCost = 100;

    GameObject currentTarget;
    Health currentTargetsHealth;

    CurrencyDisplay currencyDisplay;
    Animator anim;

    void Start()
    {
        currencyDisplay = FindObjectOfType<CurrencyDisplay>();
        anim = GetComponent<Animator>();
    }

    public void AddGold(int amount)
    {
        currencyDisplay.AddGold(amount);
    }

    public int GetGoldCost()
    {
        return goldCost;
    }

    public void Attack(GameObject target)
    {
        anim.SetBool("isAttacking", true);
        currentTarget = target;
        currentTargetsHealth = currentTarget.GetComponent<Health>();
    }

    public void StrikeCurrentTarget(float damage)
    {
        if (currentTarget == null) return;

        if (currentTargetsHealth)
            currentTargetsHealth.DealDamage(damage);
    }
}
