using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controll Setting")]
    [SerializeField] private LayerMask interactableLayer;            // ��ȣ�ۿ� ���̾�
    [SerializeField] private float interactRange = 2f;               // ��ȣ�ۿ� ����

    [Header("Key Bindings")]
    [SerializeField] private KeyCode interactKey = KeyCode.F;        // ��ȣ�ۿ�
    [SerializeField] private KeyCode kitKey = KeyCode.Q;             // ���޻���
    [SerializeField] private KeyCode helpKey = KeyCode.K;            // ����Ű
    [SerializeField] private KeyCode inventoryKey = KeyCode.Tab;     // �κ��丮
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;      // ���� ����
    [SerializeField] private KeyCode attackkey = KeyCode.Mouse0;            // �⺻ ����
    [SerializeField] private KeyCode speacialAttackkey = KeyCode.Mouse1;    // Ư�� ����
    [SerializeField] private KeyCode reloadKey = KeyCode.R;                 // ������ Ű
    [SerializeField] private KeyCode equipAKM = KeyCode.Alpha1;      // AKM
    [SerializeField] private KeyCode equipR1895 = KeyCode.Alpha2;    // ������
    [SerializeField] private KeyCode equipS12K = KeyCode.Alpha3;     // ���̰�
    [SerializeField] private KeyCode equipKar98 = KeyCode.Alpha4;    // ī����

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
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        InteractItems();
        HandleInputItem();
        HandleInputUI();
        HandleInputWeapon();
        HandleInputAttack();
    }

    // ������ ��� Input
    private void HandleInputItem()
    {
        // ���޻��� ���
        if (Input.GetKeyDown(kitKey))
        {
            // ���޻��� ��ũ��Ʈ ����� �ű� ��� ȣ��
        }
    }

    // UI Input
    private void HandleInputUI()
    {
        // ����Ű ���
        if (Input.GetKeyDown(helpKey))
        {
            isKeyInfoUiOn = !isKeyInfoUiOn;
            keyInfoUI.SetActive(isKeyInfoUiOn);
            // on/off SFX
        }

        // �κ��丮 ���
        if (Input.GetKeyDown(inventoryKey))
        {
            isInventoryUiOn = !isInventoryUiOn;
            inventoryUI.SetActive(isInventoryUiOn);
            // on/off SFX
        }

        // ���� ���� ���
        if (Input.GetKeyDown(pauseKey))
        {
            isStopPannelUiOn = !isStopPannelUiOn;
            stopPannelUI.SetActive(isStopPannelUiOn);
            // on/off SFX
        }
    }

    // ���� ��ȯ Input
    private void HandleInputWeapon()
    {
        // AKM
        if (Input.GetKeyDown(equipAKM))
        {
            akmUI.SetActive(true);
            r1895UI.SetActive(false);
            s12kUI.SetActive(false);
            kar98UI.SetActive(false);
            // ���� ��ȯ SFX
            player.currentWeapon = Player.WeaponType.AKM;
        }

        // R1895
        if (Input.GetKeyDown(equipR1895))
        {
            akmUI.SetActive(false);
            r1895UI.SetActive(true);
            s12kUI.SetActive(false);
            kar98UI.SetActive(false);
            // ���� ��ȯ SFX
            player.currentWeapon = Player.WeaponType.R1895;
        }

        // S12k
        if (Input.GetKeyDown(equipS12K))
        {
            akmUI.SetActive(false);
            r1895UI.SetActive(false);
            s12kUI.SetActive(true);
            kar98UI.SetActive(false);
            // ���� ��ȯ SFX
            player.currentWeapon = Player.WeaponType.S12k;
        }

        // Kar98
        if (Input.GetKeyDown(equipKar98))
        {
            akmUI.SetActive(false);
            r1895UI.SetActive(false);
            s12kUI.SetActive(false);
            kar98UI.SetActive(true);
            // ���� ��ȯ SFX
            player.currentWeapon = Player.WeaponType.Kar98;
        }

        
    }

    // ���� ��� Input
    private void HandleInputAttack()
    {
        // �Ϲ� ����
        if (Input.GetKey(attackkey))
        {
            // Player.cs�� �⺻ ���� �Լ� ȣ��
            Debug.Log("�Ϲݰ���");
        }

        // Ư�� ����
        if (Input.GetKey(speacialAttackkey))
        {
            // Player.cs�� Ư�� ���� �Լ� ȣ��
            Debug.Log("Ư������");
        }

        // ������
        if (Input.GetKeyDown(reloadKey))
        {
            // Player.cs�� ������ �Լ� ȣ��
            Debug.Log("������");
        }
    }

    // ��ȣ�ۿ� ������ ����
    private void InteractItems()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, interactRange, interactableLayer))
        {
            interactUI.SetActive(true);

            if (Input.GetKeyDown(interactKey))
            {
                if (hit.collider.CompareTag("Essence"))
                {
                    Debug.Log("���� ����");
                    // �κ��丮�� ++
                    // �ݱ� ����
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Kit"))
                {
                    Debug.Log("���޻��� ����");
                    // �κ��丮�� ++
                    // �ݱ� ����
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("7.62mm"))
                {
                    Debug.Log("7.62mm ����");
                    // �κ��丮�� ++
                    // �ݱ� ����
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("12Gauge"))
                {
                    Debug.Log("12Gauge ����");
                    // �κ��丮�� ++
                    // �ݱ� ����
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag(".45ACP"))
                {
                    Debug.Log(".45ACP ����");
                    // �κ��丮�� ++
                    // �ݱ� ����
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
