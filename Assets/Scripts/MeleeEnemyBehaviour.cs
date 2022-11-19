using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyBehaviour : EnemyBehaviour
{
    public GameObject meleeProjectile;
    public float attackMovement;
    public float meleeAttackRange;

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
        transform.Translate(directionOfPlayer.normalized * attackMovement);
        Destroy(Instantiate(meleeProjectile, transform.position + (directionOfPlayer.normalized * meleeAttackRange), attackRotation), 0.1f);
    }


}
