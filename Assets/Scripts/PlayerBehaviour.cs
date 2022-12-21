using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    public float moveSpeed;
    public float attackRange;
    public float attackDelay;
    public float dodgeDistance;
    public float dodgeInvulnerabilityTime;
    public float dodgeDelay;
    public float invincibilityTime;
    public float timeResource;
    public float timeResourceDrainRate;
    public float timeResourceRechargeRate;
    public float timeResourceRechargeDelay;
    public float timeSlowEfficacy;//How much time is slowed, 0-1 where 1 is unchanged.
    public float timeSinceSlowTime;

    public int hitPoints;

    bool canAttack = true;
    bool canDodge = true;
    public bool canBeHit = true;
    bool canRechargeTimeResource = true;

    Rigidbody2D playerRb;
    SpriteRenderer m_SpriteRenderer;

    public GameObject swordAttack;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && canAttack)//On left click
        {
            Attack();
        }

        if (Input.GetButtonDown("Jump") && canDodge)//Jump is set to space by default in input manager
        {
            Dodge();
        }

        if (Input.GetButton("Fire2") && (timeResource > 1))//Fire2 is default right mouse button (mouse1) on keyboard
        {
            SlowTime();
        }

        if (Input.GetMouseButtonUp(1))
        {
            EndSlowTime();
        }

        if ((Time.time - timeSinceSlowTime > timeResourceRechargeDelay) && (timeResource < 100) && canRechargeTimeResource)
        {
            timeResource += timeResourceRechargeRate * Time.deltaTime;
        }

        gameManager.timeResourceBar.GetComponent<Slider>().value = timeResource / 100;//Updates the UI element showing timeResource
    }

    private void FixedUpdate()
	{
        Move();
    }



    //Takes the player's directional inputs, and moves them correspondingly
    void Move()
    {
        Vector3 movementInput = new Vector3 (Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        playerRb.MovePosition(transform.position + (movementInput * Time.deltaTime * moveSpeed));
    }

    //Instantiates the attack (containing animation and hitbox), and then removes it
    void Attack()
    {
        canAttack = false;

        Vector3 screenMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angleRad = Mathf.Atan2(screenMousePosition.y - transform.position.y, screenMousePosition.x - transform.position.x);
        float angleDeg = Mathf.Rad2Deg *angleRad;

        transform.rotation = Quaternion.Euler(0, 0, angleDeg - 90);
         
        Destroy(Instantiate(swordAttack, transform.position + (attackRange * transform.up), transform.rotation),0.1f);//number at end is how long it stays in scene

        StartCoroutine(AttackCooldown(attackDelay));
    }
    IEnumerator AttackCooldown(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        canAttack = true;
    }

    //Moves the player slightly in the direction they're moving, giving invulnerability for a short period
    void Dodge()
    {
        canDodge = false;
        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.Translate(movementInput.normalized * dodgeDistance, Space.World);
        StartCoroutine(DodgeCooldown(dodgeDelay));
    }

    IEnumerator DodgeCooldown(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        canDodge = true;
        //Can add an animation here, to show when able to dodge again
    }

    //Damage the player. Public method to be called by whatever is doing the damage.
    //Check PlayerBehaviour.canBeHit == true before calling this method.
    public void DamagePlayer(int damage)
    {
        canBeHit = false;
        hitPoints -= damage;
        m_SpriteRenderer.color = new Color(0.5f, 0, 0);
        StartCoroutine(beHitCooldown(invincibilityTime));
    }

    IEnumerator beHitCooldown(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        m_SpriteRenderer.color = Color.red;
        canBeHit = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttack") && canBeHit)
        {
            DamagePlayer(collision.gameObject.transform.parent.root.gameObject.GetComponent<EnemyBehaviour>().attackDamage);
        }
    }

    //Temporarily slow down the flow of time. Active until player runs out of time resource or releases button. Delay on recharge after use.
    void SlowTime()
    {
        canRechargeTimeResource = false;
        gameManager.timeSlowOverlay.SetActive(true);
        gameManager.timeFlow = timeSlowEfficacy;
        timeResource -= timeResourceDrainRate * Time.deltaTime;

        if (timeResource < 1)
        {
            EndSlowTime();
        }
    }

    //Resets time flow, and tracks time since end (to begin recharging). Called when player releases button, or runs out of resource.
    void EndSlowTime()
    {
        gameManager.timeSlowOverlay.SetActive(false) ;
        timeSinceSlowTime = Time.time;
        gameManager.timeFlow = 1;
        canRechargeTimeResource = true;//Still requires the delay to be passed. If in Update() checks both.
    }

}
