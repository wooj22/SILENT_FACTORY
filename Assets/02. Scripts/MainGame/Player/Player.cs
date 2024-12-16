using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("Player")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public int currentHealth;

    [Header ("State")]
    public MovementState currentMovementState;
    public WeaponType currentWeapon;

    public enum MovementState { Idle, Walking, Running, Sneaking }
    public enum WeaponType { AKM, R1895, S12k, Kar98 }

    [Header("Asset")]
    [SerializeField] ParticleSystem bloodEffect;

    public bool isDie;
    public PlayerStateUI playerStateUI;
    public Inventory inventory;

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
        playerStateUI.UpdateHpUi();
        bloodEffect.Play();
        // �ǰ� ����
        Debug.Log("���ݹ���");
    }

    // ��
    public void Heal(int healAmount)
    {
        if(inventory.kit >= 1)
        {
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            inventory.UpdateKit(-1);
            playerStateUI.UpdateHpUi();

            Debug.Log("��");
        }
        else
        {
            Debug.Log("���޻��� ����");
        }
    }

    // ���
    private void Die()
    {
        isDie = true;
        Debug.Log("����");
    }
}
