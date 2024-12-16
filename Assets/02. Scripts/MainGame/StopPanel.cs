using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPanel : MonoBehaviour
{
    public void OnEnable()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SoundManager.Instance.PlaySFX("SFX_StopPanel");
    }

    public void OnDisable()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SoundManager.Instance.PlaySFX("SFX_StopPanel");
    }

    public void Continue()
    {
        Time.timeScale = 1f;
    }
}
