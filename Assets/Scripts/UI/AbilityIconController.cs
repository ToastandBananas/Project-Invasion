using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AbilityIconController : MonoBehaviour
{
    public Sprite fireArrowsIcon, rapidFireIcon, thornsIcon, longThrowIcon;

    [HideInInspector] public Squad selectedSquad;

    List<Button>      abilityIconButtons = new List<Button>();
    List<Image>       abilityIconImages  = new List<Image>();
    List<AbilityIcon> abilityIcons       = new List<AbilityIcon>();

    AudioManager audioManager;
    DefenderSpawner defenderSpawner;
    ResourceDisplay resourceDisplay;
    SquadData squadData;
    Tooltip tooltip;

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
        resourceDisplay = ResourceDisplay.instance;
        squadData = GameManager.instance.squadData;
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();

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
        CheckIfIconsInteractable();
    }

    public void EnableAbilityIcons()
    {
        if (defenderSpawner.ghostImageSquad == null)
        {
            switch (selectedSquad.squadType)
            {
                case SquadType.Knights:
                    SetKnightIcons();
                    break;
                case SquadType.Spearmen:
                    SetSpearmenIcons();
                    break;
                case SquadType.Archers:
                    SetArcherIcons();
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
                SetupIcon(0, squadData.fireArrowsCost, fireArrowsIcon, ActivateFireArrows);

            // Rapid Fire
            if (squadData.archerRapidFireUnlocked)
                SetupIcon(1, squadData.rapidFireCost, rapidFireIcon, ActivateRapidFire);
        }
    }

    void ActivateFireArrows()
    {
        InitUseAbility(squadData.fireArrowsCost);

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
        InitUseAbility(squadData.rapidFireCost);

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
        else
            yield return new WaitForSeconds(20f);

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
            // Thorns
            if (squadData.knightThornsUnlocked)
                SetupIcon(0, squadData.thornsCost, thornsIcon, ActivateThorns);
        }
    }

    void ActivateThorns()
    {
        InitUseAbility(squadData.thornsCost);

        if (selectedSquad.leader != null)
            selectedSquad.leader.health.thornsActive = true;

        foreach (Defender unit in selectedSquad.units)
        {
            unit.health.thornsActive = true;
        }

        StartCoroutine(DeactivateThorns(selectedSquad));
    }

    IEnumerator DeactivateThorns(Squad squad)
    {
        yield return new WaitForSeconds(squadData.knightThornsTime);

        squad.abilityActive = false;

        if (selectedSquad.leader != null)
            selectedSquad.leader.health.thornsActive = false;

        foreach (Defender unit in selectedSquad.units)
        {
            unit.health.thornsActive = false;
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
                SetupIcon(0, squadData.longThrowCost, longThrowIcon, ActivateLongThrow);
        }
    }

    void ActivateLongThrow()
    {
        InitUseAbility(squadData.longThrowCost);

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

        squad.abilityActive = false;
        squad.rangeCollider.boxCollider.offset = squad.rangeCollider.originalOffset;
        squad.rangeCollider.boxCollider.size = squad.rangeCollider.originalSize;
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

    void SetupIcon(int iconIndex, int abilityCost, Sprite iconSprite, UnityAction listenerFunction)
    {
        abilityIcons[iconIndex].abilityCost = abilityCost;

        CheckIfIconsInteractable();

        abilityIconButtons[iconIndex].transform.parent.gameObject.SetActive(true);
        abilityIconButtons[iconIndex].onClick.AddListener(listenerFunction);
        abilityIconImages[iconIndex].sprite = iconSprite;
    }

    void InitUseAbility(int abilityCost)
    {
        selectedSquad.abilityActive = true;
        resourceDisplay.SpendSupplies(abilityCost);
        PlayButtonClickSound();
        DisableAbilityIcons();
    }

    void PlayButtonClickSound()
    {
        audioManager.PlaySound(audioManager.buttonClickSounds, audioManager.buttonClickSounds[0].soundName, Vector3.zero);
    }

    void CheckIfIconsInteractable()
    {
        for (int i = 0; i < abilityIcons.Count; i++)
        {
            if (abilityIcons[i].gameObject.activeSelf)
            {
                if (resourceDisplay.HaveEnoughSupplies(abilityIcons[i].abilityCost) == false)
                    abilityIconButtons[i].interactable = false;
                else
                    abilityIconButtons[i].interactable = true;
            }
        }
    }
}
