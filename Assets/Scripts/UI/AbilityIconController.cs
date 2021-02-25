using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIconController : MonoBehaviour
{
    public List<Button> abilityIconButtons = new List<Button>();

    public Squad selectedSquad;

    SquadData squadData;

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
        squadData = GameManager.instance.squadData;

        for (int i = 0; i < transform.childCount; i++)
        {
            abilityIconButtons.Add(transform.GetChild(i).GetComponentInChildren<Button>());
            if (abilityIconButtons[i].gameObject.activeSelf)
                abilityIconButtons[i].transform.parent.gameObject.SetActive(false);
        }
    }

    public void DisableAbilityIcons()
    {
        foreach (Button button in abilityIconButtons)
        {
            button.onClick.RemoveAllListeners();
            button.transform.parent.gameObject.SetActive(false);
        }
    }
    
    public void EnableAbilityIcons()
    {
        switch (selectedSquad.squadType)
        {
            case SquadType.Knights:
                break;
            case SquadType.Spearmen:
                break;
            case SquadType.Archers:
                SetArcherIcons();
                break;
            default:
                break;
        }

        transform.position = selectedSquad.transform.position + new Vector3(-0.35f, 0f);
    }

    void SetArcherIcons()
    {
        if (squadData.archerFireArrowsUnlocked && selectedSquad.leader.myShooter.isShootingSecondaryProjectile == false)
        {
            abilityIconButtons[0].transform.parent.gameObject.SetActive(true);
            abilityIconButtons[0].onClick.AddListener(ActivateSecondaryProjectile);
        }
    }

    void ActivateSecondaryProjectile()
    {
        if (selectedSquad.leader.myShooter.isShootingSecondaryProjectile == false)
        {
            selectedSquad.leader.myShooter.isShootingSecondaryProjectile = true;
            foreach (Defender unit in selectedSquad.units)
            {
                unit.myShooter.isShootingSecondaryProjectile = true;
            }

            DisableAbilityIcons();
            StartCoroutine(DeactivateSecondaryProjectile(selectedSquad, 30f));
        }
    }

    IEnumerator DeactivateSecondaryProjectile(Squad squad, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        squad.leader.myShooter.isShootingSecondaryProjectile = false;
        foreach (Defender unit in squad.units)
        {
            unit.myShooter.isShootingSecondaryProjectile = false;
        }
    }
}
