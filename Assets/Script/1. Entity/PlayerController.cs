using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;
using static DesignEnums;
public class PlayerController : MonoBehaviour
{
    public PlayerStatHandler playerStatHandler { get; private set; } = new PlayerStatHandler();
    private NavMeshAgent agent;
    private List<GameObject> monsters;
    private GameObject currentTarget;
    private float attackRange;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        attackRange = playerStatHandler.GetStat(EStat.AttackRange);
        FindMonsters();
    }

    void Update()
    {
        if (currentTarget == null || !currentTarget.activeInHierarchy)
        {
            FindNextTarget();
        }
        else
        {
            MoveToTarget();
            CheckAndAttack();
        }
    }

    private void FindMonsters()
    {
        monsters = Managers.Stage.GetMonsters().ToList(); 
    }
 
    private void FindNextTarget()
    {
        currentTarget = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject monster in monsters)
        {
            if (monster.activeInHierarchy)
            {
                float distance = Vector3.Distance(transform.position, monster.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    currentTarget = monster;
                }
            }
        }
    }

    private void MoveToTarget()
    {
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.transform.position);
        }
    }

    private void CheckAndAttack()
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distanceToTarget <= attackRange)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Implement attack logic here
        Debug.Log("Player attacks the monster!");
    }
}
