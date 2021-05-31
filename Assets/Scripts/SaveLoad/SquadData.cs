﻿using UnityEngine;

public class SquadData : MonoBehaviour
{
    string squadDataSavePath = "SquadData.es3";

    [Header("Laborer Data")]
    public int laborerGoldCost;
    public int laborerSuppliesCost;
    public float laborerHealth, laborerMiningSpeedMultiplier;
    public float laborerDoubleTimeTime;
    public bool laborerDoubleTimeUnlocked;

    [Header("Knight Data")]
    public int knightSquadGoldCost;
    public int knightSquadSuppliesCost;
    public float knightHealth, knightLeaderHealth;
    public float knightSlashDamage, knightLeaderSlashDamage;
    public float knightInspireMultiplier, knightInspireTime, knightThornsDamageMultiplier, knightThornsTime;
    public bool knightInspireUnlocked, knightThornsUnlocked;

    [Header("Spearmen Data")]
    public int spearmenSquadGoldCost;
    public int spearmenSquadSuppliesCost;
    public float spearmenHealth, spearmenLeaderHealth, spearmenAccuracy, spearmenLeaderAccuracy;
    public float spearmenPiercingDamage, spearmenRangedPiercingDamage, spearmenLeaderPiercingDamage, spearmenLeaderRangedPiercingDamage;
    public float spearmenLongThrowTime, spearmenSpearWallTime;
    public bool spearmenLongThrowUnlocked, spearmenSpearWallUnlocked;

    [Header("Archer Data")]
    public int archerSquadGoldCost;
    public int archerSquadSuppliesCost;
    public float archerHealth, archerLeaderHealth, archerAccuracy, archerLeaderAccuracy;
    public float archerPiercingDamage, archerRangedPiercingDamage, archerLeaderPiercingDamage, archerLeaderRangedPiercingDamage;
    public float archerFireArrowsTime, archerFireArrowsDamageMultiplier, archerRapidFireTime, archerRapidFireSpeedMultipilier;
    public bool archerShouldRetreatWhenEnemyNear, archerFireArrowsUnlocked, archerRapidFireUnlocked;

    [Header("Defaults")]
    // Archers
    public float defaultFireArrowTime = 30f;
    public float defaultRapidFireTime = 20f;
    public float defaultRapidFireSpeedMultiplier = 2f;

    // Knights
    public float defaultInspireMultiplier = 0.25f;
    public float defaultInspireTime = 60f;
    public float defaultThornsDamageMultiplier = 0.2f;
    public float defaultThornsTime = 30f;

    // Laborers
    public float defaultMiningSpeedMultiplier = 1f;
    public float defaultDoubleTimeTime = 45f;

    // Spearmen
    public float defaultLongThrowTime = 30f;
    public float defaultSpearWallTime = 30f;

    [Header("Ability Costs")]
    // Archers
    public int fireArrowsGoldCost = 75;
    public int fireArrowsSuppliesCost = 7;
    public int rapidFireGoldCost = 100;
    public int rapidFireSuppliesCost = 10;

    // Knights
    public int inspireGoldCost = 100;
    public int inspireSuppliesCost = 5;
    public int thornsGoldCost = 50;
    public int thornsSuppliesCost = 5;

    // Laborers
    public int doubleTimeGoldCost = 0;
    public int doubleTimeSuppliesCost = 40;

    // Spearmen
    public int longThrowGoldCost = 75;
    public int longThrowSuppliesCost = 10;
    public int spearWallGoldCost = 100;
    public int spearWallSuppliesCost = 5;

    [HideInInspector] public bool laborersUnlocked, knightsUnlocked, spearmenUnlocked, archersUnlocked;

    public void ApplyLaborerData(int gold, int supplies, float health, float miningSpeedMultiplier, float doubleTimeTime, bool doubleTimeUnlocked)
    {
        laborerGoldCost += gold;
        laborerSuppliesCost += supplies;

        laborerHealth += health;
        laborerMiningSpeedMultiplier += miningSpeedMultiplier;

        laborerDoubleTimeTime += doubleTimeTime;

        if (doubleTimeUnlocked)
            laborerDoubleTimeUnlocked = true;
    }

