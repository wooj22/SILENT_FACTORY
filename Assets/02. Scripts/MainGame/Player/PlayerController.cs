using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controll Setting")]
    [SerializeField] private LayerMask interactableLayer;            // 상호작용 레이어
    [SerializeField] private float interactRange = 2f;               // 상호작용 범위

    [Header("Key Bindings")]
    [SerializeField] private KeyCode interactKey = KeyCode.F;        // 상호작용
    [SerializeField] private KeyCode kitKey = KeyCode.Q;             // 구급상자
    [SerializeField] private KeyCode helpKey = KeyCode.K;            // 조작키
    [SerializeField] private KeyCode inventoryKey = KeyCode.Tab;     // 인벤토리
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;      // 게임 중지
    [SerializeField] private KeyCode attackkey = KeyCode.Mouse0;            // 기본 공격
    [SerializeField] private KeyCode speacialAttackkey = KeyCode.Mouse1;    // 특수 공격
    [SerializeField] private KeyCode reloadKey = KeyCode.R;                 // 재장전 키
    [SerializeField] private KeyCode equipAKM = KeyCode.Alpha1;      // AKM
    [SerializeField] private KeyCode equipR1895 = KeyCode.Alpha2;    // 리볼버
    [SerializeField] private KeyCode equipS12K = KeyCode.Alpha3;     // 사이가
    [SerializeField] private KeyCode equipKar98 = KeyCode.Alpha4;    // 카구팔

    [Header("UI Elements")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject stopPannelUI;
    [SerializeField] private GameObject keyInfoUI;
    [SerializeField] private GameObject interactUI;
    [SerializeField] private GameObject akmUI;
    [SerializeField] private GameObject r1895UI;
    [SerializeField] private GameObject s12kUI;
    [SerializeField] private GameObject kar98UI;

    private bool isInventoryUiOn = true;
    private bool isStopPannelUiOn = false;
    private bool isKeyInfoUiOn = false;

    [Header("---")]
    [SerializeField] private Player player;
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerStateUI playerStateUI;
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        InteractItems();
        HandleInputItem();
        HandleInputUI();
        HandleInputWeapon();
        HandleInputAttack();
    }

    // 아이템 사용 Input
    private void HandleInputItem()
    {
        // 구급상자 사용
        if (Input.GetKeyDown(kitKey))
        {
            player.Heal(50);
        }
    }

    // UI Input
    private void HandleInputUI()
    {
        // 조작키 토글
        if (Input.GetKeyDown(helpKey))
        {
            isKeyInfoUiOn = !isKeyInfoUiOn;
            keyInfoUI.SetActive(isKeyInfoUiOn);
            // on/off SFX
        }

        // 인벤토리 토글
        if (Input.GetKeyDown(inventoryKey))
        {
            isInventoryUiOn = !isInventoryUiOn;
            inventoryUI.SetActive(isInventoryUiOn);
            // on/off SFX
        }

        // 게임 중지 토글
        if (Input.GetKeyDown(pauseKey))
        {
            isStopPannelUiOn = !isStopPannelUiOn;
            stopPannelUI.SetActive(isStopPannelUiOn);
            // on/off SFX
        }
    }

    // 무기 전환 Input
    private void HandleInputWeapon()
    {
        // AKM
        if (Input.GetKeyDown(equipAKM))
        {
            akmUI.SetActive(true);
            r1895UI.SetActive(false);
            s12kUI.SetActive(false);
            kar98UI.SetActive(false);
            
            SoundManager.Instance.PlaySFX("SFX_Switch");
            player.currentWeapon = Player.WeaponType.AKM;
            playerStateUI.UpdateWeaponUi();
        }

        // R1895
        if (Input.GetKeyDown(equipR1895))
        {
            akmUI.SetActive(false);
            r1895UI.SetActive(true);
            s12kUI.SetActive(false);
            kar98UI.SetActive(false);
            
            SoundManager.Instance.PlaySFX("SFX_Switch");
            player.currentWeapon = Player.WeaponType.R1895;
            playerStateUI.UpdateWeaponUi();
        }

        // S12k
        if (Input.GetKeyDown(equipS12K))
        {
            akmUI.SetActive(false);
            r1895UI.SetActive(false);
            s12kUI.SetActive(true);
            kar98UI.SetActive(false);
            
            SoundManager.Instance.PlaySFX("SFX_Switch");
            player.currentWeapon = Player.WeaponType.S12k;
            playerStateUI.UpdateWeaponUi();
        }

        // Kar98
        if (Input.GetKeyDown(equipKar98))
        {
            akmUI.SetActive(false);
            r1895UI.SetActive(false);
            s12kUI.SetActive(false);
            kar98UI.SetActive(true);
            
            SoundManager.Instance.PlaySFX("SFX_Switch");
            player.currentWeapon = Player.WeaponType.Kar98;
            playerStateUI.UpdateWeaponUi();
        }

        
    }

    // 무기 사용 Input
    private void HandleInputAttack()
    {
        // 일반 공격
        if (Input.GetKey(attackkey))
        {
            player.Attack();
        }

        // 특수 공격
        if (Input.GetKey(speacialAttackkey))
        {
            player.SpecialAttack();
        }

        // 재장전
        if (Input.GetKeyDown(reloadKey))
        {
            player.Reloading();
        }
    }

    // 상호작용 아이템 습득
    private void InteractItems()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, interactRange, interactableLayer))
        {
            // 백신 기계 가동
            if (hit.collider.CompareTag("Machine"))
            {
                if (GameManager.Instance.isGetMaxEssence)
                {
                    interactUI.SetActive(true);
                    if (Input.GetKeyDown(interactKey))
                    {
                        GameManager.Instance.GameSuccess();
                        return;
                    }

                }
            }

            interactUI.SetActive(true);
            if (Input.GetKeyDown(interactKey))
            {
                if (hit.collider.CompareTag("Essence"))
                {
                    Debug.Log("정수 습득");
                    inventory.UpdateEssence(1);
                    GameManager.Instance.GetEssence();
                    SoundManager.Instance.PlaySFX("SFX_Get");
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Kit"))
                {
                    Debug.Log("구급상자 습득");
                    inventory.UpdateKit(1);
                    SoundManager.Instance.PlaySFX("SFX_Get");
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("7.62mm"))
                {
                    Debug.Log("7.62mm 습득");
                    inventory.Update762(40);
                    SoundManager.Instance.PlaySFX("SFX_Get");
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("12Gauge"))
                {
                    Debug.Log("12Gauge 습득");
                    inventory.Update12(12);
                    SoundManager.Instance.PlaySFX("SFX_Get");
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag(".45ACP"))
                {
                    Debug.Log(".45ACP 습득");
                    inventory.Update45(30);
                    SoundManager.Instance.PlaySFX("SFX_Get");
                    Destroy(hit.collider.gameObject);
                } 
            }
        }
        else
        {
            interactUI.SetActive(false);
        }
    }
}
