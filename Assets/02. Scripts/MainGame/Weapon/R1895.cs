using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1895 : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int currentAmmo;          // 현재 탄약 수
    public int maxAmmo;              // 최대 장전 탄약 수
    public int ammoPerShot;          // 탄약 소모량
    public float reloadTime;         // 재장전 시간
    public float damage;             // 기본 데미지
    public float attackCoolTime;     // 발사 주기

    [Header("Assets")]
    public GameObject bulletPrefab;      // 총알 프리팹
    public Transform muzzlePoint;        // 총알 생성 위치
    public ParticleSystem muzzleFlash;   // 총구 화염 이펙트
    public AudioClip fireSFX;            // 발사 사운드

    // Components
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerStateUI playerStateUI;

    private bool isReloading = false;    // 재장전 여부
    private float lastAttackTime = 0f;   // 마지막 공격 시간

    // 공격
    public void Attack()
    {
        // 쿨타임 제어
        if (Time.time - lastAttackTime < attackCoolTime)
            return;

        // 탄약 확인
        if (currentAmmo >= ammoPerShot)
        {
            // 총알 생성
            Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
            muzzleFlash.Play();
            audioSource.PlayOneShot(fireSFX);

            // 탄약 소비, UI 업데이트
            currentAmmo -= ammoPerShot;
            playerStateUI.UpdateAmmunitionUi(currentAmmo, maxAmmo, 0);

            // 쿨타임 제어
            lastAttackTime = Time.time;
        }
    }

    // 재장전
    public void ReLoading()
    {
        if (isReloading)
            return;

        // 인벤토리 잔여 탄약 확인
        if (inventory.ammunition45 <= 0)
            return;

        // 장전 탄약 확인, 재장전
        if (currentAmmo < maxAmmo)
        {
            StartCoroutine(ReLoadingCoroutine());
        }
    }

    private IEnumerator ReLoadingCoroutine()
    {
        isReloading = true;
        SoundManager.Instance.PlaySFX("SFX_Reloading");

        yield return new WaitForSeconds(reloadTime);

        // 충전 가능 탄약
        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, inventory.ammunition762);

        // 탄약 충전
        currentAmmo += ammoToReload;
        inventory.ammunition45 -= ammoToReload;

        // UI 업데이트
        playerStateUI.UpdateAmmunitionUi(currentAmmo, maxAmmo, ammoToReload);

        isReloading = false;
    }
}
