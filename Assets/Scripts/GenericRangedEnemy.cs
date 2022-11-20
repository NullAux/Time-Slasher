using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericRangedEnemy : EnemyBehaviour
{
    public GameObject rangedProjectile;

    public float projectileVelocity;
    public float projectileExpiryTime;

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
        GameObject projectile = Instantiate(rangedProjectile, transform.position + directionOfPlayer.normalized, attackRotation, gameObject.transform);
        projectile.GetComponentInChildren<Rigidbody2D>().velocity = directionOfPlayer.normalized * projectileVelocity;
        Destroy(projectile, projectileExpiryTime);

    }
}
