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

        // 플레이어 인식
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

        // 공격 거리 체크
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

        // 공격 쿨다운 감소
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    void AttackPlayer()
    {
        if (isDead || isStunned) return;

        // 공격 애니메이션 재생
        animator.SetTrigger("Attack");

        // 공격 SFX 재생
        if (attackSFX)
        {
            AudioSource.PlayClipAtPoint(attackSFX, transform.position);
        }

        // 플레이어에게 데미지 전달
        //player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        // 피격 효과
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

        // 죽음 체크
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

        // 사망 애니메이션
        animator.SetTrigger("Die");

        // 사망 SFX 재생
        if (deathSFX)
        {
            AudioSource.PlayClipAtPoint(deathSFX, transform.position);
        }

        // 3초 뒤 Destroy
        Destroy(gameObject, 3f);
    }
}
