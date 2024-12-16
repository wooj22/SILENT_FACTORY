using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Image hpBar;
    [SerializeField] Text hpText;
    [SerializeField] Text weaponText;
    [SerializeField] Image weapoImage;
    [SerializeField] Text ammunitionText;
    [SerializeField] Image coolTimeBar;

    [Header("Asset")]
    [SerializeField] Sprite akm;
    [SerializeField] Sprite r1895;
    [SerializeField] Sprite s12k;
    [SerializeField] Sprite kar98;

    [SerializeField] private Player player;
    [SerializeField] private Inventory inventory;

    // HP
    public void UpdateHpUi()
    {
        hpBar.fillAmount = player.currentHealth;
        hpText.text = (player.currentHealth).ToString();
    }

    // ¹«±â Á¤º¸
    public void UpdateWeaponUi()
    {
        switch (player.currentWeapon)
        {
            case Player.WeaponType.AKM:
                weaponText.text = "AKM";
                weapoImage.sprite = akm;
                break;
            case Player.WeaponType.R1895:
                weaponText.text = "R1895";
                weapoImage.sprite = r1895;
                break;
            case Player.WeaponType.S12k:
                weaponText.text = "S12k";
                weapoImage.sprite = s12k;
                break;
            case Player.WeaponType.Kar98:
                weaponText.text = "Kar98";
                weapoImage.sprite = kar98;
                break;
            default:
                break;
        }
    }

    // ÀÜ¿© Åº¾à
    public void UpdateAmmunitionUi(int current, int max, int use)
    {
        switch (player.currentWeapon)
        {
            case Player.WeaponType.AKM:
                inventory.Update762(-use);
                ammunitionText.text =
                    current + "/" + max + " (" + inventory.ammunition762 + ")";
                break;
            case Player.WeaponType.R1895:
                inventory.Update45(-use);
                ammunitionText.text =
                    current + "/" + max + " (" + inventory.ammunition45 + ")";
                break;
            case Player.WeaponType.S12k:
                inventory.Update12(-use);
                ammunitionText.text =
                    current + "/" + max + " (" + inventory.ammunition12 + ")";
                break;
            case Player.WeaponType.Kar98:
                inventory.Update762(-use);
                ammunitionText.text =
                    current + "/" + max + " (" + inventory.ammunition762 + ")";
                break;
            default:
                break;
        }


    }

    // Æ¯¼ö °ø°Ý ÄðÅ¸ÀÓ
    public void UpdateCoolTime(float amount)
    {
        coolTimeBar.fillAmount += amount;
    }
}
