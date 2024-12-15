using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("Player")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    [Header ("State")]
    public MovementState currentMovementState;
    public WeaponType currentWeapon;

    public enum MovementState { Idle, Walking, Running, Sneaking }
    public enum WeaponType { AKM, R1895, S12k, Kar98 }

    public bool isDie;

    private void Awake()
    {
        // �ʱ� ����
        isDie = false;
        currentHealth = maxHealth;
        currentMovementState = MovementState.Idle;
        currentWeapon = WeaponType.AKM;
    }

    // �Ϲ� ����
    public void Attack()
    {
        Debug.Log("�Ϲݰ���");
    }

    // Ư�� ����
    public void SpecialAttack()
    {
        Debug.Log("Ư�� ����");
    }

    // ������
    public void Reloading()
    {
        Debug.Log("������");
    }

    // �ǰ�
    public void Damaged(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        Debug.Log("���ݹ���");
    }

    // ��
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("��");
    }

    // ���
    private void Die()
    {
        isDie = true;
        Debug.Log("����");
    }
}