using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PhantomBoss : MonoBehaviour
{
    public Game game;

    public Rigidbody2D rb;
    public Collider2D col;
    public SpriteRenderer sprite;
    public Animator anim;

    public GameObject player;
    Transform playerTransform;

    public GameObject weapon;
    SpriteRenderer weaponSprite;
    Collider2D weaponCol;

    public Vector3[] warpPoints;
    float warpTime;

    int hp;

    Vector3 attackDir;
    float moveSpeed;
    float moveDeceleration;
    float attackCD;
    bool isAttacking;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.GetComponent<Transform>();

        weaponSprite = weapon.GetComponent<SpriteRenderer>();
        weaponCol = weapon.GetComponent<Collider2D>();

        hp = 15;

        warpTime = 0f;
        moveSpeed = 0f;
        moveDeceleration = 5f;
        attackCD = 0f;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        // attack random spot every 10 seconds
        warpTime += Time.deltaTime;
        if (warpTime > 10f)
        {
            AttackRandom();
        }

        // attack player every 4 seconds
        attackCD += Time.deltaTime;
        if (attackCD > 4f)
        {
            AttackPlayer();
        }

        // attack in forward dash
        if (isAttacking)
        {
            moveSpeed -= moveSpeed * moveDeceleration * Time.deltaTime;
            rb.velocity = attackDir * moveSpeed;

            // stop attacking
            if (moveSpeed < 1f)
            {
                StopAttacking();
            }
        }

        if (hp <= 0)
        {
            Die();
        }
    }

    void AttackRandom()
    {
        //choose random spot to attack
        attackDir = warpPoints[UnityEngine.Random.Range(0, 4)] - transform.position;
        transform.up = attackDir;

        moveSpeed = 5f;
        isAttacking = true;
        warpTime = 0f;
        attackCD = 0f;


        weaponSprite.enabled = true;
        weaponCol.enabled = true;
        anim.SetTrigger("Attack");
    }

    void AttackPlayer()
    {
        // attack in direction of player
        attackDir = playerTransform.position - transform.position;
        transform.up = attackDir;
        moveSpeed = 3f;

        isAttacking = true;
        attackCD = 0f;

        weaponSprite.enabled = true;
        weaponCol.enabled = true;
        anim.SetTrigger("Attack");
    }
    void StopAttacking()
    {
        rb.velocity = Vector2.zero;
        Debug.Log("BOSS 1 STOPPED ATTACKING");
        isAttacking = false;

        weaponSprite.enabled = false;
        weaponCol.enabled = false;
        anim.SetTrigger("Idle");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.tag == "Weapon")
        {
            StartCoroutine(TakeDamage());
        }
    }

    private IEnumerator TakeDamage()
    {
        hp--;
        sprite.color = Color.red;
        yield return new WaitForSeconds(1f);
        sprite.color = Color.white;
    }

    void Die()
    {
        Debug.Log("BOSS DEFEATED");
        game.EndLevel("YOU WIN");
        gameObject.SetActive(false);
    }
}
