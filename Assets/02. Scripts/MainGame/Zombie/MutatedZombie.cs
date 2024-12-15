using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MutatedZombie : MonoBehaviour
{
    [Header("State")]
    public ZombieState zombieState;
    public enum ZombieState
    {
        PATROL,
        TRACE,
        ATTACK,
        DEAD
    }

    [Header("Zombie")]
    public float hp = 150f;
    public float walkSpeed = 1f;
    public float runSpeed = 2f;
    public float attackDamage = 10f;     // 공격 데미지
    public float attackcoolTime = 3f;    // 공격 쿨타임
    public float detectionRadius = 20f;  // 추적 범위
    public float attackRadius = 2.5f;      // 공격 범위

    [Header("Asset")]
    public AudioClip findSFX;
    public AudioClip attackSFX;
    public ParticleSystem bloodEffect;

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    private Transform playerPos;

    // 순찰
    private GameObject posOb;
    private List<Vector3> patrolPos = new List<Vector3>();

    private Coroutine zombieAiCo;


    private void Start()
    {
        // 컴포넌트, 오브젝트 찾기
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // 순찰 지점
        posOb = GameObject.Find("AIPos");
        SetAiPatrolPos();

        // AI 시작
        zombieAiCo = StartCoroutine(ZombieAI());
        agent.speed = this.runSpeed;
    }

    // 변이 좀비 행동 패턴 ★
    private IEnumerator ZombieAI()
    {
        WaitForSeconds ws = new WaitForSeconds(0.7f);

        while (zombieState != ZombieState.DEAD)
        {
            yield return ws;  // 간격

            // State
            float currentDist = (playerPos.position - transform.position).magnitude;

            if (currentDist < attackRadius)
                zombieState = ZombieState.ATTACK;
            else if (currentDist < detectionRadius)
                zombieState = ZombieState.TRACE;
            else
                zombieState = ZombieState.PATROL;

            // 살아있을 떄 행동
            switch (zombieState)
            {
                case ZombieState.PATROL:      // 1. 순찰
                    agent.isStopped = false;
                    agent.SetDestination(GetAiPatrolPos());
                    animator.SetBool("Run", false);
                    break;
                case ZombieState.TRACE:     // 2. 추적
                    agent.isStopped = false;
                    agent.SetDestination(playerPos.position);
                    animator.SetBool("Run", true);
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

    // 순찰 지점 set
    private void SetAiPatrolPos()
    {
        foreach (Transform child in posOb.transform)
        {
            patrolPos.Add(child.position);
        }
    }

    // 순찰 지점 get
    private Vector3 GetAiPatrolPos()
    {
        int randomIndex = Random.Range(0, patrolPos.Count);
        return patrolPos[randomIndex];
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

        if (hp < 0.001f)
        {
            zombieState = ZombieState.DEAD;
        }
    }
}
