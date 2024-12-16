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
    public int attackDamage = 10;        // 공격 데미지
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
    private Player player;

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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

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
                    audioSource.PlayOneShot(attackSFX);
                    Attack();
                    yield return new WaitForSeconds(attackcoolTime);
                    break;
            }
        }
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
        player.Damaged(attackDamage);
    }

    // 피격
    public void Hit(float damage)
    {
        hp -= damage;
        animator.SetTrigger("Hit");
        bloodEffect.Play();

        if (hp < 1f)
        {
            // 사망 
            StopCoroutine(zombieAiCo);

            zombieState = ZombieState.DEAD;
            agent.isStopped = true;
            animator.SetTrigger("Die");

            Invoke(nameof(Initialization), 4f);
        }
    }

    private void Initialization()
    {
        gameObject.SetActive(false);
        zombieState = ZombieState.PATROL;
    }
}
