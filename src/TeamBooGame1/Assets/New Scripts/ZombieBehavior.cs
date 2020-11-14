using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehavior : MonoBehaviour
{
    public enum ZombieState{idle, walking, running, attacking};

    public ZombieState zombieState = ZombieState.idle;
    NavMeshAgent nm;
    public Transform player;
    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        switch (zombieState)
        {
            case (ZombieState.idle):
                OnIdle();
                break;
            case (ZombieState.running):
                OnRunning(dist);
                break;
            case (ZombieState.attacking):
                OnAttacking(dist);
                break;
            default:
                break;
        }
    }

    private void OnIdle()
    {
        if (CanSeePlayer())
        {
            zombieState = ZombieState.running;
            //nm.SetDestination(player.position);
            animator.SetBool("running", true);
        }
    }

    private void OnAttacking(float dist)
    {
        nm.SetDestination(transform.position);
        if (dist > 5f)
        {
            zombieState = ZombieState.running;
            animator.SetBool("attack", false);
        }
    }

    void OnRunning(float distance)
    {
        if (CanSeePlayer())
        {
            nm.SetDestination(player.position);
            if (distance < 5f)
            {
                zombieState = ZombieState.attacking;
                //animator.SetBool("running", false);
                animator.SetBool("attack", true);
            }
        }
        else
        {
            nm.SetDestination(transform.position);
            zombieState = ZombieState.idle;
            animator.SetBool("running", false);
        }
    }

    bool CanSeePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, direction);
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 30f))
        {
            if(hit.collider.tag == "Player" && angle < 80f)
            {
                return true;
            }
        }
        return false;
    }


}
