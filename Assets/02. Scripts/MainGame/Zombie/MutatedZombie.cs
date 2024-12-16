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
    public int attackDamage = 10;        // ���� ������
    public float attackcoolTime = 3f;    // ���� ��Ÿ��
    public float detectionRadius = 20f;  // ���� ����
    public float attackRadius = 2.5f;      // ���� ����

    [Header("Asset")]
    public AudioClip findSFX;
    public AudioClip attackSFX;
    public ParticleSystem bloodEffect;

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audioSource;
    private Transform playerPos;
    private Player player;

    // ����
    private GameObject posOb;
    private List<Vector3> patrolPos = new List<Vector3>();

    private Coroutine zombieAiCo;


    private void Start()
    {
        // ������Ʈ, ������Ʈ ã��
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // ���� ����
        posOb = GameObject.Find("AIPos");
        SetAiPatrolPos();

        // AI ����
        zombieAiCo = StartCoroutine(ZombieAI());
        agent.speed = this.runSpeed;
    }

    // ���� ���� �ൿ ���� ��
    private IEnumerator ZombieAI()
    {
        WaitForSeconds ws = new WaitForSeconds(0.7f);

        while (zombieState != ZombieState.DEAD)
        {
            yield return ws;  // ����

            // State
            float currentDist = (playerPos.position - transform.position).magnitude;

            if (currentDist < attackRadius)
                zombieState = ZombieState.ATTACK;
            else if (currentDist < detectionRadius)
                zombieState = ZombieState.TRACE;
            else
                zombieState = ZombieState.PATROL;

            // ������� �� �ൿ
            switch (zombieState)
            {
                case ZombieState.PATROL:      // 1. ����
                    agent.isStopped = false;
                    agent.SetDestination(GetAiPatrolPos());
                    animator.SetBool("Run", false);
                    break;
                case ZombieState.TRACE:     // 2. ����
                    agent.isStopped = false;
                    agent.SetDestination(playerPos.position);
                    animator.SetBool("Run", true);
                    break;
                case ZombieState.ATTACK:    // 3. ����
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

    // ���� ���� set
    private void SetAiPatrolPos()
    {
        foreach (Transform child in posOb.transform)
        {
            patrolPos.Add(child.position);
        }
    }

    // ���� ���� get
    private Vector3 GetAiPatrolPos()
    {
        int randomIndex = Random.Range(0, patrolPos.Count);
        return patrolPos[randomIndex];
    }

    // ����
    private void Attack()
    {
        player.Damaged(attackDamage);
    }

    // �ǰ�
    public void Hit(float damage)
    {
        hp -= damage;
        animator.SetTrigger("Hit");
        bloodEffect.Play();

        if (hp < 1f)
        {
            // ��� 
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
