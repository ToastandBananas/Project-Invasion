using UnityEngine;

public class SquadData : MonoBehaviour
{
    [Header("Knight Data")]
    public int knightSquadGoldCost;
    public float knightHealth, knightLeaderHealth, knightMeleeDamage, knightLeaderMeleeDamage;

    [Header("Spearmen Data")]
    public int spearmenSquadGoldCost;
    public float spearmenHealth, spearmenLeaderHealth, spearmenMeleeDamage, spearmenLeaderMeleeDamage, spearmenRangedDamage, spearmenLeaderRangedDamage, spearmenAccuracy, spearmenLeaderAccuracy;

    [Header("Archer Data")]
    public int archerSquadGoldCost;
    public float archerHealth, archerLeaderHealth, archerMeleeDamage, archerLeaderMeleeDamage, archerRangedDamage, archerLeaderRangedDamage, archerAccuracy, archerLeaderAccuracy;
    public bool archerShouldRetreatWhenEnemyNear, archerFireArrowsUnlocked;

    public void ApplyKnightData(int gold, float health, float leaderHealth, float damage, float leaderDamage)
    {
        knightSquadGoldCost += gold;
        knightHealth += health;
        knightLeaderHealth += leaderHealth;
        knightMeleeDamage += damage;
        knightLeaderMeleeDamage += leaderDamage;
    }

    public void ApplySpearmenData(int gold, float health, float leaderHealth, float meleeDamage, float leaderMeleeDamage, float rangedDamage, float leaderRangedDamage, float accuracy, float leaderAccuracy)
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

    public void ApplySpearmenData(int gold, float health, float leaderHealth, float meleeDamage, float leaderMeleeDamage, float rangedDamage, float leaderRangedDamage, float accuracy, float leaderAccuracy, bool shouldRetreat, bool fireArrowsUnlocked)
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

    public void SaveSquadData()
    {
        // Knight Data
        ES3.Save("knightSquadGoldCost", knightSquadGoldCost);
        ES3.Save("knightHealth", knightHealth);
        ES3.Save("knightLeaderHealth", knightLeaderHealth);
        ES3.Save("knightMeleeDamage", knightMeleeDamage);
        ES3.Save("knightLeaderMeleeDamage", knightLeaderMeleeDamage);

        // Spearmen Data
        ES3.Save("spearmenSquadGoldCost", spearmenSquadGoldCost);
        ES3.Save("spearmenHealth", spearmenHealth);
        ES3.Save("spearmenLeaderHealth", spearmenLeaderHealth);
        ES3.Save("spearmenMeleeDamage", spearmenMeleeDamage);
        ES3.Save("spearmenLeaderMeleeDamage", spearmenLeaderMeleeDamage);
        ES3.Save("spearmenRangedDamage", spearmenRangedDamage);
        ES3.Save("spearmenLeaderRangedDamage", spearmenLeaderRangedDamage);
        ES3.Save("spearmenAccuracy", spearmenAccuracy);
        ES3.Save("spearmenLeaderAccuracy", spearmenLeaderAccuracy);

        // Archer Data
        ES3.Save("archerSquadGoldCost", archerSquadGoldCost);
        ES3.Save("archerHealth", archerHealth);
        ES3.Save("archerLeaderHealth", archerLeaderHealth);
        ES3.Save("archerMeleeDamage", archerMeleeDamage);
        ES3.Save("archerLeaderMeleeDamage", archerLeaderMeleeDamage);
        ES3.Save("archerRangedDamage", archerRangedDamage);
        ES3.Save("archerLeaderRangedDamage", archerLeaderRangedDamage);
        ES3.Save("archerAccuracy", archerAccuracy);
        ES3.Save("archerLeaderAccuracy", archerLeaderAccuracy);
        ES3.Save("archerShouldRetreatWhenEnemyNear", archerShouldRetreatWhenEnemyNear);
        ES3.Save("archerFireArrowsUnlocked", archerFireArrowsUnlocked);
    }

    public void LoadSquadData()
    {
        // Knight Data
        knightSquadGoldCost = ES3.Load("knightSquadGoldCost", 0);
        knightHealth = ES3.Load("knightHealth", 0f);
        knightLeaderHealth = ES3.Load("knightLeaderHealth", 0f);
        knightMeleeDamage = ES3.Load("knightMeleeDamage", 0f);
        knightLeaderMeleeDamage = ES3.Load("knightLeaderMeleeDamage", 0f);

        // Spearmen Data
        spearmenSquadGoldCost = ES3.Load("spearmenSquadGoldCost", 0);
        spearmenHealth = ES3.Load("spearmenHealth", 0f);
        spearmenLeaderHealth = ES3.Load("spearmenLeaderHealth", 0f);
        spearmenMeleeDamage = ES3.Load("spearmenMeleeDamage", 0f);
        spearmenLeaderMeleeDamage = ES3.Load("spearmenLeaderMeleeDamage", 0f);
        spearmenRangedDamage = ES3.Load("spearmenRangedDamage", 0f);
        spearmenLeaderRangedDamage = ES3.Load("spearmenLeaderRangedDamage", 0f);
        spearmenAccuracy = ES3.Load("spearmenAccuracy", 0f);
        spearmenLeaderAccuracy = ES3.Load("spearmenLeaderAccuracy", 0f);

        // Archer Data
        archerSquadGoldCost = ES3.Load("archerSquadGoldCost", 0);
        archerHealth = ES3.Load("archerHealth", 0f);
        archerLeaderHealth = ES3.Load("archerLeaderHealth", 0f);
        archerMeleeDamage = ES3.Load("archerMeleeDamage", 0f);
        archerLeaderMeleeDamage = ES3.Load("archerLeaderMeleeDamage", 0f);
        archerRangedDamage = ES3.Load("archerRangedDamage", 0f);
        archerLeaderRangedDamage = ES3.Load("archerLeaderRangedDamage", 0f);
        archerAccuracy = ES3.Load("archerAccuracy", 0f);
        archerLeaderAccuracy = ES3.Load("archerLeaderAccuracy", 0f);
        archerShouldRetreatWhenEnemyNear = ES3.Load("archerShouldRetreatWhenEnemyNear", true);
        archerFireArrowsUnlocked = ES3.Load("archerFireArrowsUnlocked", false);
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