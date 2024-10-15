using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float time;
    public float damage;
    public AudioClip explosionSound;

    private void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0f)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().PlayOneShot(explosionSound);
            }

            GetComponent<Animator>().SetTrigger("Explosion");
            Invoke("DestroyThis", 3f);
        }
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Health>().Damage(damage);
        }
    }
}