using UnityEngine;

public class Gravestone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        Attacker attacker = collision.GetComponent<Attacker>();

        if (attacker)
        {
            // Do some sort of animation...
        }
    }
}
