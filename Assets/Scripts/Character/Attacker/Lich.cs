using System.Collections;
using UnityEngine;

public class Lich : MonoBehaviour
{
    [SerializeField] Attacker skeleton;
    [SerializeField] Attacker skeletonWarrior;

    [SerializeField] float delayMin = 10f;
    [SerializeField] float delayMax = 20f;
    float raiseUndeadDelay;
    Vector2 currentTilePos;

    Animator anim;
    Attacker attackerScript;
    Health health;
    int random;

    IEnumerator Start()
    {
        anim = GetComponent<Animator>();
        attackerScript = GetComponent<Attacker>();
        health = GetComponent<Health>();

        while (true)
        {
            raiseUndeadDelay = Random.Range(delayMin, delayMax);

            yield return new WaitForSeconds(raiseUndeadDelay);

            anim.SetBool("isSummoning", true);
            currentTilePos = new Vector2(Mathf.Floor(transform.position.x), Mathf.Floor(attackerScript.myAttackerSpawner.transform.position.y));
        }
    }

    void Update()
    {
        if (health.isDead)
            this.enabled = false;
    }

    public void RaiseUndead()
    {
        random = Random.Range(0, 6);

        if (random == 0 || random == 1 || random == 2)
        {
            SpawnUndeadAttacker(skeleton, currentTilePos + new Vector2(Random.Range(-0.3f, -0.1f), Random.Range(-0.1f, 0.1f)));
            SpawnUndeadAttacker(skeleton, currentTilePos + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.3f, -0.1f)));
            SpawnUndeadAttacker(skeleton, currentTilePos + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(0.1f, 0.3f)));
        }
        else if (random == 3 || random == 4)
        {
            SpawnUndeadAttacker(skeletonWarrior, currentTilePos + new Vector2(Random.Range(-0.4f, -0.2f), Random.Range(0.1f, 0.3f)));
            SpawnUndeadAttacker(skeletonWarrior, currentTilePos + new Vector2(Random.Range(-0.4f, -0.2f), Random.Range(-0.3f, -0.1f)));
        }
        else if (random == 5)
        {
            SpawnUndeadAttacker(skeletonWarrior, currentTilePos + new Vector2(Random.Range(-0.3f, -0.1f), Random.Range(-0.1f, 0.1f)));
            SpawnUndeadAttacker(skeleton, currentTilePos + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.3f, -0.1f)));
            SpawnUndeadAttacker(skeleton, currentTilePos + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(0.1f, 0.3f)));
        }
    }

    public void StopSummoning()
    {
        anim.SetBool("isSummoning", false);
    }

    void SpawnUndeadAttacker(Attacker attackerToSpawn, Vector2 pos)
    {
        Attacker newAttacker = Instantiate(attackerToSpawn, pos, Quaternion.identity);
        newAttacker.myAttackerSpawner = attackerScript.myAttackerSpawner;
        newAttacker.transform.SetParent(newAttacker.myAttackerSpawner.transform);

        // Add to total attackers in level (so we can adjust the level's win condition)
        LevelController.instance.numberOfAttackers++;
    }
}
