using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1895 : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int currentAmmo;          // ���� ź�� ��
    public int maxAmmo;              // �ִ� ���� ź�� ��
    public int ammoPerShot;          // ź�� �Ҹ�
    public float reloadTime;         // ������ �ð�
    public float damage;             // �⺻ ������
    public float attackCoolTime;     // �߻� �ֱ�

    [Header("Assets")]
    public GameObject bulletPrefab;      // �Ѿ� ������
    public Transform muzzlePoint;        // �Ѿ� ���� ��ġ
    public ParticleSystem muzzleFlash;   // �ѱ� ȭ�� ����Ʈ
    public AudioClip fireSFX;            // �߻� ����

    // Components
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerStateUI playerStateUI;

    private bool isReloading = false;    // ������ ����
    private float lastAttackTime = 0f;   // ������ ���� �ð�

    // ����
    public void Attack()
    {
        // ��Ÿ�� ����
        if (Time.time - lastAttackTime < attackCoolTime)
            return;

        // ź�� Ȯ��
        if (currentAmmo >= ammoPerShot)
        {
            // �Ѿ� ����
            Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
            muzzleFlash.Play();
            audioSource.PlayOneShot(fireSFX);

            // ź�� �Һ�, UI ������Ʈ
            currentAmmo -= ammoPerShot;
            playerStateUI.UpdateAmmunitionUi(currentAmmo, maxAmmo, 0);

            // ��Ÿ�� ����
            lastAttackTime = Time.time;
        }
    }

    // ������
    public void ReLoading()
    {
        if (isReloading)
            return;

        // �κ��丮 �ܿ� ź�� Ȯ��
        if (inventory.ammunition45 <= 0)
            return;

        // ���� ź�� Ȯ��, ������
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

        // ���� ���� ź��
        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, inventory.ammunition762);

        // ź�� ����
        currentAmmo += ammoToReload;
        inventory.ammunition45 -= ammoToReload;

        // UI ������Ʈ
        playerStateUI.UpdateAmmunitionUi(currentAmmo, maxAmmo, ammoToReload);

        isReloading = false;
    }
}
