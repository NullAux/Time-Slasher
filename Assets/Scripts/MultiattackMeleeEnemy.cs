using System.Collections;
using UnityEngine;

public class MultiattackMeleeEnemy : MeleeEnemyBehaviour
{

    public float delayBetweenAttacks;
    
    public int numberOfAttacks;


    // Start is called before the first frame update
    void Start()
    {
        StartOperations();
    }

    // Update is called once per frame
    void Update()
    {
        FixedUpdateOperations();
    }

    protected override void AttackContents(Vector3 directionOfPlayer, Quaternion attackRotation)
    {
        //transform.Translate(directionOfPlayer.normalized * attackMovement);
        StartCoroutine(LungeCounter(lungeCompletionTime / gameManager.timeFlow, directionOfPlayer, attackRotation));
    }

    //The forward motion associtaed with a melee attack. Has its own IEnumerator so it can be drawn out if it happens during time slow.
    protected override IEnumerator LungeCounter(float relativeTime, Vector3 directionOfPlayer, Quaternion attackRotation)
    {
        for (int i = 0; i < numberOfAttacks; i++)
        {
            enemyRb.velocity = directionOfPlayer * attackVelocity * gameManager.timeFlow;
            yield return new WaitForSeconds(lungeCompletionTime / gameManager.timeFlow);
            enemyRb.velocity = new Vector2(0, 0);
            Destroy(Instantiate(meleeProjectile, transform.position + (directionOfPlayer.normalized * meleeAttackRange), attackRotation), 0.1f);

            yield return new WaitForSeconds(delayBetweenAttacks / gameManager.timeFlow);
        }

    }


}

