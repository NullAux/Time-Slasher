using System.Collections;
using UnityEngine;

public class MeleeEnemyBehaviour : EnemyBehaviour
{
    public GameObject meleeProjectile;
    public float meleeAttackRange;
    public float lungeCompletionTime;
    public float attackVelocity;

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
        StartCoroutine(LungeCounter(lungeCompletionTime / gameManager.timeFlow, directionOfPlayer, attackRotation));
    }

    //The forward motion associtaed with a melee attack. Has its own IEnumerator so it can be drawn out if it happens during time slow.
    protected virtual IEnumerator LungeCounter(float relativeTime, Vector3 directionOfPlayer, Quaternion attackRotation)
    {

        enemyRb.velocity = directionOfPlayer * attackVelocity * gameManager.timeFlow;
        yield return new WaitForSeconds(lungeCompletionTime / gameManager.timeFlow);
        enemyRb.velocity = new Vector2(0, 0);
        Destroy(Instantiate(meleeProjectile, transform.position + (directionOfPlayer.normalized * meleeAttackRange), attackRotation, gameObject.transform), 0.1f);

    }


}
