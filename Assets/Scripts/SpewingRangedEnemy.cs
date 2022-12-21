using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpewingRangedEnemy : GenericRangedEnemy
{
    public int numberOfBulletPerSpew;
    public float delayBetweenSpewBullets;


    public float randomnessWeighting;
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
        StartCoroutine(DelayBetweenSpewShots(delayBetweenSpewBullets, directionOfPlayer,attackRotation));
    }

    protected IEnumerator DelayBetweenSpewShots(float delay, Vector3 directionOfPlayer, Quaternion attackRotation)
    {
        for (int i = 0; i < numberOfBulletPerSpew; i++)
        {
            float randomOffsetX = Random.Range(-100, 100);
            float randomOffsetY = Random.Range(-100, 100);
            Vector3 randomAngle = new Vector3(randomOffsetX, randomOffsetY, 0).normalized * randomnessWeighting;

            /*
            float angleRad = Mathf.Atan2(directionOfPlayer.y, directionOfPlayer.x);
            float angleDeg = Mathf.Rad2Deg * angleRad;
            Quaternion RandomizedAttackRotation = Quaternion.Euler(0, 0, angleDeg - 90);
            */

            Quaternion randomizedAttackDirection = Quaternion.Euler(randomOffsetX, randomOffsetY, 0).normalized;

            GameObject projectile = Instantiate(rangedProjectile, transform.position + directionOfPlayer.normalized, attackRotation * randomizedAttackDirection, gameObject.transform);
            //projectile.GetComponentInChildren<Rigidbody2D>().rotation = (directionOfPlayer.normalized + randomAngle);

            //projectile.GetComponent<ProjectileBehaviour>().RandomRotate(randomAngle);
            projectile.GetComponentInChildren<ProjectileBehaviour>().projectileSpeed = projectileVelocity;
            Destroy(projectile, projectileExpiryTime);

            yield return new WaitForSeconds(delay);


        }
    }
}
