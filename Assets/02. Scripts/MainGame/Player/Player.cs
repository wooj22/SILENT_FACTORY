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

    [Header("---")]
    public bool isDie;
    public PlayerStateUI playerStateUI;
    public Inventory inventory;

    // Weapons
    [SerializeField] private Akm akm;
    [SerializeField] private R1895 r1895;
    [SerializeField] private S12k s12K;
    [SerializeField] private Kar98 kar98;

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
        switch (currentWeapon)
        {
            case WeaponType.AKM:
                akm.Attack();
                break;
            case WeaponType.R1895:
                r1895.Attack();
                break;
            case WeaponType.S12k:
                s12K.Attack();
                break;
            case WeaponType.Kar98:
                kar98.Attack();
                break;
        }
    }

    // 특수 공격 (X)
    public void SpecialAttack()
    {
        Debug.Log("특수 공격");
        switch (currentWeapon)
        {
            case WeaponType.AKM:
                break;
            case WeaponType.R1895:
                break;
            case WeaponType.S12k:
                break;
            case WeaponType.Kar98:
                break;
        }
    }

    // 재장전
    public void Reloading()
    {
        Debug.Log("재장전");
        switch (currentWeapon)
        {
            case WeaponType.AKM:
                akm.ReLoading();
                break;
            case WeaponType.R1895:
                r1895.ReLoading();
                break;
            case WeaponType.S12k:
                s12K.ReLoading();
                break;
            case WeaponType.Kar98:
                kar98.ReLoading();
                break;
        }
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
        playerStateUI.UpdateHpUi();
        bloodEffect.Play();
        SoundManager.Instance.PlaySFX("SFX_PlayerHit");
        Debug.Log("공격받음");
    }

    // 힐
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
            SoundManager.Instance.PlaySFX("SFX_Heal");
            Debug.Log("힐");
        }
        else
        {
            Debug.Log("구급상자 없음");
        }
    }

    // 사망
    private void Die()
    {
        isDie = true;
        GameManager.Instance.GameOver();
        Debug.Log("죽음");
    }
}
