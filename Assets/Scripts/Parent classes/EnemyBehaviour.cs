using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    //hit points, movespeed, attackdelay, effectiverange
    public int hitPoints;

    public float moveSpeed;
    public float attackDelay;
    public float attackRecharge;
    public float effectiveRange;
    public float detectionRange;
    

    public GameObject Player;
    protected Rigidbody2D enemyRb;
    public GameManager gameManager;

    protected Vector3 knownPlayerLocation;

    protected bool knowsPlayerLocation;
    protected bool canBeHit = true;
    protected bool isAttacking = false;//prevents movement and by extension attacking before the current attack is done

    protected int layerMask;


    // Start is called before the first frame update
    void Start()
    {
        StartOperations();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FixedUpdateOperations();
    }

    //Look for the player. If seen, save their location to move to
    protected void FindPlayer()
    {

        Vector2 directionTowardPlayer = (Player.transform.position - gameObject.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionTowardPlayer, detectionRange, layerMask);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("PlayerTag"))
            {
                knownPlayerLocation = hit.transform.position;
                knowsPlayerLocation = true;
            }
        }

    }

    public virtual void MoveRelativeToPlayer()
    {
        enemyRb.MovePosition(transform.position + ((knownPlayerLocation - transform.position).normalized * moveSpeed * Time.deltaTime * gameManager.timeFlow));
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack") && canBeHit)
        {
            canBeHit = false;
            hitPoints -= 1;
            if (hitPoints <= 0)
            {
                //Die - can add stuff here like animation, loot drops
                Destroy(this.gameObject);
            }

            StartCoroutine(beHitCooldown());
        }
    }

    protected IEnumerator beHitCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        canBeHit = true;
    }

    protected void StartOperations()
    {
        Player = GameObject.FindWithTag("PlayerTag");
        knownPlayerLocation = transform.position;//Doesn't know where player is straight away, so stays where it is
        enemyRb = gameObject.GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        layerMask = ~(LayerMask.GetMask("Enemy"));
    }

    protected void FixedUpdateOperations()
    {
        FindPlayer();

        if (knowsPlayerLocation && !isAttacking)
        {
            if (Vector3.Distance(gameObject.transform.position, knownPlayerLocation) > effectiveRange)
            {
                MoveRelativeToPlayer();
            }

            else
            {
                //Check player still there
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.transform.position - gameObject.transform.position, effectiveRange, layerMask);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.CompareTag("PlayerTag"))
                    {
                        //AttackPlayer();
                        StartCoroutine(AttackDelays(attackDelay,attackRecharge));
                    }

                }

                else//Enemy has lost track of player
                {
                    knowsPlayerLocation = false;
                }
            }
        }
    }

    protected IEnumerator AttackDelays(float timeToDelay, float timeToRecharge)
    {
        if (!isAttacking)
        {
            isAttacking = true;

            Vector3 directionOfPlayer = (Player.transform.position - transform.position);
            float angleRad = Mathf.Atan2(directionOfPlayer.y, directionOfPlayer.x);
            float angleDeg = Mathf.Rad2Deg * angleRad;
            Quaternion attackRotation = Quaternion.Euler(0, 0, angleDeg - 90);

            yield return new WaitForSeconds(timeToDelay);
            AttackContents(directionOfPlayer, attackRotation);
            yield return new WaitForSeconds(timeToRecharge);
            isAttacking = false;
        }

    }

    protected virtual void AttackContents(Vector3 directionOfPlayer, Quaternion attackRotation)
    {
        Debug.Log("No attack created in override AttackContents. Put something here!");
    }
}

