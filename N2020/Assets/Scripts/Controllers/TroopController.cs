using SensorToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TroopController : MonoBehaviour
{
    NavMeshAgent agent;
    private bool isChasing;
    private bool isFighting;
    private Transform fightTarget;
    private RangeSensor sensor;

    [SerializeField] private float timeToDisapear;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeedDelay;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int strength;
    [SerializeField] private int maxStrength;
    [SerializeField] private HalthBar healthBar;

    [SerializeField]private Animator troopAnim;

    [SerializeField]bool isValkyrie;

    bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        agent = GetComponent<NavMeshAgent>();
        sensor = GetComponent<RangeSensor>();
        health = maxHealth;
        healthBar.SetMaxHealth(health);
        agent.isStopped = true;
        agent.stoppingDistance = attackRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) 
        {
            if (!agent.isStopped)
            {
                troopAnim.SetBool("isRunning", true);
            }
            else
            {
                troopAnim.SetBool("isRunning", false);
            }
            if (fightTarget != null)
            {
                if (Vector3.Distance(fightTarget.position, transform.position) >= attackRange && health > 0)
                {
                    agent.isStopped = false;
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

            if (isFighting)
            {
                Vector3 targetPoint = new Vector3(fightTarget.position.x, transform.position.y, fightTarget.position.z) - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);

                transform.rotation = new Quaternion(targetRotation.x, transform.rotation.y, targetRotation.z, transform.rotation.w);
            }
        }
 
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if(health <= 0)
        {            
            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(gameObject, timeToDisapear);
            troopAnim.SetTrigger("Die");
            GetComponent<TroopController>().enabled = false;
            isDead = true;
        }
    }

    public void Cure(int amountToCure)
    {
        health += amountToCure;
        healthBar.SetHealth(health);
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Fortify(int amountToFortify)
    {
        health += amountToFortify;
        maxHealth += amountToFortify;
        healthBar.SetHealth(health);
    }

    public void Berserk(int amountToBerserk)
    {
        strength += amountToBerserk;
    }

    public void StartChasing()
    {
        agent.isStopped = false;
        isChasing = true;
        fightTarget = sensor.DetectedObjectsOrderedByDistance[0].transform;
    }
    public void StopFighting()
    {
        if (sensor.DetectedObjects.Count <= 0)
        {
            agent.isStopped = true;
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
            Attack();
            yield return new WaitForSeconds(attackSpeedDelay);
        }
    }

    void Attack()
    {
        troopAnim.SetTrigger("Attack");
        if (isValkyrie)
        {
            foreach(GameObject nearEnemy in sensor.DetectedObjects)
            {
                if(Vector3.Distance(transform.position, nearEnemy.transform.position) <= attackRange)
                {
                    nearEnemy.GetComponent<EnemyController>().TakeDamage(strength);
                }
                
            }
        }
        else if(fightTarget)
        {
            fightTarget.GetComponent<EnemyController>().TakeDamage(strength);
        }       
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
