using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBehavior : MonoBehaviour
{
    public enum ZombieState { idle, walking, running, attacking };

    public Transform[] waypoints;
    public int speed;

    public int waypointIndex;
    private float WaypointDist;

    public ZombieState zombieState = ZombieState.idle;
    NavMeshAgent nm;
    public Transform player;
    public Animator animator;

    IEnumerator damagePlayer;
    IEnumerator healPlayer = null;
    public float damage = 5.0f;
    public float secondToDamagePlayer = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        transform.LookAt(waypoints[waypointIndex].position);
        nm = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        damagePlayer = player.GetComponent<PlayerBehaviour>().RemovePlayerHealth(damage, secondToDamagePlayer);

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
            case (ZombieState.walking):
                WaypointDist = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
                if (WaypointDist < 3f)
                {
                    IncreaseIndex();
                }
                onWalking();
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
        if (!CanSeePlayer())
        {
            nm.speed = 1.5f;
            zombieState = ZombieState.walking;
            animator.SetBool("walking", true);
        }

        if (CanSeePlayer())
        {
            zombieState = ZombieState.running;
            //nm.SetDestination(player.position);
            animator.SetBool("running", true);
        }
    }

    private void OnAttacking(float dist)
    {
        //nm.SetDestination(transform.position);
        if (GlobalControl.Instance.isPaused)
        {
            zombieState = ZombieState.idle;
            StopCoroutine(damagePlayer);
            animator.SetBool("attack", false);
            healPlayer = player.GetComponent<PlayerBehaviour>().StartHealPlayer(player.GetComponent<PlayerBehaviour>().healValue,
                player.GetComponent<PlayerBehaviour>().secondToHeal);
            StartCoroutine(healPlayer);
        }
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        if (dist > 6f)
        {
            StopCoroutine(damagePlayer);
            zombieState = ZombieState.running;
            animator.SetBool("attack", false);
            healPlayer = player.GetComponent<PlayerBehaviour>().StartHealPlayer(player.GetComponent<PlayerBehaviour>().healValue,
                player.GetComponent<PlayerBehaviour>().secondToHeal);
            StartCoroutine(healPlayer);
        }
    }

    void OnRunning(float distance)
    {
        if (CanSeePlayer())
        {
            nm.SetDestination(player.position);
            if (distance <= 6f)
            {
                zombieState = ZombieState.attacking;
                animator.SetBool("running", false);
                animator.SetBool("attack", true);
                if (healPlayer != null)
                {
                    StopCoroutine(healPlayer);
                }
                StartCoroutine(damagePlayer);
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
        if (GlobalControl.Instance.isPaused)
        {
            return false;
        }
        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, direction);
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 30f))
        {
            if (hit.collider.tag == "Player" && angle < 90f)
            {
                return true;
            }
        }
        return false;
    }

    void onWalking()
    {
        nm.SetDestination(waypoints[waypointIndex].position);
        if (CanSeePlayer())
        {
            nm.SetDestination(transform.position);
            zombieState = ZombieState.running;
            animator.SetBool("walking", false);
            animator.SetBool("running", true);
        }
    }

    void Patrol()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void IncreaseIndex()
    {
        waypointIndex++;
        if (waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
        //transform.LookAt(waypoints[waypointIndex].position);
    }




}
