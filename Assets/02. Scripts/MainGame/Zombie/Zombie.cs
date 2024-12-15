using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("Zombie Stats")]
    public float health = 100f;
    public float speed = 3f;
    public int attackDamage = 5;
    public float attackCooldown = 5f;
    public float detectionRadius = 20f;
    public float attackRadius = 5f;

    [Header("Asset")]
    public AudioClip attackSFX;
    public AudioClip hitSFX;
    public AudioClip deathSFX;
    public GameObject bloodEffect;

    // components
    private NavMeshAgent navAgent;
    private Animator animator;
    private GameObject player;

    // values
    private bool isPlayerDetected = false;
    private bool isPlayerInAttackRange = false;
    private bool isDead = false;

    private float attackTimer = 0f;
    private bool isStunned = false;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        navAgent.speed = speed;
    }

    void Update()
    {
        if (isDead || isStunned) return;

        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        // �÷��̾� �ν�
        if (playerDistance <= detectionRadius)
        {
            isPlayerDetected = true;
            navAgent.SetDestination(player.transform.position);
            animator.SetBool("isWalking", true);
        }
        else
        {
            isPlayerDetected = false;
            navAgent.ResetPath();
            animator.SetBool("isWalking", false);
        }

        // ���� �Ÿ� üũ
        if (isPlayerDetected && playerDistance <= attackRadius)
        {
            isPlayerInAttackRange = true;
            navAgent.isStopped = true;

            if (attackTimer <= 0f)
            {
                AttackPlayer();
                attackTimer = attackCooldown;
            }
        }
        else
        {
            isPlayerInAttackRange = false;
            navAgent.isStopped = false;
        }

        // ���� ��ٿ� ����
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void AttackPlayer()
    {
        if (isDead || isStunned) return;

        // ���� �ִϸ��̼� ���
        animator.SetTrigger("Attack");

        // ���� SFX ���
        if (attackSFX)
        {
            AudioSource.PlayClipAtPoint(attackSFX, transform.position);
        }

        // �÷��̾�� ������ ����
        //player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        // �ǰ� ȿ��
        health -= damage;
        if (bloodEffect)
        {
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
        }

        if (hitSFX)
        {
            AudioSource.PlayClipAtPoint(hitSFX, transform.position);
        }

        animator.SetTrigger("Hit");

        // ���� üũ
        if (health <= 0f)
        {
            Die();
        }
    }

    public void TakeStunDamage(float damage)
    {
        TakeDamage(damage);
        if (!isDead)
        {
            StartCoroutine(StunCoroutine());
        }
    }

    private IEnumerator StunCoroutine()
    {
        isStunned = true;
        navAgent.isStopped = true;
        yield return new WaitForSeconds(3f);
        isStunned = false;
        navAgent.isStopped = false;
    }

    void Die()
    {
        isDead = true;
        navAgent.isStopped = true;

        // ��� �ִϸ��̼�
        animator.SetTrigger("Die");

        // ��� SFX ���
        if (deathSFX)
        {
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
        }

        // 3�� �� Destroy
        Destroy(gameObject, 3f);
    }
}
