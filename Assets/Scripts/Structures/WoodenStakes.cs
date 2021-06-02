using UnityEngine;

public class WoodenStakes : Structure
{
    [Header("Sprite Renderers")]
    public SpriteRenderer[] stakeSpriteRenderers_Group1;
    public SpriteRenderer[] stakeSpriteRenderers_Group2;

    [Header("Box Colliders")]
    public BoxCollider2D boxCollider_Group1, boxCollider_Group2;

    void Awake()
    {
        canPlaceMore = true;

        if (stakeSpriteRenderers_Group1.Length == 0)
        {
            stakeSpriteRenderers_Group1 = transform.GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        }

        if (stakeSpriteRenderers_Group2.Length == 0)
        {
            stakeSpriteRenderers_Group2 = transform.GetChild(1).GetComponentsInChildren<SpriteRenderer>();
        }

        for (int i = 0; i < stakeSpriteRenderers_Group1.Length; i++)
        {
            stakeSpriteRenderers_Group1[i].sortingOrder = Mathf.RoundToInt(transform.position.y * -90);
        }

        for (int i = 0; i < stakeSpriteRenderers_Group2.Length; i++)
        {
            stakeSpriteRenderers_Group2[i].sortingOrder = Mathf.RoundToInt(transform.position.y * -89);
        }

        transform.GetChild(1).gameObject.SetActive(false);
    }

    public override void PlaceNewStructure(Vector2 coordinates)
    {
        base.PlaceNewStructure(coordinates);

        for (int i = 0; i < stakeSpriteRenderers_Group1.Length; i++)
        {
            stakeSpriteRenderers_Group1[i].color = activeColor;
        }

        boxCollider_Group1.enabled = true;
    }

    public override void BuildNextStructure()
    {
        base.BuildNextStructure();

        transform.GetChild(1).gameObject.SetActive(true);

        for (int i = 0; i < stakeSpriteRenderers_Group2.Length; i++)
        {
            stakeSpriteRenderers_Group2[i].color = activeColor;
        }

        boxCollider_Group2.enabled = true;
    }

    public override void ShowNextStructureGhostImage(ResourceDisplay currencyDisplay)
    {
        base.ShowNextStructureGhostImage(currencyDisplay);

        obstacles[1].gameObject.SetActive(true);

        if (currencyDisplay.HaveEnoughGold(GetGoldCost()) == false || currencyDisplay.HaveEnoughSupplies(GetSuppliesCost()) == false)
        {
            for (int i = 0; i < stakeSpriteRenderers_Group2.Length; i++)
            {
                stakeSpriteRenderers_Group2[i].color = invalidColor;
            }
        }
        else
        {
            for (int i = 0; i < stakeSpriteRenderers_Group2.Length; i++)
            {
                stakeSpriteRenderers_Group2[i].color = ghostImageColor;
            }
        }
    }

    public override void HideNextStructureGhostImage()
    {
        base.HideNextStructureGhostImage();
        
        if (currentStructureCount != maxStructureCount)
            obstacles[1].gameObject.SetActive(false);
    }

    public override void OnStructureDestroyed()
    {
        if (currentStructureCount == 2)
        {
            currentStructureCount--;
            canPlaceMore = true;

            for (int i = 0; i < stakeSpriteRenderers_Group2.Length; i++)
            {
                stakeSpriteRenderers_Group2[i].color = ghostImageColor;
                boxCollider_Group2.enabled = false;
            }
        }

        base.OnStructureDestroyed();
    }

    public override void SetGhostImageColor(Color color)
    {
        base.SetGhostImageColor(color);

        for (int i = 0; i < stakeSpriteRenderers_Group1.Length; i++)
        {
            stakeSpriteRenderers_Group1[i].color = color;
        }
    }
}
