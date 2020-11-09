using UnityEngine;

public class Attacker : MonoBehaviour
{
    float currentSpeed = 1f;
    
    void Update()
    {
        transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
    }

    public void SetMovementSpeed(float speed)
    {
        currentSpeed = speed;
    }
}
