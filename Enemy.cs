using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum AISTATE { PATROL, CHASE, ATTACK }
    public AISTATE enemystate = AISTATE.PATROL;

    public Transform player;
    private NavMeshAgent enemy;
    private Animator anim;

    public float distanceOffset = 2f;
    public List<Transform> waypoints = new List<Transform>();
    private Transform currentWaypoint;

    public float chaseRange = 10f;
    public float attackRange = 2.5f;
    public float attackRate = 1.2f;
    private float nextAttackTime;

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (waypoints.Count > 0)
        {
            currentWaypoint = waypoints[Random.Range(0, waypoints.Count)];
        }

        Changestate(AISTATE.PATROL);
    }

    public void Changestate(AISTATE newstate)
    {
        StopAllCoroutines();
        enemystate = newstate;

        switch (enemystate)
        {
            case AISTATE.PATROL:
                StartCoroutine(PatrolState());
                break;

            case AISTATE.CHASE:
                StartCoroutine(ChaseState());
                break;

            case AISTATE.ATTACK:
                StartCoroutine(AttackState());
                break;
        }
    }

    IEnumerator PatrolState()
    {
        enemy.isStopped = false;

        anim.SetBool("walk", true);
        anim.SetBool("run", false);

        while (enemystate == AISTATE.PATROL)
        {
            if (enemy.isOnNavMesh && currentWaypoint != null)
            {
                enemy.SetDestination(currentWaypoint.position);

                if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceOffset)
                {
                    currentWaypoint = waypoints[Random.Range(0, waypoints.Count)];
                }
            }

            if (player != null && Vector3.Distance(transform.position, player.position) < chaseRange)
            {
                Changestate(AISTATE.CHASE);
            }

            yield return null;
        }
    }

    IEnumerator ChaseState()
    {
        enemy.isStopped = false;

        anim.SetBool("walk", false);
        anim.SetBool("run", true);

        while (enemystate == AISTATE.CHASE)
        {
            if (player == null) yield break;

            float dist = Vector3.Distance(transform.position, player.position);

            if (dist <= attackRange)
            {
                Changestate(AISTATE.ATTACK);
            }
            else if (dist > chaseRange)
            {
                Changestate(AISTATE.PATROL);
            }
            else if (enemy.isOnNavMesh)
            {
                enemy.SetDestination(player.position);
            }

            yield return null;
        }
    }

    IEnumerator AttackState()
    {
        enemy.isStopped = true;

        anim.SetBool("run", false);
        anim.SetBool("walk", false);

        while (enemystate == AISTATE.ATTACK)
        {
            if (player == null) yield break;

            float dist = Vector3.Distance(transform.position, player.position);

            if (dist > attackRange)
            {
                Changestate(AISTATE.CHASE);
            }

            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + attackRate;
            }

            yield return null;
        }
    }

    void Attack()
    {
        anim.SetTrigger("rifle");

        Debug.Log("Enemy Attacked Player");

        if (player.GetComponent<Playerhdeath>() != null)
        {
            player.GetComponent<Playerhdeath>().TakeDamage(10);
        }
    }
}