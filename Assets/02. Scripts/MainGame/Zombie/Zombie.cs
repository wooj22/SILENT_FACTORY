using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("State")]
    public ZombieState zombieState;
    public enum ZombieState
    {
        IDLE,
        TRACE,
        ATTACK,
        DEAD
    }

    [Header("Zombie")]
    public float hp = 100f;
    public float speed = 1f;            
    public int attackDamage = 5;        // 공격 데미지
    public float attackcoolTime = 5f;   // 공격 쿨타임
    public float detectionRadius = 15f; // 추적 범위
    public float attackRadius = 2.5f;     // 공격 범위

    [Header("Asset")]
    public AudioClip attackSFX;
    public ParticleSystem bloodEffect;

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    private Transform playerPos;
    private Player player;

    private Coroutine zombieAiCo;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        zombieAiCo = StartCoroutine(ZombieAI());
        agent.speed = this.speed;
    }

    // 좀비 행동 패턴 
    private IEnumerator ZombieAI()
    {
        WaitForSeconds ws = new WaitForSeconds(0.7f);
        bool isTracking = false;   // 추적상태 유지 제어 변수(한번 인식하면 소리를 안내도 추적 유지)

        while (zombieState != ZombieState.DEAD)
        {
            yield return ws;  // 간격

            // State Set★
            float currentDist = (playerPos.position - transform.position).magnitude;

            if (currentDist < attackRadius)
                zombieState = ZombieState.ATTACK;
            else if (currentDist < detectionRadius)
            {
                if (player.currentMovementState == Player.MovementState.Running ||
                    player.currentMovementState == Player.MovementState.Walking)
                {
                    zombieState = ZombieState.TRACE;
                    isTracking = true;
                }
                else if (isTracking)
                {
                    zombieState = ZombieState.TRACE;
                }
            }
            else
            {
                zombieState = ZombieState.IDLE;
                isTracking = false;
            }

            // 행동
            switch (zombieState)
            {
                case ZombieState.IDLE:      // 1. 기본
                    agent.isStopped = true;
                    animator.SetBool("Walk", false);
                    break;
                case ZombieState.TRACE:     // 2. 추적
                    agent.isStopped = false;
                    agent.SetDestination(playerPos.position);
                    animator.SetBool("Walk", true);
                    break;
                case ZombieState.ATTACK:    // 3. 공격
                    agent.isStopped = true;
                    agent.SetDestination(playerPos.position);
                    animator.SetTrigger("Attack");
                    audioSource.PlayOneShot(attackSFX);
                    Attack();
                    yield return new WaitForSeconds(attackcoolTime);
                    break; 
            }
        }

        // 사망 및 초기화
        agent.isStopped = true;
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(4f);

        gameObject.SetActive(false);
        zombieState = ZombieState.IDLE;

        yield return null;
    }

    // 공격
    private void Attack()
    {
        player.Damaged(attackDamage);
    }

    // 피격
    public void Hit(float damage)
    {
        hp -= damage;
        animator.SetTrigger("Hit");
        bloodEffect.Play();

        if(hp < 1f)
        {
            zombieState = ZombieState.DEAD;
        }
    }
}
