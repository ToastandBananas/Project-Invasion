using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    StringBuilder stringBuilder = new StringBuilder();

    float textPaddingSize = 8f;
    Vector2 backgroundSize;
    
    RectTransform tooltipBackground;
    Text tooltipText;
    SquadData squadData;

    AbilityIconController abilityIconController;
    Defender leader;
    Defender unit;
    Health leaderHealth;
    Health unitHealth;
    Shooter leaderShooter;
    Shooter unitShooter;
    
    void Start()
    {
        abilityIconController = AbilityIconController.instance;
        squadData = GameManager.instance.squadData;

        tooltipBackground = transform.GetChild(0).GetComponent<RectTransform>();
        tooltipText = transform.GetChild(1).GetComponent<Text>();

        if (tooltipBackground.gameObject.activeSelf) tooltipBackground.gameObject.SetActive(false);
        if (tooltipText.gameObject.activeSelf) tooltipText.gameObject.SetActive(false);
    }
    
    public void ToggleSquadTooltip(Squad squad, Vector2 pos)
    {
        tooltipBackground.gameObject.SetActive(!tooltipBackground.gameObject.activeSelf);
        tooltipText.gameObject.SetActive(!tooltipText.gameObject.activeSelf);

        if (tooltipText.gameObject.activeSelf)
            CreateSquadTooltip(squad, pos);
    }

    void CreateSquadTooltip(Squad squad, Vector2 pos)
    {
        if (stringBuilder.Equals("") == false)
            stringBuilder.Clear();

        leader = squad.transform.Find("Leader").GetChild(0).GetComponent<Defender>();
        unit = squad.transform.Find("Units").GetChild(0).GetComponent<Defender>();
        leaderHealth = leader.GetComponent<Health>();
        unitHealth = unit.GetComponent<Health>();
        leaderShooter = leader.GetComponent<Shooter>();
        unitShooter = unit.GetComponent<Shooter>();

        stringBuilder.Append("<b><size=24>" + squad.squadType + "</size></b>\n\n");

        stringBuilder.Append(squad.description + "\n\n");

        stringBuilder.Append("<b>Unit Stats:</b>\n");
        stringBuilder.Append("Health: " + (unitHealth.GetMaxHealth() + squadData.GetHealthData(squad.squadType, false)).ToString() + "\n");

        if (unit.bluntDamage > 0f || unit.slashDamage > 0f || unit.piercingDamage > 0f || unit.fireDamage > 0f)
        {
            if (unit.bluntDamage > 0f)
                stringBuilder.Append("Melee <i>Blunt</i> Damage: " + (unit.bluntDamage + squadData.GetBluntDamageData(squad.squadType, false)).ToString() + "\n");

            if (unit.slashDamage > 0f)
                stringBuilder.Append("Melee <i>Slash</i> Damage: " + (unit.slashDamage + squadData.GetSlashDamageData(squad.squadType, false)).ToString() + "\n");

            if (unit.piercingDamage > 0f)
                stringBuilder.Append("Melee <i>Piercing</i> Damage: " + (unit.piercingDamage + squadData.GetPiercingDamageData(squad.squadType, false)).ToString() + "\n");

            if (unit.fireDamage > 0f)
                stringBuilder.Append("Melee <i>Fire</i> Damage: " + (unit.fireDamage + squadData.GetFireDamageData(squad.squadType, false)).ToString() + "\n");
        }

        if (unitShooter != null && (unitShooter.bluntDamage > 0f || unitShooter.piercingDamage > 0f || unitShooter.fireDamage > 0f))
        {
            if (unitShooter.bluntDamage > 0f)
                stringBuilder.Append("Ranged <i>Blunt</i> Damage: " + (unitShooter.bluntDamage + squadData.GetRangedBluntDamageData(squad.squadType, false)).ToString() + "\n");

            if (unitShooter.piercingDamage > 0f)
                stringBuilder.Append("Ranged <i>Piercing</i> Damage: " + (unitShooter.piercingDamage + squadData.GetRangedPiercingDamageData(squad.squadType, false)).ToString() + "\n");

            if (unitShooter.fireDamage > 0f)
                stringBuilder.Append("Ranged <i>Fire</i> Damage: " + (unitShooter.fireDamage + squadData.GetRangedFireDamageData(squad.squadType, false)).ToString() + "\n");

            stringBuilder.Append("Ranged Accuracy: " + (unitShooter.accuracy + squadData.GetRangedAccuracyData(squad.squadType, false)).ToString() + "%\n");
        }

        stringBuilder.Append("\n");

        stringBuilder.Append("<b>Leader Stats:</b>\n");
        stringBuilder.Append("Health: " + (leaderHealth.GetMaxHealth() + squadData.GetHealthData(squad.squadType, true)).ToString() + "\n");

        if (leader != null && (leader.bluntDamage > 0f || leader.slashDamage > 0f || leader.piercingDamage > 0f || leader.fireDamage > 0f))
        {
            if (leader.bluntDamage > 0)
                stringBuilder.Append("Melee <i>Blunt</i> Damage: " + (leader.bluntDamage + squadData.GetBluntDamageData(squad.squadType, true)).ToString() + "\n");

            if (leader.slashDamage > 0)
                stringBuilder.Append("Melee <i>Slash</i> Damage: " + (leader.slashDamage + squadData.GetSlashDamageData(squad.squadType, true)).ToString() + "\n");

            if (leader.piercingDamage > 0)
                stringBuilder.Append("Melee <i>Piercing</i> Damage: " + (leader.piercingDamage + squadData.GetPiercingDamageData(squad.squadType, true)).ToString() + "\n");

            if (leader.fireDamage > 0)
                stringBuilder.Append("Melee <i>Fire</i> Damage: " + (leader.fireDamage + squadData.GetFireDamageData(squad.squadType, true)).ToString() + "\n");
        }

        if (leaderShooter != null && (leaderShooter.bluntDamage > 0f || leaderShooter.piercingDamage > 0f || leaderShooter.fireDamage > 0f))
        {
            if (leaderShooter.bluntDamage > 0f)
                stringBuilder.Append("Ranged <i>Blunt</i> Damage: " + (leaderShooter.bluntDamage + squadData.GetRangedBluntDamageData(squad.squadType, true)).ToString() + "\n");

            if (leaderShooter.piercingDamage > 0f)
                stringBuilder.Append("Ranged <i>Piercing</i> Damage: " + (leaderShooter.piercingDamage + squadData.GetRangedPiercingDamageData(squad.squadType, true)).ToString() + "\n");

            if (leaderShooter.fireDamage > 0f)
                stringBuilder.Append("Ranged <i>Fire</i> Damage: " + (leaderShooter.fireDamage + squadData.GetRangedFireDamageData(squad.squadType, true)).ToString() + "\n");

            stringBuilder.Append("Ranged Accuracy: " + (leaderShooter.accuracy + squadData.GetRangedAccuracyData(squad.squadType, true)).ToString() + "%\n");
        }

        tooltipText.text = stringBuilder.ToString();

        SetBackgroundSize();
        transform.position = pos + new Vector2(-0.15f, 0.5f);
    }

    public void ActivateAbilityTooltip(string abilityName, Vector2 pos)
    {
        tooltipText.gameObject.SetActive(true);
        CreateAbilityTooltip(abilityName, pos);
    }

    public void DeactivateTooltip()
    {
        tooltipBackground.gameObject.SetActive(false);
        tooltipText.gameObject.SetActive(false);
        tooltipText.alignment = TextAnchor.LowerLeft;
    }

    void CreateAbilityTooltip(string abilityName, Vector2 pos)
    {
        tooltipText.text = abilityName;

        SetBackgroundSize();
        if (abilityIconController.selectedSquad == null || abilityIconController.selectedSquad.isCastleWallSquad == false)
        {
            transform.position = pos + new Vector2(-1.1f, -0.05f);
            tooltipText.alignment = TextAnchor.MiddleRight;
        }
        else
            transform.position = pos + new Vector2(0.75f, 0.05f); // For castle wall squads
    }

    void SetBackgroundSize()
    {
        backgroundSize = new Vector2(tooltipText.preferredWidth + (textPaddingSize * 2f), tooltipText.preferredHeight + (textPaddingSize * 2f));
        if (backgroundSize.x > 240f) backgroundSize.x = 240f + (textPaddingSize * 2f);
        tooltipBackground.sizeDelta = backgroundSize;
    }
}
