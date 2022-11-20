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



            GameObject projectile = Instantiate(rangedProjectile, transform.position + directionOfPlayer.normalized, attackRotation, gameObject.transform);
            projectile.GetComponentInChildren<Rigidbody2D>().velocity = (directionOfPlayer.normalized + randomAngle) * projectileVelocity;
            Destroy(projectile, projectileExpiryTime);

            yield return new WaitForSeconds(delay);


        }
    }
}
