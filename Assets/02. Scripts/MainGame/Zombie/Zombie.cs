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
    public float attackDamage = 5f;     // ���� ������
    public float attackcoolTime = 5f;   // ���� ��Ÿ��
    public float detectionRadius = 15f; // ���� ����
    public float attackRadius = 2.5f;     // ���� ����

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

    // ���� �ൿ ���� ��  ---> isHearPlayerSound�� Ȱ���� �߼Ҹ� �ý��� ���� �ʿ�!!!
    private IEnumerator ZombieAI()
    {
        WaitForSeconds ws = new WaitForSeconds(0.7f);

        while (zombieState != ZombieState.DEAD)
        {
            yield return ws;  // ����
            
            // State
            float currentDist = (playerPos.position - transform.position).magnitude;

            if (currentDist < attackRadius )
                zombieState = ZombieState.ATTACK;
            else if (currentDist < detectionRadius)
                zombieState = ZombieState.TRACE;
            else
                zombieState = ZombieState.IDLE;

            // ������� �� �ൿ
            switch (zombieState)
            {
                case ZombieState.IDLE:      // 1. �⺻
                    agent.isStopped = true;
                    animator.SetBool("Walk", false);
                    break;
                case ZombieState.TRACE:     // 2. ����
                    agent.isStopped = false;
                    agent.SetDestination(playerPos.position);
                    animator.SetBool("Walk", true);
                    break;
                case ZombieState.ATTACK:    // 3. ����
                    agent.isStopped = true;
                    agent.SetDestination(playerPos.position);
                    animator.SetTrigger("Attack");
                    break; 
            }
        }

        // ���
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 4f);
        yield return null;
    }

    // ����
    private void Attack()
    {
        // ���� ��Ÿ�ӿ� ���
        // - ������ ����(�÷��̾� �ǰ��Լ�)
        // audioSource.PlayOneShot(attackSFX);
    }

    // �ǰ�
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