    public void ApplyKnightData(int gold, int supplies, float health, float leaderHealth, float slashDamage, float leaderSlashDamage, float inspireMultiplier, float inspireTime, float thornsDamageMultiplier, float thornsTime, bool inspireUnlocked, bool thornsUnlocked)
    {
        knightSquadGoldCost += gold;
        knightSquadSuppliesCost += supplies;

        knightHealth += health;
        knightLeaderHealth += leaderHealth;

        knightSlashDamage += slashDamage;
        knightLeaderSlashDamage += leaderSlashDamage;

        knightInspireMultiplier += inspireMultiplier;
        knightInspireTime += inspireTime;

        knightThornsDamageMultiplier += thornsDamageMultiplier;
        knightThornsTime += thornsTime;

        if (inspireUnlocked)
            knightInspireUnlocked = true;

        if (thornsUnlocked)
            knightThornsUnlocked = true;
    }

    public void ApplySpearmenData(int gold, int supplies, float health, float leaderHealth, float piercingDamage, float leaderPiercingDamage, float rangedPiercingDamage, float leaderRangedPiercingDamage, float accuracy, float leaderAccuracy, float longThrowTime, float spearWallTime, bool longThrowUnlocked, bool spearWallUnlocked)
    {
        spearmenSquadGoldCost += gold;
        spearmenSquadSuppliesCost += supplies;

        spearmenHealth += health;
        spearmenLeaderHealth += leaderHealth;

        spearmenPiercingDamage += piercingDamage;
        spearmenLeaderPiercingDamage += leaderPiercingDamage;

        spearmenRangedPiercingDamage += rangedPiercingDamage;
        spearmenLeaderRangedPiercingDamage += leaderRangedPiercingDamage;

        spearmenAccuracy += accuracy;
        spearmenLeaderAccuracy += leaderAccuracy;

        spearmenLongThrowTime += longThrowTime;
        spearmenSpearWallTime += spearWallTime;

        if (longThrowUnlocked)
            spearmenLongThrowUnlocked = true;

        if (spearWallUnlocked)
            spearmenSpearWallUnlocked = true;
    }

    public void ApplyArcherData(int gold, int supplies, float health, float leaderHealth, float piercingDamage, float leaderPiercingDamage, float rangedPiercingDamage, float leaderRangedPiercingDamage, float accuracy, float leaderAccuracy, float fireArrowsDamageMultiplier, float fireArrowsTime, float rapidFireSpeedMultiplier, float rapidFireTime, bool shouldRetreat, bool fireArrowsUnlocked, bool rapidFireUnlocked)
    {
        archerSquadGoldCost += gold;
        archerSquadSuppliesCost += supplies;

        archerHealth += health;
        archerLeaderHealth += leaderHealth;

        archerPiercingDamage += piercingDamage;
        archerLeaderPiercingDamage += leaderPiercingDamage;

        archerRangedPiercingDamage += rangedPiercingDamage;
        archerLeaderRangedPiercingDamage += leaderRangedPiercingDamage;

        archerAccuracy += accuracy;
        archerLeaderAccuracy += leaderAccuracy;

        archerFireArrowsDamageMultiplier += fireArrowsDamageMultiplier;
        archerFireArrowsTime += fireArrowsTime;

        archerRapidFireSpeedMultipilier += rapidFireSpeedMultiplier;
        archerRapidFireTime += rapidFireTime;

        if (shouldRetreat == false)
            archerShouldRetreatWhenEnemyNear = false;

        if (fireArrowsUnlocked)
            archerFireArrowsUnlocked = true;

        if (rapidFireUnlocked)
            archerRapidFireUnlocked = true;
    }

