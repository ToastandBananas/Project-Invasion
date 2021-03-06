using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIconController : MonoBehaviour
{
    public Sprite fireArrowsIcon, rapidFireIcon, longThrowIcon;

    [HideInInspector] public List<Button> abilityIconButtons = new List<Button>();
    [HideInInspector] public List<Image>  abilityIconImages  = new List<Image>();
    [HideInInspector] public Squad selectedSquad;

    AudioManager audioManager;
    DefenderSpawner defenderSpawner;
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
        squadData = GameManager.instance.squadData;
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();

        for (int i = 0; i < transform.childCount; i++)
        {
            abilityIconButtons.Add(transform.GetChild(i).GetComponentInChildren<Button>());
            abilityIconImages.Add(abilityIconButtons[i].GetComponent<Image>());
            if (abilityIconButtons[i].gameObject.activeSelf)
                abilityIconButtons[i].transform.parent.gameObject.SetActive(false);
        }
    }
    
    public void EnableAbilityIcons()
    {
        if (defenderSpawner.ghostImageSquad == null)
        {
            switch (selectedSquad.squadType)
            {
                case SquadType.Knights:
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
                transform.position = selectedSquad.transform.position + new Vector3(0.3f, 0f); // For castle wall squads
        }
    }

    public void DisableAbilityIcons()
    {
        foreach (Button button in abilityIconButtons)
        {
            button.onClick.RemoveAllListeners();
            button.transform.parent.gameObject.SetActive(false);
        }

        tooltip.DeactivateTooltip();
    }
    
    void SetSpearmenIcons()
    {
        if (selectedSquad.abilityActive == false)
        {
            if (squadData.spearmenLongThrowUnlocked) // Long Throw
            {
                abilityIconButtons[0].transform.parent.gameObject.SetActive(true);
                abilityIconButtons[0].onClick.AddListener(ActivateLongThrow);
                abilityIconImages[0].sprite = longThrowIcon;
            }
        }
    }

    void ActivateLongThrow()
    {
        selectedSquad.abilityActive = true;
        PlayButtonClickSound();

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

        DisableAbilityIcons();
        StartCoroutine(DeactivateLongThrow(selectedSquad));
    }

    IEnumerator DeactivateLongThrow(Squad squad)
    {
        yield return new WaitForSeconds(squadData.spearmenLongThrowTime);

        squad.abilityActive = false;
        squad.rangeCollider.boxCollider.offset = squad.rangeCollider.originalOffset;
        squad.rangeCollider.boxCollider.size = squad.rangeCollider.originalSize;
    }

    void SetArcherIcons()
    {
        if (selectedSquad.abilityActive == false)
        {
            if (squadData.archerFireArrowsUnlocked) // Fire Arrows
            {
                abilityIconButtons[0].transform.parent.gameObject.SetActive(true);
                abilityIconButtons[0].onClick.AddListener(ActivateSecondaryProjectile);
                abilityIconImages[0].sprite = fireArrowsIcon;
            }

            if (squadData.archerRapidFireUnlocked) // Rapid Fire
            {
                abilityIconButtons[1].transform.parent.gameObject.SetActive(true);
                abilityIconButtons[1].onClick.AddListener(ActivateRapidShoot);
                abilityIconImages[1].sprite = rapidFireIcon;
            }
        }
    }

    void ActivateSecondaryProjectile()
    {
        selectedSquad.abilityActive = true;
        PlayButtonClickSound();

        if (selectedSquad.leader != null)
            selectedSquad.leader.myShooter.isShootingSecondaryProjectile = true;

        foreach (Defender unit in selectedSquad.units)
        {
            unit.myShooter.isShootingSecondaryProjectile = true;
        }

        DisableAbilityIcons();
        StartCoroutine(DeactivateSecondaryProjectile(selectedSquad));
    }

    IEnumerator DeactivateSecondaryProjectile(Squad squad)
    {
        if (squad.squadType == SquadType.Archers)
            yield return new WaitForSeconds(squadData.archerFireArrowsTime);
        else
            yield return new WaitForSeconds(30f);

        squad.abilityActive = false;

        if (squad.leader != null)
            squad.leader.myShooter.isShootingSecondaryProjectile = false;

        foreach (Defender unit in squad.units)
        {
            unit.myShooter.isShootingSecondaryProjectile = false;
        }
    }

    void ActivateRapidShoot()
    {
        selectedSquad.abilityActive = true;
        PlayButtonClickSound();

        if (selectedSquad.leader != null)
            selectedSquad.leader.anim.SetFloat("shootSpeed", squadData.archerRapidFireSpeedMultipilier);

        foreach (Defender unit in selectedSquad.units)
        {
            unit.anim.SetFloat("shootSpeed", squadData.archerRapidFireSpeedMultipilier);
        }

        DisableAbilityIcons();
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

    void PlayButtonClickSound()
    {
        audioManager.PlaySound(audioManager.buttonClickSounds, "WetClick", Vector3.zero);
    }
}
