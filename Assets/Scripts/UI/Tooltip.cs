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

    Defender leader;
    Defender unit;
    Health leaderHealth;
    Health unitHealth;
    Shooter leaderShooter;
    Shooter unitShooter;
    
    void Start()
    {
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
        if (unit.GetMeleeDamage() > 0)
            stringBuilder.Append("Melee Damage: " + (unit.GetMeleeDamage() + squadData.GetMeleeDamageData(squad.squadType, false)).ToString() + "\n");
        if (unitShooter != null)
        {
            stringBuilder.Append("Ranged Damage: " + (unitShooter.GetRangedDamage() + squadData.GetRangedDamageData(squad.squadType, false)).ToString() + "\n");
            stringBuilder.Append("Ranged Accuracy: " + (unitShooter.GetRangedAccuracy() + squadData.GetRangedAccuracyData(squad.squadType, false)).ToString() + "%\n");
        }

        stringBuilder.Append("\n");

        stringBuilder.Append("<b>Leader Stats:</b>\n");
        stringBuilder.Append("Health: " + (leaderHealth.GetMaxHealth() + squadData.GetHealthData(squad.squadType, true)).ToString() + "\n");
        if (leader.GetMeleeDamage() > 0)
            stringBuilder.Append("Melee Damage: " + (leader.GetMeleeDamage() + squadData.GetMeleeDamageData(squad.squadType, true)).ToString() + "\n");
        if (leaderShooter != null)
        {
            stringBuilder.Append("Ranged Damage: " + (leaderShooter.GetRangedDamage() + squadData.GetRangedDamageData(squad.squadType, true)).ToString() + "\n");
            stringBuilder.Append("Ranged Accuracy: " + (leaderShooter.GetRangedAccuracy() + squadData.GetRangedAccuracyData(squad.squadType, true)).ToString() + "%\n");
        }

        tooltipText.text = stringBuilder.ToString();

        backgroundSize = new Vector2(tooltipText.preferredWidth + (textPaddingSize * 2f), tooltipText.preferredHeight + (textPaddingSize * 2f));
        if (backgroundSize.x > 200f) backgroundSize.x = 200f + (textPaddingSize * 2f);
        tooltipBackground.sizeDelta = backgroundSize;

        transform.position = pos + new Vector2(0f, 0.5f);
    }
}
