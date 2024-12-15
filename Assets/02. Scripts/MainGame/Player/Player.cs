using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 체력 관련 변수
    [SerializeField] private int maxHealth = 100; // 최대 체력
    private int currentHealth;

    // 움직임 상태
    public enum MovementState { Walking, Running, Sneaking }
    public MovementState currentMovementState;

    // 체력 상태
    public enum HealthState { Normal, TakingDamage }
    public HealthState currentHealthState;

    // 장착 무기 현황
    public enum WeaponType { AKM, R1895, S12k, Kar98 }
    public WeaponType currentWeapon;

    private void Awake()
    {
        // 체력 초기화
        currentHealth = maxHealth;

        // 초기 상태 설정
        currentMovementState = MovementState.Walking;
        currentHealthState = HealthState.Normal;
        currentWeapon = WeaponType.AKM;
    }

    // 체력 관리
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

    // 플레이어 사망 처리
    private void Die()
    {
        Debug.Log("Player is Dead!");
        // 추가적인 사망 처리 로직 구현 필요
    }

    // 현재 체력 반환
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // 무기 변경
    public void EquipWeapon(WeaponType weapon)
    {
        currentWeapon = weapon;
    }
}
