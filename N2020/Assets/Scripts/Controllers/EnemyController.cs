using SensorToolkit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    GameManager gameManager;
    NavMeshAgent agent;

    private bool isChasing;
    private bool isFighting;
    private RangeSensor sensor;

    private int targetIndex;

    private Transform fightTarget;

    
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeedDelay;
    [SerializeField] private int strength;
    [SerializeField] private int health;
    [SerializeField] private HalthBar healthBar;

    [SerializeField] private float timeToDisapear;

    [SerializeField] private Animator enemyAnim;

    [HideInInspector]public bool isDead;

    
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth(health);
        gameManager = FindObjectOfType<GameManager>();
        isChasing = false;
        targetIndex = 0;
        sensor = GetComponent<RangeSensor>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.velocity.magnitude >= 0.1f && !isFighting)
        {
            enemyAnim.SetBool("isRunning", true);
        }
        else
        {
            enemyAnim.SetBool("isRunning", false);
        }
        if (!isDead)
        {
            if (!isChasing && health > 0)
            {

                agent.SetDestination(gameManager.pathTargets[targetIndex].position);

                if (Vector3.Distance(transform.position, gameManager.pathTargets[targetIndex].position) <= attackRange)
                {
                    targetIndex++;
                }
            }
            else
            {
                if (fightTarget != null)
                {
                    if (Vector3.Distance(fightTarget.position, transform.position) >= 6 && health > 0)
                    {
                        agent.SetDestination(fightTarget.position);
                    }
                    else if (!isFighting)
                    {
                        StartCoroutine(Fight());
                    }
                }
                else if (isFighting)
                {
                    StopFighting();
                }


            }

            if (isFighting)
            {
                Vector3 targetPoint = new Vector3(fightTarget.position.x, transform.position.y, fightTarget.position.z) - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
            }
        }
        else
        {
            agent.isStopped = true;
        }
       

    }

    public void StartChasing()
    {
        isChasing = true;
        fightTarget = sensor.DetectedObjectsOrderedByDistance[0].transform;

    }

    public void StopFighting()
    {
        if(sensor.DetectedObjects.Count <= 0)
        {
            isChasing = false;
            isFighting = false;
        }
        else
        { 
            fightTarget = sensor.DetectedObjectsOrderedByDistance[0].transform;
        }
        
    }

    IEnumerator Fight()
    {
        isFighting = true;
        while (isFighting)
        {            
            if (!isDead)
            {
                Attack();
                
            }
            yield return new WaitForSeconds(attackSpeedDelay);
        }
    }

    void Attack()
    {

        enemyAnim.SetTrigger("Attack");
        fightTarget.GetComponent<TroopController>().TakeDamage(strength);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {

            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(gameObject, timeToDisapear);
            enemyAnim.SetTrigger("Die");
            isDead = true;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
