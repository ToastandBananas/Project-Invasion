using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AbilityIconController : MonoBehaviour
{
    [Header("Ability Icons")]
    public Sprite fireArrowsIcon;
    public Sprite rapidFireIcon, thornsIcon, inspireIcon, doubleTimeIcon, longThrowIcon, spearWallIcon, blessIcon, resurrectIcon;

    [HideInInspector] public SquadHighlighter squadHighlighter;
    [HideInInspector] public Squad selectedSquad;
    [HideInInspector] public Squad activeAbilitySquad;
    [HideInInspector] public bool abilitySelectSquadActive;
    [HideInInspector] public bool resurrectAbilityActive;

    List<Button>      abilityIconButtons = new List<Button>();
    List<Image>       abilityIconImages  = new List<Image>();
    List<AbilityIcon> abilityIcons       = new List<AbilityIcon>();

    AudioManager audioManager;
    DefenderSpawner defenderSpawner;
    GameManager gm;
    ResourceDisplay resourceDisplay;
    SquadData squadData;
    Tooltip tooltip;
    Transform deadCharactersParent;

    LayerMask squadMask;

    #region Singleton
    public static AbilityIconController instance;
    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
                Destroy(gameObject);
        }
        else
            instance = this;
    }
    #endregion

    void Start()
    {
        audioManager = AudioManager.instance;
        defenderSpawner = DefenderSpawner.instance;
        gm = GameManager.instance;
        resourceDisplay = ResourceDisplay.instance;
        squadData = GameManager.instance.squadData;
        squadHighlighter = SquadHighlighter.instance;
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
        deadCharactersParent = GameObject.Find("Dead Characters").transform;
        squadMask = LayerMask.GetMask("Squads");

        for (int i = 0; i < transform.childCount; i++)
        {
            abilityIconButtons.Add(transform.GetChild(i).GetComponentInChildren<Button>());
            abilityIconImages.Add(abilityIconButtons[i].GetComponent<Image>());
            abilityIcons.Add(abilityIconButtons[i].GetComponent<AbilityIcon>());
            if (abilityIconButtons[i].gameObject.activeSelf)
                abilityIconButtons[i].transform.parent.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        CheckIfIconIsInteractable();

        if (GameControls.gamePlayActions.select.WasPressed && abilitySelectSquadActive)
        {
            if (resurrectAbilityActive)
                ResurrectUnits();
        }

        if (GameControls.gamePlayActions.deselect.WasPressed && abilitySelectSquadActive)
            Reset();
    }

    public void EnableAbilityIcons()
    {
        if (defenderSpawner.ghostImageSquad == null && abilitySelectSquadActive == false)
        {
            switch (selectedSquad.squadType)
            {
                case SquadType.Archers:
                    SetArcherIcons();
                    break;
                case SquadType.Knights:
                    SetKnightIcons();
                    break;
                case SquadType.Laborers:
                    SetLaborerIcons();
                    break;
                case SquadType.Spearmen:
                    SetSpearmenIcons();
                    break;
                case SquadType.Priests:
                    SetPriestIcons();
                    break;
                default:
                    break;
            }

            if (selectedSquad.isCastleWallSquad == false)
                transform.position = selectedSquad.transform.position + new Vector3(-0.35f, 0f);
            else
                transform.position = selectedSquad.transform.position + new Vector3(0.3f, 0f); // For castle wall squads, place the icon to the right of the squad
        }
    }

    #region Archer
    void SetArcherIcons()
    {
        if (selectedSquad.abilityActive == false)
        {
            // Fire Arrows
            if (squadData.archerFireArrowsUnlocked)
                SetupIcon(0, squadData.fireArrowsGoldCost, squadData.fireArrowsSuppliesCost, fireArrowsIcon, ActivateFireArrows);

            // Rapid Fire
            if (squadData.archerRapidFireUnlocked)
                SetupIcon(1, squadData.rapidFireGoldCost, squadData.rapidFireSuppliesCost, rapidFireIcon, ActivateRapidFire);
        }
    }

    void ActivateFireArrows()
    {
        InitializeUseAbility(squadData.fireArrowsGoldCost, squadData.fireArrowsSuppliesCost, true);

        if (selectedSquad.leader != null)
        {
            selectedSquad.leader.myShooter.isShootingSecondaryProjectile = true;
            selectedSquad.leader.myShooter.fireDamage = selectedSquad.leader.myShooter.piercingDamage * selectedSquad.leader.myShooter.secondaryRangedDamageMultiplier;
        }

        foreach (Defender unit in selectedSquad.units)
        {
            unit.myShooter.isShootingSecondaryProjectile = true;
            unit.myShooter.fireDamage = unit.myShooter.piercingDamage * unit.myShooter.secondaryRangedDamageMultiplier;
        }

        StartCoroutine(DeactivateFireArrows(selectedSquad));
    }

    IEnumerator DeactivateFireArrows(Squad squad)
    {
        yield return new WaitForSeconds(squadData.archerFireArrowsTime);

        squad.abilityActive = false;

        if (squad.leader != null)
        {
            squad.leader.myShooter.isShootingSecondaryProjectile = false;
            squad.leader.myShooter.fireDamage = 0f;
        }

        foreach (Defender unit in squad.units)
        {
            unit.myShooter.isShootingSecondaryProjectile = false;
            unit.myShooter.fireDamage = 0f;
        }
    }

    void ActivateRapidFire()
    {
        InitializeUseAbility(squadData.rapidFireGoldCost, squadData.rapidFireSuppliesCost, true);

        if (selectedSquad.leader != null)
            selectedSquad.leader.anim.SetFloat("shootSpeed", squadData.archerRapidFireSpeedMultipilier);

        foreach (Defender unit in selectedSquad.units)
        {
            unit.anim.SetFloat("shootSpeed", squadData.archerRapidFireSpeedMultipilier);
        }

        StartCoroutine(DeactivateRapidShoot(selectedSquad));
    }

    IEnumerator DeactivateRapidShoot(Squad squad)
    {
        if (squad.squadType == SquadType.Archers)
            yield return new WaitForSeconds(squadData.archerRapidFireTime);

        squad.abilityActive = false;

        if (squad.leader != null)
            squad.leader.anim.SetFloat("shootSpeed", 1f);

        foreach (Defender unit in squad.units)
        {
            unit.anim.SetFloat("shootSpeed", 1f);
        }
    }
    #endregion

    #region Knights
    void SetKnightIcons()
    {
        if (selectedSquad.abilityActive == false)
        {
            // Inspire
            if (squadData.knightInspireUnlocked)
                SetupIcon(0, squadData.inspireGoldCost, squadData.inspireSuppliesCost, inspireIcon, ActivateInspire);

            // Thorns
            if (squadData.knightThornsUnlocked)
                SetupIcon(1, squadData.thornsGoldCost, squadData.thornsSuppliesCost, thornsIcon, ActivateThorns);
        }
    }

    void ActivateInspire()
    {
        Squad leftSquad  = null;
        Squad rightSquad = null;

        Collider2D leftSquadCollider  = Physics2D.OverlapCircle(new Vector2(selectedSquad.transform.position.x - 1, selectedSquad.transform.position.y), 0.5f, squadMask);
        Collider2D rightSquadCollider = Physics2D.OverlapCircle(new Vector2(selectedSquad.transform.position.x + 1, selectedSquad.transform.position.y), 0.5f, squadMask);

        int inspireCount = 0;

        if (selectedSquad.leader.health.isInspired == false)
        {
            Inspire_SetStatsForSquad(selectedSquad);
            inspireCount++;
        }

        if (leftSquadCollider != null)
        {
            leftSquad = leftSquadCollider.GetComponent<Squad>();
            if (leftSquad.leader.health.isInspired == false)
            {
                Inspire_SetStatsForSquad(leftSquad);
                inspireCount++;
            }
        }

        if (rightSquadCollider != null)
        {
            rightSquad = rightSquadCollider.GetComponent<Squad>();
            if (rightSquad.leader.health.isInspired == false)
            {
                Inspire_SetStatsForSquad(rightSquad);
                inspireCount++;
            }
        }

        if (inspireCount > 0)
        {
            InitializeUseAbility(squadData.inspireGoldCost, squadData.inspireSuppliesCost, true);
            StartCoroutine(DeactivateInspire(selectedSquad, leftSquad, rightSquad));
        }
        else
        {
            PlayButtonClickSound();
            TextPopup.CreateTextStringPopup(selectedSquad.transform.position, "Targets are already inspired");
            Reset();
        }
    }

    void Inspire_SetStatsForSquad(Squad squad)
    {
        if (squad.leader != null)
        {
            squad.leader.health.isInspired = true;
            squad.leader.health.SetMaxHealth(squad.leader.health.GetMaxHealth() + (squad.leader.health.GetMaxHealth() * squadData.knightInspireMultiplier));
            squad.leader.health.Heal(squad.leader.health.GetMaxHealth() * squadData.knightInspireMultiplier);

            squad.leader.SetAttackDamage(squad.leader.bluntDamage * squadData.knightInspireMultiplier, squad.leader.slashDamage * squadData.knightInspireMultiplier, squad.leader.piercingDamage * squadData.knightInspireMultiplier, squad.leader.fireDamage * squadData.knightInspireMultiplier);

            if (squad.leader.myShooter != null)
                squad.leader.myShooter.SetRangedDamage(squad.leader.myShooter.bluntDamage * squadData.knightInspireMultiplier, squad.leader.myShooter.piercingDamage * squadData.knightInspireMultiplier, squad.leader.myShooter.fireDamage * squadData.knightInspireMultiplier);
        }

        foreach (Defender unit in squad.units)
        {
            unit.health.isInspired = true;
            unit.health.SetMaxHealth(unit.health.GetMaxHealth() + (unit.health.GetMaxHealth() * squadData.knightInspireMultiplier));
            unit.health.Heal(unit.health.GetMaxHealth() * squadData.knightInspireMultiplier);

            unit.SetAttackDamage(unit.bluntDamage * squadData.knightInspireMultiplier, unit.slashDamage * squadData.knightInspireMultiplier, unit.piercingDamage * squadData.knightInspireMultiplier, unit.fireDamage * squadData.knightInspireMultiplier);

            if (unit.myShooter != null)
                unit.myShooter.SetRangedDamage(unit.myShooter.bluntDamage * squadData.knightInspireMultiplier, unit.myShooter.piercingDamage * squadData.knightInspireMultiplier, unit.myShooter.fireDamage * squadData.knightInspireMultiplier);
        }
    }

    void Inspire_ResetStatsForSquad(Squad squad)
    {
        if (squad.leader != null)
        {
            squad.leader.health.isInspired = false;
            squad.leader.health.SetMaxHealth(squad.leader.health.GetMaxHealth() - (squad.leader.health.startingMaxHealth * squadData.knightInspireMultiplier));
            if (squad.leader.health.GetCurrentHealth() > squad.leader.health.GetMaxHealth())
                squad.leader.health.SetCurrentHealthToMaxHealth();

            squad.leader.SetAttackDamage(squad.leader.startingBluntDamage * -squadData.knightInspireMultiplier, squad.leader.startingSlashDamage * -squadData.knightInspireMultiplier, squad.leader.startingPiercingDamage * -squadData.knightInspireMultiplier, squad.leader.startingFireDamage * -squadData.knightInspireMultiplier);

            if (squad.leader.myShooter != null)
                squad.leader.myShooter.SetRangedDamage(squad.leader.myShooter.startingBluntDamage * -squadData.knightInspireMultiplier, squad.leader.myShooter.startingPiercingDamage * -squadData.knightInspireMultiplier, squad.leader.myShooter.startingFireDamage * -squadData.knightInspireMultiplier);
        }

        foreach (Defender unit in squad.units)
        {
            unit.health.isInspired = false;
            unit.health.SetMaxHealth(unit.health.GetMaxHealth() - (unit.health.startingMaxHealth * squadData.knightInspireMultiplier));
            if (unit.health.GetCurrentHealth() > unit.health.GetMaxHealth())
                unit.health.SetCurrentHealthToMaxHealth();

            unit.SetAttackDamage(unit.startingBluntDamage * -squadData.knightInspireMultiplier, unit.startingSlashDamage * -squadData.knightInspireMultiplier, unit.startingPiercingDamage * -squadData.knightInspireMultiplier, unit.startingFireDamage * -squadData.knightInspireMultiplier);

            if (unit.myShooter != null)
                unit.myShooter.SetRangedDamage(unit.myShooter.startingBluntDamage * -squadData.knightInspireMultiplier, unit.myShooter.startingPiercingDamage * -squadData.knightInspireMultiplier, unit.myShooter.startingFireDamage * -squadData.knightInspireMultiplier);
        }
    }

    IEnumerator DeactivateInspire(Squad mainSquad, Squad leftSquad, Squad rightSquad)
    {
        yield return new WaitForSeconds(squadData.knightInspireTime);

        if (mainSquad != null)
        {
            mainSquad.abilityActive = false;
            Inspire_ResetStatsForSquad(mainSquad);
        }

        if (leftSquad != null)
            Inspire_ResetStatsForSquad(leftSquad);

        if (rightSquad != null)
            Inspire_ResetStatsForSquad(rightSquad);
    }

    void ActivateThorns()
    {
        InitializeUseAbility(squadData.thornsGoldCost, squadData.thornsSuppliesCost, true);

        if (selectedSquad.leader != null)
        {
            selectedSquad.leader.health.thornsActive = true;
            selectedSquad.leader.effectAnim3.SetBool("thornsActive", true);
        }

        foreach (Defender unit in selectedSquad.units)
        {
            unit.health.thornsActive = true;
            unit.effectAnim3.SetBool("thornsActive", true);
        }

        StartCoroutine(DeactivateThorns(selectedSquad));
    }

    IEnumerator DeactivateThorns(Squad squad)
    {
        yield return new WaitForSeconds(squadData.knightThornsTime);

        if (squad != null)
        {
            squad.abilityActive = false;

            if (squad.leader != null)
            {
                squad.leader.health.thornsActive = false;
                squad.leader.effectAnim3.SetBool("thornsActive", false);
            }

            foreach (Defender unit in squad.units)
            {
                unit.health.thornsActive = false;
                unit.effectAnim3.SetBool("thornsActive", false);
            }
        }
    }
    #endregion

    #region Laborers
    void SetLaborerIcons()
    {
        if (selectedSquad.abilityActive == false)
        {
            // Double Time
            if (squadData.laborerDoubleTimeUnlocked)
                SetupIcon(0, squadData.doubleTimeGoldCost, squadData.doubleTimeSuppliesCost, doubleTimeIcon, ActivateDoubleTime);
        }
    }

    public void ActivateDoubleTime()
    {
        InitializeUseAbility(squadData.doubleTimeGoldCost, squadData.doubleTimeSuppliesCost, true);

        foreach (Defender unit in selectedSquad.units)
        {
            unit.anim.SetFloat("miningSpeed", unit.anim.GetFloat("miningSpeed") * 2);
        }

        StartCoroutine(DeactivateDoubleTime(selectedSquad));
    }

    IEnumerator DeactivateDoubleTime(Squad squad)
    {
        yield return new WaitForSeconds(squadData.laborerDoubleTimeTime);

        foreach (Defender unit in squad.units)
        {
            unit.anim.SetFloat("miningSpeed", unit.anim.GetFloat("miningSpeed") / 2);
        }
    }
    #endregion

    #region Priests
    void SetPriestIcons()
    {
        if (selectedSquad.abilityActive == false)
        {
            // Resurrect
            if (squadData.priestResurrectUnlocked)
                SetupIcon(0, 0, 0, resurrectIcon, ActivateResurrect_SquadSelect); // We don't want to use resources right away, since we won't know how much to spend until we choose a Squad to resurrect from

            // Bless
            if (squadData.priestBlessUnlocked)
                SetupIcon(1, squadData.blessGoldCost, squadData.blessSuppliesCost, blessIcon, ActivateBless);
        }
    }

    void ActivateResurrect_SquadSelect()
    {
        abilitySelectSquadActive = true;
        resurrectAbilityActive = true;
        activeAbilitySquad = selectedSquad;
        activeAbilitySquad.HighlightSquad();

        InitializeUseAbility(0, 0, false);
    }

    void ResurrectUnits()
    {
        if (squadHighlighter.selectedSquad == null)
        {
            TextPopup.CreateTextStringPopup(Utilities.GetMouseWorldPosition(), "Invalid target");
            Reset();
            return;
        }
        else if (activeAbilitySquad.rangeCollider.squadsInRange.Contains(squadHighlighter.selectedSquad) == false && squadHighlighter.selectedSquad != activeAbilitySquad)
        {
            TextPopup.CreateTextStringPopup(squadHighlighter.selectedSquad.transform.position, "Target not in range");
            Reset();
            return;
        }
        else if (squadHighlighter.selectedSquad.deadUnits.Count == 0)
        {
            TextPopup.CreateTextStringPopup(squadHighlighter.selectedSquad.transform.position, "No units to resurrect");
            Reset();
            return;
        }

        int goldCost = Mathf.RoundToInt(squadHighlighter.selectedSquad.GetGoldCost() / squadHighlighter.selectedSquad.maxUnitCount * squadHighlighter.selectedSquad.deadUnits.Count);
        int suppliesCost = Mathf.RoundToInt(squadHighlighter.selectedSquad.GetSuppliesCost() / squadHighlighter.selectedSquad.maxUnitCount * squadHighlighter.selectedSquad.deadUnits.Count);

        if (resourceDisplay.HaveEnoughGold(goldCost) && resourceDisplay.HaveEnoughSupplies(suppliesCost))
        {
            activeAbilitySquad.abilityActive = true;
            PlayCastSpellAnims_Priests(activeAbilitySquad);

            float resurrectAnimationTime = audioManager.resurrectSounds[0].clip.length;

            for (int i = 0; i < squadHighlighter.selectedSquad.deadUnits.Count; i++)
            {
                ResurrectUnitFromSquad(squadHighlighter.selectedSquad.deadUnits[i], resurrectAnimationTime);
            }

            resourceDisplay.SpendGold(goldCost);
            resourceDisplay.SpendSupplies(suppliesCost);

            audioManager.PlayRandomSound(audioManager.resurrectSounds);

            StartCoroutine(DeactivateResurrect(resurrectAnimationTime, activeAbilitySquad));
            Reset();
        }
        else // Not enough supplies or gold, so cancel using the ability
        {
            TextPopup.CreateTextStringPopup(squadHighlighter.selectedSquad.transform.position, "Not enough resources");
            Reset();
        }
    }

    void ResurrectUnitFromSquad(Defender defenderToResurrect, float resurrectAnimationTime)
    {
        StartCoroutine(defenderToResurrect.health.Resurrect(resurrectAnimationTime, null));
    }
    
    IEnumerator DeactivateResurrect(float waitTime, Squad activeSquad)
    {
        yield return new WaitForSeconds(waitTime);
        activeSquad.abilityActive = false;
    }

    void ActivateBless()
    {
        if (selectedSquad.rangeCollider.squadsInRange.Count > 0)
        {
            Squad firstSquad = null;
            Squad secondSquad = null;
            if (selectedSquad.rangeCollider.squadsInRange.Count > 0)
                firstSquad = selectedSquad.rangeCollider.squadsInRange[0];
            if (selectedSquad.rangeCollider.squadsInRange.Count > 1)
                secondSquad = selectedSquad.rangeCollider.squadsInRange[1];

            int blessCount = 0;
            if (firstSquad != null && firstSquad.leader.health.isBlessed == false)
                blessCount++;
            if (secondSquad != null && secondSquad.leader.health.isBlessed == false)
                blessCount++;

            if (blessCount == 0)
            {
                PlayButtonClickSound();
                TextPopup.CreateTextStringPopup(selectedSquad.transform.position, "Targets already blessed");
                Reset();
                return;
            }

            InitializeUseAbility(squadData.blessGoldCost, squadData.blessSuppliesCost, true);
            PlayCastSpellAnims_Priests(selectedSquad);
            audioManager.PlayRandomSound(audioManager.blessSounds);
            
            StartCoroutine(Bless(selectedSquad, firstSquad, secondSquad));
        }
        else
        {
            PlayButtonClickSound();
            TextPopup.CreateTextStringPopup(selectedSquad.transform.position, "No squads in range");
            Reset();
        }
    }

    IEnumerator Bless(Squad priestSquad, Squad firstSquad, Squad secondSquad)
    {
        yield return new WaitForSeconds(audioManager.blessSounds[0].clip.length);

        for (int i = 0; i < priestSquad.rangeCollider.squadsInRange.Count; i++)
        {
            if (priestSquad.rangeCollider.squadsInRange[i].leader.health.isBlessed == false)
            {
                priestSquad.rangeCollider.squadsInRange[i].leader.health.isBlessed = true;
                priestSquad.rangeCollider.squadsInRange[i].leader.effectAnim1.SetBool("blessActive", true);
                for (int j = 0; j < priestSquad.rangeCollider.squadsInRange[i].units.Count; j++)
                {
                    priestSquad.rangeCollider.squadsInRange[i].units[j].health.isBlessed = true;
                    priestSquad.rangeCollider.squadsInRange[i].units[j].effectAnim1.SetBool("blessActive", true);
                }
            }
        }
        
        StartCoroutine(DeactivateBless(squadData.priestBlessTime, priestSquad, firstSquad, secondSquad));
    }

    IEnumerator DeactivateBless(float waitTime, Squad priestSquad, Squad firstSquad, Squad secondSquad)
    {
        yield return new WaitForSeconds(waitTime);

        priestSquad.abilityActive = false;

        if (firstSquad != null)
        {
            firstSquad.leader.health.isBlessed = false;
            firstSquad.leader.effectAnim1.SetBool("blessActive", false);
            for (int i = 0; i < firstSquad.units.Count; i++)
            {
                firstSquad.units[i].health.isBlessed = false;
                firstSquad.units[i].effectAnim1.SetBool("blessActive", false);
            }
        }

        if (secondSquad != null)
        {
            secondSquad.leader.health.isBlessed = false;
            secondSquad.leader.effectAnim1.SetBool("blessActive", false);
            for (int i = 0; i < secondSquad.units.Count; i++)
            {
                secondSquad.units[i].health.isBlessed = false;
                secondSquad.units[i].effectAnim1.SetBool("blessActive", false);
            }
        }
    }

    void PlayCastSpellAnims_Priests(Squad priestSquad)
    {
        priestSquad.leader.anim.Play("CastSpell", 0);
        for (int i = 0; i < priestSquad.units.Count; i++)
        {
            priestSquad.units[i].anim.Play("CastSpell", 0);
        }
    }
    #endregion

    #region Spearmen
    void SetSpearmenIcons()
    {
        if (selectedSquad.abilityActive == false)
        {
            // Long Throw
            if (squadData.spearmenLongThrowUnlocked)
                SetupIcon(0, squadData.longThrowGoldCost, squadData.longThrowSuppliesCost, longThrowIcon, ActivateLongThrow);

            // Spear Wall
            if (squadData.spearmenSpearWallUnlocked)
                SetupIcon(1, squadData.spearWallGoldCost, squadData.spearWallSuppliesCost, spearWallIcon, ActivateSpearWall);
        }
    }

    void ActivateLongThrow()
    {
        InitializeUseAbility(squadData.longThrowGoldCost, squadData.longThrowSuppliesCost, true);

        if (selectedSquad.isCastleWallSquad == false)
        {
            selectedSquad.rangeCollider.boxCollider.offset = new Vector2(5.25f, 0f);
            selectedSquad.rangeCollider.boxCollider.size = new Vector2(9.5f, 0.9f);
        }
        else // For castle wall squads
        {
            selectedSquad.rangeCollider.boxCollider.offset = new Vector2(5.4f, 0f);
            selectedSquad.rangeCollider.boxCollider.size = new Vector2(10f, 2.9f);
        }

        StartCoroutine(DeactivateLongThrow(selectedSquad));
    }

    IEnumerator DeactivateLongThrow(Squad squad)
    {
        yield return new WaitForSeconds(squadData.spearmenLongThrowTime);

        if (squad != null)
        {
            squad.abilityActive = false;
            squad.rangeCollider.boxCollider.offset = squad.rangeCollider.originalOffset;
            squad.rangeCollider.boxCollider.size = squad.rangeCollider.originalSize;
        }
    }

    void ActivateSpearWall()
    {
        if (selectedSquad.attackersNearby.Count == 0)
        {
            InitializeUseAbility(squadData.spearWallGoldCost, squadData.spearWallSuppliesCost, true);

            selectedSquad.squadFormation = SquadFormation.Wall;
            selectedSquad.AssignLeaderPosition();
            selectedSquad.AssignUnitPositions();

            StartCoroutine(DeactivateSpearWall(selectedSquad, selectedSquad.leader.health.bluntResistance, selectedSquad.leader.health.piercingResistance, selectedSquad.leader.health.slashResistance, selectedSquad.leader.health.fireResistance));

            if (selectedSquad.leader != null)
            {
                selectedSquad.leader.shouldKnockback = true;
                selectedSquad.leader.health.bluntResistance = 1f;
                selectedSquad.leader.health.piercingResistance = 1f;
                selectedSquad.leader.health.slashResistance = 1f;
                selectedSquad.leader.health.fireResistance = 1f;
            }

            foreach (Defender unit in selectedSquad.units)
            {
                unit.shouldKnockback = true;
                unit.health.bluntResistance = 1f;
                unit.health.piercingResistance = 1f;
                unit.health.slashResistance = 1f;
                unit.health.fireResistance = 1f;
            }
        }
        else
        {
            PlayButtonClickSound();
            TextPopup.CreateTextStringPopup(selectedSquad.transform.position, "Cannot activate. Enemies nearby.");
        }
    }

    IEnumerator DeactivateSpearWall(Squad squad, float originalBluntResist, float originalPiercingResist, float originalSlashResist, float originalFireResist)
    {
        yield return new WaitForSeconds(squadData.spearmenSpearWallTime);

        if (squad != null)
        {
            squad.squadFormation = SquadFormation.Line;
            squad.AssignLeaderPosition();
            squad.AssignUnitPositions();

            if (squad.leader != null)
            {
                squad.leader.shouldKnockback = false;
                squad.leader.health.bluntResistance = originalBluntResist;
                squad.leader.health.piercingResistance = originalPiercingResist;
                squad.leader.health.slashResistance = originalSlashResist;
                squad.leader.health.fireResistance = originalFireResist;
            }

            foreach (Defender unit in squad.units)
            {
                unit.shouldKnockback = false;
                unit.health.bluntResistance = originalBluntResist;
                unit.health.piercingResistance = originalPiercingResist;
                unit.health.slashResistance = originalSlashResist;
                unit.health.fireResistance = originalFireResist;
            }
        }
    }
    #endregion

    public void DisableAbilityIcons()
    {
        foreach (Button button in abilityIconButtons)
        {
            button.interactable = true;
            button.onClick.RemoveAllListeners();
            button.transform.parent.gameObject.SetActive(false);
        }

        tooltip.DeactivateTooltip();
    }

    void SetupIcon(int iconIndex, int abilityGoldCost, int abilitySuppliesCost, Sprite iconSprite, UnityAction listenerFunction)
    {
        abilityIcons[iconIndex].abilityGoldCost = abilityGoldCost;
        abilityIcons[iconIndex].abilitySuppliesCost = abilitySuppliesCost;

        CheckIfIconIsInteractable();

        abilityIconButtons[iconIndex].transform.parent.gameObject.SetActive(true);
        abilityIconButtons[iconIndex].onClick.AddListener(listenerFunction);
        abilityIconImages[iconIndex].sprite = iconSprite;
    }

    void InitializeUseAbility(int abilityGoldCost, int abilitySuppliesCost, bool activateAbilityImmediately)
    {
        if (activateAbilityImmediately)
            selectedSquad.abilityActive = true;

        resourceDisplay.SpendGold(abilityGoldCost);
        resourceDisplay.SpendSupplies(abilitySuppliesCost);
        PlayButtonClickSound();
        DisableAbilityIcons();
    }

    void PlayButtonClickSound()
    {
        audioManager.PlaySound(audioManager.buttonClickSounds, audioManager.buttonClickSounds[0].soundName, Vector3.zero);
    }

    void CheckIfIconIsInteractable()
    {
        for (int i = 0; i < abilityIcons.Count; i++)
        {
            if (abilityIcons[i].gameObject.activeSelf)
            {
                if (resourceDisplay.HaveEnoughSupplies(abilityIcons[i].abilitySuppliesCost) == false || resourceDisplay.HaveEnoughGold(abilityIcons[i].abilityGoldCost) == false)
                    abilityIconButtons[i].interactable = false;
                else
                    abilityIconButtons[i].interactable = true;
            }
        }
    }

    void Reset()
    {
        abilitySelectSquadActive = false;
        resurrectAbilityActive = false;
        activeAbilitySquad = null;
        squadHighlighter.DisableHighlighter();

        if (selectedSquad != null)
            EnableAbilityIcons();
    }
}
