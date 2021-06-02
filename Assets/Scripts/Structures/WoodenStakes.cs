using UnityEngine;

public class WoodenStakes : Structure
{
    [Header("Animators")]
    public Animator[] stakeAnims_Group1;
    public Animator[] stakeAnims_Group2;

    [Header("Sprite Renderers")]
    public SpriteRenderer[] stakeSpriteRenderers_Group1;
    public SpriteRenderer[] stakeSpriteRenderers_Group2;

    [Header("Box Colliders")]
    public BoxCollider2D boxCollider_Group1;
    public BoxCollider2D boxCollider_Group2;

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
            stakeSpriteRenderers_Group1[i].sortingOrder = Mathf.RoundToInt((transform.position.y * -100) + 10);
        }

        for (int i = 0; i < stakeSpriteRenderers_Group2.Length; i++)
        {
            stakeSpriteRenderers_Group2[i].sortingOrder = Mathf.RoundToInt((transform.position.y * -100) + 9);
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
            stakeAnims_Group2[i].SetBool("isDestroyed", false);
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
        if (currentStructureCount == 1)
        {
            for (int i = 0; i < stakeSpriteRenderers_Group1.Length; i++)
            {
                boxCollider_Group1.enabled = false;
                stakeAnims_Group1[i].SetBool("isDestroyed", true);

                audioManager.PlayRandomSound(audioManager.woodBreakingSounds);

                if (destroyAnimationTime == 0f)
                    destroyAnimationTime = stakeAnims_Group1[i].GetCurrentAnimatorClipInfo(0).Length;
            }
        }
        else if (currentStructureCount == 2)
        {
            for (int i = 0; i < stakeSpriteRenderers_Group2.Length; i++)
            {
                boxCollider_Group2.enabled = false;
                stakeAnims_Group2[i].SetBool("isDestroyed", true);

                audioManager.PlayRandomSound(audioManager.woodBreakingSounds);

                if (destroyAnimationTime == 0f)
                    destroyAnimationTime = stakeAnims_Group2[i].GetCurrentAnimatorClipInfo(0).Length;
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
