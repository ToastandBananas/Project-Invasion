using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public int abilityGoldCost;
    [HideInInspector] public int abilitySuppliesCost;

    Tooltip tooltip;
    Image image;

    void Start()
    {
        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
        image = GetComponent<Image>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        tooltip.ActivateAbilityTooltip(image.sprite.name, transform.position);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.DeactivateTooltip();
    }
}
