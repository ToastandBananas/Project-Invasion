using System.Collections.Generic;
using UnityEngine;

public class CastleCollider : MonoBehaviour
{
    CastleHealth castleHealth;
    //Transform wallsParent;
    //List<SpriteRenderer> castleWallSpriteRenderers = new List<SpriteRenderer>();

    //Color defaultColor = new Color(1f, 1f, 1f);
    //Color highlightColor = new Color(0.8f, 0.8f, 1f);

    #region Singleton
    public static CastleCollider instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    void Start()
    {
        castleHealth = CastleHealth.instance;
        /*wallsParent = transform.parent.Find("Walls");
        for (int i = 0; i < wallsParent.childCount; i++)
        {
            castleWallSpriteRenderers.Add(wallsParent.GetChild(i).GetComponent<SpriteRenderer>());
        }*/
    }

    /*void OnMouseEnter()
    {
        foreach(SpriteRenderer sr in castleWallSpriteRenderers)
        {
            sr.color = highlightColor;
        }
    }

    void OnMouseExit()
    {
        foreach (SpriteRenderer sr in castleWallSpriteRenderers)
        {
            sr.color = defaultColor;
        }
    }

    void OnMouseDown()
    {
        // Pop up wall defender menu
        if (DefenderSpawner.instance.ghostImageSquad == null)
        {

        }
    }*/

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        otherCollider.TryGetComponent<Attacker>(out Attacker attacker);
        if (attacker != null)
        {
            attacker.isAttackingCastle = true;
            attacker.Attack();
            return;
        }

        otherCollider.TryGetComponent<RangeCollider>(out RangeCollider rangeCollider);
        if (rangeCollider != null)
        {
            rangeCollider.transform.parent.TryGetComponent<Shooter>(out Shooter rangedAttacker);
            if (rangedAttacker != null)
                rangedAttacker.isShootingCastle = true;
        }
    }
}
