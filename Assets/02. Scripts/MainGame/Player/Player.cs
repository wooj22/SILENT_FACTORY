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
        // 초기 상태
        isDie = false;
        currentHealth = maxHealth;
        currentMovementState = MovementState.Idle;
        currentWeapon = WeaponType.AKM;
    }

    // 일반 공격
    public void Attack()
    {
        Debug.Log("일반공격");
    }

    // 특수 공격
    public void SpecialAttack()
    {
        Debug.Log("특수 공격");
    }

    // 재장전
    public void Reloading()
    {
        Debug.Log("재장전");
    }

    // 피격
    public void Damaged(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        Debug.Log("공격받음");
    }

    // 힐
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("힐");
    }

    // 사망
    private void Die()
    {
        isDie = true;
        Debug.Log("죽음");
    }
}
