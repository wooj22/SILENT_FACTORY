using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // ü�� ���� ����
    [SerializeField] private int maxHealth = 100; // �ִ� ü��
    private int currentHealth;

    // ������ ����
    public enum MovementState { Walking, Running, Sneaking }
    public MovementState currentMovementState;

    // ü�� ����
    public enum HealthState { Normal, TakingDamage }
    public HealthState currentHealthState;

    // ���� ���� ��Ȳ
    public enum WeaponType { AKM, R1895, S12k, Kar98 }
    public WeaponType currentWeapon;

    private void Awake()
    {
        // ü�� �ʱ�ȭ
        currentHealth = maxHealth;

        // �ʱ� ���� ����
        currentMovementState = MovementState.Walking;
        currentHealthState = HealthState.Normal;
        currentWeapon = WeaponType.AKM;
    }

    // ü�� ����
    public void TakeDamage(int damage)
    {
        currentHealthState = HealthState.TakingDamage;
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // �÷��̾� ��� ó��
    private void Die()
    {
        Debug.Log("Player is Dead!");
        // �߰����� ��� ó�� ���� ���� �ʿ�
    }

    // ���� ü�� ��ȯ
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // ���� ����
    public void EquipWeapon(WeaponType weapon)
    {
        currentWeapon = weapon;
    }
}
