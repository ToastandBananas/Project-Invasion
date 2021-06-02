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
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        otherCollider.TryGetComponent(out Attacker attacker);
        if (attacker != null)
        {
            attacker.isAttackingCastle = true;
            attacker.Attack();
            return;
        }

        otherCollider.TryGetComponent(out RangeCollider rangeCollider);
        if (rangeCollider != null)
        {
            rangeCollider.transform.parent.TryGetComponent(out Shooter rangedAttacker);
            if (rangedAttacker != null)
                rangedAttacker.isShootingCastle = true;
        }
    }
}
