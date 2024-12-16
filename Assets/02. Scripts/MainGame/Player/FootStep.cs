using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    [SerializeField] private Player player;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FootStepCoroutine());
    }

    IEnumerator FootStepCoroutine()
    {
        while (!player.isDie) // �÷��̾ ���� �ʾ��� �� �ݺ�
        {
            switch (player.currentMovementState)
            {
                case Player.MovementState.Walking:
                    audioSource.Play();
                    yield return new WaitForSeconds(0.5f);
                    break;

                case Player.MovementState.Running:
                    audioSource.Play();
                    yield return new WaitForSeconds(0.3f);
                    break;

                case Player.MovementState.Sneaking:
                    audioSource.Play();
                    yield return new WaitForSeconds(1f);
                    break;

                case Player.MovementState.Idle:
                    audioSource.Stop();
                    yield return null;
                    break;

                default:
                    yield return null;
                    break;
            }
        }
    }
}
