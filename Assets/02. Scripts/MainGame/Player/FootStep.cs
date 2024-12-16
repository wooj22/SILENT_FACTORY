using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    [SerializeField] Player player;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //StartCoroutine(FootSpet());
    }

    IEnumerator FootSpet()
    {
        while (!player.isDie)
        {
            switch (player.currentMovementState)
            {
                case Player.MovementState.Walking:
                    //audioSource.Stop();
                    audioSource.Play();
                    yield return new WaitForSeconds(0.5f);
                    break;
                case Player.MovementState.Running:
                    //audioSource.Stop();
                    audioSource.Play();
                    yield return new WaitForSeconds(0.3f);
                    break;
                case Player.MovementState.Sneaking:
                    //audioSource.Stop();
                    audioSource.Play();
                    yield return new WaitForSeconds(1f);
                    break;
                case Player.MovementState.Idle:
                    audioSource.Stop();
                    break;
                default:
                    break;
            }
        }
        
        yield return null;
    }
}