    public float GetHealthData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Laborers:
                return laborerHealth;
            case SquadType.Knights:
                if (isLeader) return knightLeaderHealth;
                else return knightHealth;
            case SquadType.Spearmen:
                if (isLeader) return spearmenLeaderHealth;
                else return spearmenHealth;
            case SquadType.Archers:
                if (isLeader) return archerLeaderHealth;
                else return archerHealth;
            default:
                return 0f;
        }
    }

    public float GetBluntDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            default:
                return 0f;
        }
    }

    public float GetSlashDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Knights:
                if (isLeader) return knightLeaderSlashDamage;
                else return knightSlashDamage;
            default:
                return 0f;
        }
    }

    public float GetPiercingDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Spearmen:
                if (isLeader) return spearmenLeaderPiercingDamage;
                else return spearmenPiercingDamage;
            case SquadType.Archers:
                if (isLeader) return archerLeaderPiercingDamage;
                else return archerPiercingDamage;
            default:
                return 0f;
        }
    }

    public float GetFireDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            default:
                return 0f;
        }
    }

    public float GetRangedBluntDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            default:
                return 0f;
        }
    }

    public float GetRangedPiercingDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Spearmen:
                if (isLeader) return spearmenLeaderRangedPiercingDamage;
                else return spearmenRangedPiercingDamage;
            case SquadType.Archers:
                if (isLeader) return archerLeaderRangedPiercingDamage;
                else return archerRangedPiercingDamage;
            default:
                return 0f;
        }
    }

    public float GetRangedFireDamageData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            default:
                return 0f;
        }
    }

    public float GetRangedAccuracyData(SquadType squadType, bool isLeader)
    {
        switch (squadType)
        {
            case SquadType.Spearmen:
                if (isLeader) return spearmenLeaderAccuracy;
                else return spearmenAccuracy;
            case SquadType.Archers:
                if (isLeader) return archerLeaderAccuracy;
                else return archerAccuracy;
            default:
                return 0f;
        }
    }

    public void UnlockSquad(SquadType squadType)
    {
        switch (squadType)
        {
            case SquadType.Laborers:
                laborersUnlocked = true;
                break;
            case SquadType.Knights:
                knightsUnlocked = true;
                break;
            case SquadType.Spearmen:
                spearmenUnlocked = true;
                break;
            case SquadType.Archers:
                archersUnlocked = true;
                break;
            default:
                break;
        }
        
        GameManager.instance.SaveCurrentGame();
    }

    public bool SquadUnlocked(SquadType squadType)
    {
        switch (squadType)
        {
            case SquadType.Laborers:
                return laborersUnlocked;
            case SquadType.Knights:
                return knightsUnlocked;
            case SquadType.Spearmen:
                return spearmenUnlocked;
            case SquadType.Archers:
                return archersUnlocked;
            default:
                return false;
        }
    }

    public void SaveSquadData()
    {
        // Laborer Data
        ES3.Save("laborersUnlocked", laborersUnlocked, squadDataSavePath);
        ES3.Save("laborerGoldCost", laborerGoldCost, squadDataSavePath);
        ES3.Save("laborerSuppliesCost", laborerSuppliesCost, squadDataSavePath);
        ES3.Save("laborerHealth", laborerHealth, squadDataSavePath);
        ES3.Save("laborerMiningSpeedMultiplier", laborerMiningSpeedMultiplier, squadDataSavePath);
        ES3.Save("laborerDoubleTimeTime", laborerDoubleTimeTime, squadDataSavePath);
        ES3.Save("laborerDoubleTimeUnlocked", laborerDoubleTimeUnlocked, squadDataSavePath);

        // Knight Data
        ES3.Save("knightsUnlocked", knightsUnlocked, squadDataSavePath);
        ES3.Save("knightSquadGoldCost", knightSquadGoldCost, squadDataSavePath);
        ES3.Save("knightSquadSuppliesCost", knightSquadSuppliesCost, squadDataSavePath);
        ES3.Save("knightHealth", knightHealth, squadDataSavePath);
        ES3.Save("knightLeaderHealth", knightLeaderHealth, squadDataSavePath);
        ES3.Save("knightSlashDamage", knightSlashDamage, squadDataSavePath);
        ES3.Save("knightLeaderSlashDamage", knightLeaderSlashDamage, squadDataSavePath);
        ES3.Save("knightInspireMultiplier", knightInspireMultiplier, squadDataSavePath);
        ES3.Save("knightInspireTime", knightInspireTime, squadDataSavePath);
        ES3.Save("knightThornsDamageMultiplier", knightThornsDamageMultiplier, squadDataSavePath);
        ES3.Save("knightThornsTime", knightThornsTime, squadDataSavePath);
        ES3.Save("knightInspireUnlocked", knightInspireUnlocked, squadDataSavePath);
        ES3.Save("knightThornsUnlocked", knightThornsUnlocked, squadDataSavePath);

        // Spearmen Data
        ES3.Save("spearmenUnlocked", spearmenUnlocked, squadDataSavePath);
        ES3.Save("spearmenSquadGoldCost", spearmenSquadGoldCost, squadDataSavePath);
        ES3.Save("spearmenSquadSuppliesCost", spearmenSquadSuppliesCost, squadDataSavePath);
        ES3.Save("spearmenHealth", spearmenHealth, squadDataSavePath);
        ES3.Save("spearmenLeaderHealth", spearmenLeaderHealth, squadDataSavePath);
        ES3.Save("spearmenPiercingDamage", spearmenPiercingDamage, squadDataSavePath);
        ES3.Save("spearmenLeaderPiercingDamage", spearmenLeaderPiercingDamage, squadDataSavePath);
        ES3.Save("spearmenRangedPiercingDamage", spearmenRangedPiercingDamage, squadDataSavePath);
        ES3.Save("spearmenLeaderRangedPiercingDamage", spearmenLeaderRangedPiercingDamage, squadDataSavePath);
        ES3.Save("spearmenAccuracy", spearmenAccuracy, squadDataSavePath);
        ES3.Save("spearmenLeaderAccuracy", spearmenLeaderAccuracy, squadDataSavePath);
        ES3.Save("spearmenLongThrowTime", spearmenLongThrowTime, squadDataSavePath);
        ES3.Save("spearmenSpearWallTime", spearmenSpearWallTime, squadDataSavePath);
        ES3.Save("spearmenLongThrowUnlocked", spearmenLongThrowUnlocked, squadDataSavePath);
        ES3.Save("spearmenSpearWallUnlocked", spearmenSpearWallUnlocked, squadDataSavePath);

        // Archer Data
        ES3.Save("archersUnlocked", archersUnlocked, squadDataSavePath);
        ES3.Save("archerSquadGoldCost", archerSquadGoldCost, squadDataSavePath);
        ES3.Save("archerSquadSuppliesCost", archerSquadSuppliesCost, squadDataSavePath);
        ES3.Save("archerHealth", archerHealth, squadDataSavePath);
        ES3.Save("archerLeaderHealth", archerLeaderHealth, squadDataSavePath);
        ES3.Save("archerPiercingDamage", archerPiercingDamage, squadDataSavePath);
        ES3.Save("archerLeaderPiercingDamage", archerLeaderPiercingDamage, squadDataSavePath);
        ES3.Save("archerRangedPiercingDamage", archerRangedPiercingDamage, squadDataSavePath);
        ES3.Save("archerLeaderRangedPiercingDamage", archerLeaderRangedPiercingDamage, squadDataSavePath);
        ES3.Save("archerAccuracy", archerAccuracy, squadDataSavePath);
        ES3.Save("archerLeaderAccuracy", archerLeaderAccuracy, squadDataSavePath);
        ES3.Save("archerFireArrowsTime", archerFireArrowsTime, squadDataSavePath);
        ES3.Save("archerFireArrowDamageMultiplier", archerFireArrowsDamageMultiplier, squadDataSavePath);
        ES3.Save("archerRapidFireTime", archerRapidFireTime, squadDataSavePath);
        ES3.Save("archerRapidFireSpeedMultiplier", archerRapidFireSpeedMultipilier, squadDataSavePath);
        ES3.Save("archerShouldRetreatWhenEnemyNear", archerShouldRetreatWhenEnemyNear, squadDataSavePath);
        ES3.Save("archerFireArrowsUnlocked", archerFireArrowsUnlocked, squadDataSavePath);
        ES3.Save("archerRapidFireUnlocked", archerRapidFireUnlocked, squadDataSavePath);
    }

    public void LoadSquadData()
    {
        // Laborer Data
        laborersUnlocked = ES3.Load("laborersUnlocked", squadDataSavePath, true);
        laborerGoldCost = ES3.Load("laborerGoldCost", squadDataSavePath, 0);
        laborerSuppliesCost = ES3.Load("laborerSuppliesCost", squadDataSavePath, 0);
        laborerHealth = ES3.Load("laborerHealth", squadDataSavePath, 0f);
        laborerMiningSpeedMultiplier = ES3.Load("laborerMiningSpeedMultiplier", squadDataSavePath, defaultMiningSpeedMultiplier);
        laborerDoubleTimeTime = ES3.Load("laborerDoubleTimeTime", squadDataSavePath, defaultDoubleTimeTime);
        laborerDoubleTimeUnlocked = ES3.Load("laborerDoubleTimeUnlocked", squadDataSavePath, false);

        // Knight Data
        knightsUnlocked = ES3.Load("knightsUnlocked", squadDataSavePath, false);
        knightSquadGoldCost = ES3.Load("knightSquadGoldCost", squadDataSavePath, 0);
        knightSquadSuppliesCost = ES3.Load("knightSquadSuppliesCost", squadDataSavePath, 0);
        knightHealth = ES3.Load("knightHealth", squadDataSavePath, 0f);
        knightLeaderHealth = ES3.Load("knightLeaderHealth", squadDataSavePath, 0f);
        knightSlashDamage = ES3.Load("knightSlashDamage", squadDataSavePath, 0f);
        knightLeaderSlashDamage = ES3.Load("knightLeaderSlashDamage", squadDataSavePath, 0f);
        knightInspireMultiplier = ES3.Load("knightInspireMultiplier", squadDataSavePath, defaultInspireMultiplier);
        knightInspireTime = ES3.Load("knightInspireTime", squadDataSavePath, defaultInspireTime);
        knightThornsDamageMultiplier = ES3.Load("knightThornsDamageMultiplier", squadDataSavePath, defaultThornsDamageMultiplier);
        knightThornsTime = ES3.Load("knightThornsTime", squadDataSavePath, defaultThornsTime);
        knightInspireUnlocked = ES3.Load("knightInspireUnlocked", squadDataSavePath, false);
        knightThornsUnlocked = ES3.Load("knightThornsUnlocked", squadDataSavePath, false);

        // Spearmen Data
        spearmenUnlocked = ES3.Load("spearmenUnlocked", squadDataSavePath, true);
        spearmenSquadGoldCost = ES3.Load("spearmenSquadGoldCost", squadDataSavePath, 0);
        spearmenSquadSuppliesCost = ES3.Load("spearmenSquadSuppliesCost", squadDataSavePath, 0);
        spearmenHealth = ES3.Load("spearmenHealth", squadDataSavePath, 0f);
        spearmenLeaderHealth = ES3.Load("spearmenLeaderHealth", squadDataSavePath, 0f);
        spearmenPiercingDamage = ES3.Load("spearmenPiercingDamage", squadDataSavePath, 0f);
        spearmenLeaderPiercingDamage = ES3.Load("spearmenLeaderPiercingDamage", squadDataSavePath, 0f);
        spearmenRangedPiercingDamage = ES3.Load("spearmenRangedPiercingDamage", squadDataSavePath, 0f);
        spearmenLeaderRangedPiercingDamage = ES3.Load("spearmenLeaderRangedPiercingDamage", squadDataSavePath, 0f);
        spearmenAccuracy = ES3.Load("spearmenAccuracy", squadDataSavePath, 0f);
        spearmenLeaderAccuracy = ES3.Load("spearmenLeaderAccuracy", squadDataSavePath, 0f);
        spearmenLongThrowTime = ES3.Load("spearmenLongThrowTime", squadDataSavePath, defaultLongThrowTime);
        spearmenSpearWallTime = ES3.Load("spearmenSpearWallTime", squadDataSavePath, defaultSpearWallTime);
        spearmenLongThrowUnlocked = ES3.Load("spearmenLongThrowUnlocked", squadDataSavePath, false);
        spearmenSpearWallUnlocked = ES3.Load("spearmenSpearWallUnlocked", squadDataSavePath, false);

        // Archer Data
        archersUnlocked = ES3.Load("archersUnlocked", squadDataSavePath, false);
        archerSquadGoldCost = ES3.Load("archerSquadGoldCost", squadDataSavePath, 0);
        archerSquadSuppliesCost = ES3.Load("archerSquadSuppliesCost", squadDataSavePath, 0);
        archerHealth = ES3.Load("archerHealth", squadDataSavePath, 0f);
        archerLeaderHealth = ES3.Load("archerLeaderHealth", squadDataSavePath, 0f);
        archerPiercingDamage = ES3.Load("archerPiercingDamage", squadDataSavePath, 0f);
        archerLeaderPiercingDamage = ES3.Load("archerLeaderPiercingDamage", squadDataSavePath, 0f);
        archerRangedPiercingDamage = ES3.Load("archerRangedPiercingDamage", squadDataSavePath, 0f);
        archerLeaderRangedPiercingDamage = ES3.Load("archerLeaderRangedPiercingDamage", squadDataSavePath, 0f);
        archerAccuracy = ES3.Load("archerAccuracy", squadDataSavePath, 0f);
        archerLeaderAccuracy = ES3.Load("archerLeaderAccuracy", squadDataSavePath, 0f);
        archerFireArrowsTime = ES3.Load("archerFireArrowsTime", squadDataSavePath, defaultFireArrowTime);
        archerFireArrowsDamageMultiplier = ES3.Load("archerFireArrowDamageMultiplier", squadDataSavePath, 0f);
        archerRapidFireTime = ES3.Load("archerRapidFireTime", squadDataSavePath, defaultRapidFireTime);
        archerRapidFireSpeedMultipilier = ES3.Load("archerRapidFireSpeedMultiplier", squadDataSavePath, defaultRapidFireSpeedMultiplier);
        archerShouldRetreatWhenEnemyNear = ES3.Load("archerShouldRetreatWhenEnemyNear", squadDataSavePath, true);
        archerFireArrowsUnlocked = ES3.Load("archerFireArrowsUnlocked", squadDataSavePath, false);
        archerRapidFireUnlocked = ES3.Load("archerRapidFireUnlocked", squadDataSavePath, false);
    }
}