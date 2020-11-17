using UnityEngine;

public class Fox : MonoBehaviour
{
    Attacker attackerScript;
    Animator anim;

    void Start()
    {
        attackerScript = GetComponent<Attacker>();
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject otherObject = collision.gameObject;

        // If this is a gravestone, jump over it
        if (otherObject.GetComponent<Gravestone>())
            anim.SetTrigger("jumpTrigger");
        else if (otherObject.GetComponent<Defender>())
            attackerScript.Attack(otherObject);
    }
}
