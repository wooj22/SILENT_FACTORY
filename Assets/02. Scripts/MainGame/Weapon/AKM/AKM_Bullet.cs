using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKM_Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public float lifetime;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            other.gameObject.GetComponent<Zombie>().Hit(damage);
        }
        else if (other.gameObject.CompareTag("MutatedZombie"))
        {
            other.gameObject.GetComponent<MutatedZombie>().Hit(damage);
        }

        Destroy(gameObject);
    }
}
