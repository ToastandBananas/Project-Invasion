using UnityEngine;

public class SquadData : MonoBehaviour
{
    [Header("Knight Data")]
    public static int knightSquadGoldCost;
    public static float knightHealth, knightLeaderHealth, knightMeleeDamage, knightLeaderMeleeDamage;

    [Header("Spearmen Data")]
    public static int spearmenSquadGoldCost;
    public static float spearmenHealth, spearmenLeaderHealth, spearmenMeleeDamage, spearmenLeaderMeleeDamage, spearmenRangedDamage, spearmenLeaderRangedDamage, spearmenAccuracy, spearmenLeaderAccuracy;

    [Header("Archer Data")]
    public static int archerSquadGoldCost;
    public static float archerHealth, archerLeaderHealth, archerMeleeDamage, archerLeaderMeleeDamage, archerRangedDamage, archerLeaderRangedDamage, archerAccuracy, archerLeaderAccuracy;
    public static bool archerShouldRetreatWhenEnemyNear, archerFireArrowsUnlocked;

    public static void ApplyKnightData(int gold, float health, float leaderHealth, float damage, float leaderDamage)
    {
        knightSquadGoldCost += gold;
        knightHealth += health;
        knightLeaderHealth += leaderHealth;
        knightMeleeDamage += damage;
        knightLeaderMeleeDamage += leaderDamage;
    }

    public static void ApplySpearmenData(int gold, float health, float leaderHealth, float meleeDamage, float leaderMeleeDamage, float rangedDamage, float leaderRangedDamage, float accuracy, float leaderAccuracy)
    {
        spearmenSquadGoldCost += gold;
        spearmenHealth += health;
        spearmenLeaderHealth += leaderHealth;
        spearmenMeleeDamage += meleeDamage;
        spearmenLeaderMeleeDamage += leaderMeleeDamage;
        spearmenRangedDamage += rangedDamage;
        spearmenLeaderRangedDamage += leaderRangedDamage;
        spearmenAccuracy += accuracy;
        spearmenLeaderAccuracy += leaderAccuracy;
    }

    public static void ApplySpearmenData(int gold, float health, float leaderHealth, float meleeDamage, float leaderMeleeDamage, float rangedDamage, float leaderRangedDamage, float accuracy, float leaderAccuracy, bool shouldRetreat, bool fireArrowsUnlocked)
    {
        archerSquadGoldCost += gold;
        archerHealth += health;
        archerLeaderHealth += leaderHealth;
        archerMeleeDamage += meleeDamage;
        archerLeaderMeleeDamage += leaderMeleeDamage;
        archerRangedDamage += rangedDamage;
        archerLeaderRangedDamage += leaderRangedDamage;
        archerAccuracy += accuracy;
        archerLeaderAccuracy += leaderAccuracy;

        if (shouldRetreat == false)
            archerShouldRetreatWhenEnemyNear = false;

        if (fireArrowsUnlocked)
            archerFireArrowsUnlocked = true;
    }
}
/*
const string KNIGHT_HEALTH_KEY = "Knight Health";
const string KNIGHT_LEADER_HEALTH_KEY = "Knight Leader Health";

const string KNIGHT_DAMAGE_KEY = "Knight Damage";
const string KNIGHT_LEADER_DAMAGE_KEY = "Knight Leader Damage";

const string KNIGHT_ACCURACY_KEY = "Knight Accuracy";
const string KNIGHT_LEADER_ACCURACY_KEY = "Knight Leader Accuracy";

const string KNIGHT_GOLD_COST_KEY = "Knight Gold Cost";

#region Knight Health
public static void SetKnightHealth(float newHealth)
{
    PlayerPrefs.SetFloat(KNIGHT_HEALTH_KEY, newHealth);
}

public static float GetKnightHealth()
{
    return PlayerPrefs.GetFloat(KNIGHT_HEALTH_KEY);
}

public static void SetKnightLeaderHealth(float newHealth)
{
    PlayerPrefs.SetFloat(KNIGHT_LEADER_HEALTH_KEY, newHealth);
}

public static float GetKnightLeaderHealth()
{
    return PlayerPrefs.GetFloat(KNIGHT_LEADER_HEALTH_KEY);
}
#endregion

#region Knight Damage
public static void SetKnightDamage(float newDamage)
{
    PlayerPrefs.SetFloat(KNIGHT_HEALTH_KEY, newDamage);
}

public static float GetKnightDamage()
{
    return PlayerPrefs.GetFloat(KNIGHT_DAMAGE_KEY);
}

public static void SetKnightLeaderDamage(float newDamage)
{
    PlayerPrefs.SetFloat(KNIGHT_LEADER_HEALTH_KEY, newDamage);
}

public static float GetKnightLeaderDamage()
{
    return PlayerPrefs.GetFloat(KNIGHT_LEADER_DAMAGE_KEY);
}
#endregion

#region Knight Accuracy
public static void SetKnightAccuracy(float newAccuracy)
{
    PlayerPrefs.SetFloat(KNIGHT_ACCURACY_KEY, newAccuracy);
}

public static float GetKnightAccuracy()
{
    return PlayerPrefs.GetFloat(KNIGHT_ACCURACY_KEY);
}

public static void SetKnightLeaderAccuracy(float newAccuracy)
{
    PlayerPrefs.SetFloat(KNIGHT_LEADER_ACCURACY_KEY, newAccuracy);
}

public static float GetKnightLeaderAccuracy()
{
    return PlayerPrefs.GetFloat(KNIGHT_LEADER_ACCURACY_KEY);
}
#endregion

#region Knight Gold Cost
public static void SetKnightGoldCost(int newGoldCost)
{
    PlayerPrefs.SetInt(KNIGHT_GOLD_COST_KEY, newGoldCost);
}

public static float GetKnightGoldCost()
{
    return PlayerPrefs.GetInt(KNIGHT_GOLD_COST_KEY);
}
#endregion
*/