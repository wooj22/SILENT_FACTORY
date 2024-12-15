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
    public float attackDamage = 5f;     // 공격 데미지
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

    private Coroutine zombieAiCo;
    private bool isHearPlayerSound;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        zombieAiCo = StartCoroutine(ZombieAI());
        agent.speed = this.speed;
    }

    // 좀비 행동 패턴 ★  ---> isHearPlayerSound를 활용한 발소리 시스템 적용 필요!!!
    private IEnumerator ZombieAI()
    {
        WaitForSeconds ws = new WaitForSeconds(0.7f);

        while (zombieState != ZombieState.DEAD)
        {
            yield return ws;  // 간격
            
            // State
            float currentDist = (playerPos.position - transform.position).magnitude;

            if (currentDist < attackRadius )
                zombieState = ZombieState.ATTACK;
            else if (currentDist < detectionRadius)
                zombieState = ZombieState.TRACE;
            else
                zombieState = ZombieState.IDLE;

            // 살아있을 떄 행동
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
                    break; 
            }
        }

        // 사망
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 4f);
        yield return null;
    }

    // 공격
    private void Attack()
    {
        // 공격 쿨타임에 계산
        // - 데미지 전달(플레이어 피격함수)
        // audioSource.PlayOneShot(attackSFX);
    }

    // 피격
    private void Hit(float damage)
    {
        hp -= damage;
        animator.SetTrigger("Hit");
        bloodEffect.Play();

        if(hp < 0.001f)
        {
            zombieState = ZombieState.DEAD;
        }
    }
}
