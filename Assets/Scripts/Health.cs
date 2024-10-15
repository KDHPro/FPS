using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public AudioClip hitSound;
    public AudioClip dieSound;
    public Image hpGauge;

    public IHealthListener healthListener;
    private float maxHp;
    public float hp = 10f;
    private float lastAttackedTime;
    public float invincibleTime;

    private void Start()
    {
        maxHp = hp;
        healthListener = GetComponent<Health.IHealthListener>();
    }

    public void Damage(float damage)
    {
        if (hp > 0f && lastAttackedTime + invincibleTime < Time.time)
        {
            hp -= damage;
            if (hpGauge != null)
            {
                hpGauge.fillAmount = hp / maxHp;
            }

            lastAttackedTime = Time.time;

            if (hp <= 0f)
            {
                if (dieSound != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(dieSound);
                }
                if (healthListener != null)
                {
                    healthListener.Die();
                }
            }
            else
            {
                if (hitSound != null)
                    GetComponent<AudioSource>().PlayOneShot(hitSound);
            }
        }
    }

    public interface IHealthListener 
    {
        void Die();
    }
}