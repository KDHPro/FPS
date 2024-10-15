using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Health.IHealthListener
{
    public enum State
    {
        Idle,
        Walk,
        Attack,
        Dying
    };
    public State state;

    public GameObject player;

    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource audio;

    public float timeForNextState = 2f;

    private void Start()
    {
        state = State.Idle;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {

        switch (state)
        {
            case State.Idle:
                float dist = (player.transform.position - (transform.position + GetComponent<CapsuleCollider>().center)).magnitude;

                if (dist < 1f)
                {
                    Attack();
                }
                else
                {
                    timeForNextState -= Time.deltaTime;
                    if (timeForNextState < 0f)
                    {
                        StartWalk();
                    }
                }
                break;
            case State.Walk:
                if (agent.remainingDistance < 1f || agent.hasPath == false)
                {
                    StartIdle();
                }
                break;
            case State.Attack:
                timeForNextState -= Time.deltaTime;
                if (timeForNextState < 0f)
                {
                    StartIdle();
                }
                break;
        }
    }

    private void Attack()
    {
        state = State.Attack;
        timeForNextState = 1.5f;
        animator.SetTrigger("Attack");
    }

    private void StartWalk()
    {
        audio.Play();
        state = State.Walk;
        agent.destination = player.transform.position;
        agent.isStopped = false;
        animator.SetTrigger("Walk");
    }

    private void StartIdle()
    {
        audio.Stop();
        state = State.Idle;
        timeForNextState = Random.Range(1f, 2f);
        agent.isStopped = true;
        animator.SetTrigger("Idle");
    }

    public void Die()
    {
        state = State.Dying;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Invoke("DestroyThis", 2.5f);
    }

    private void DestroyThis()
    {
        GameManager.instance.EnemyDied();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Health>().Damage(5);
        }
    }
}