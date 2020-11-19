using SensorToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameObject hatchetGO;
    public GameObject rockGO;
    [SerializeField] float spawnRate;
    public Transform spawnPoint;

    private bool enemyOnRange;

    private RangeSensor sensor;

    public Transform target;

    private Vector3 throwDirection;


    private void Start()
    {
        sensor = GetComponent<RangeSensor>();
    }
    public IEnumerator ThrowHatchet(Transform target)
    {
        while (enemyOnRange)
        {
            if(hatchetGO != null)
            {
                GameObject hatchet = Instantiate(hatchetGO, spawnPoint.position, Quaternion.identity);
                hatchet.GetComponent<HatchetScript>().destination = target.position;
                yield return new WaitForSeconds(spawnRate);
            }
            else
            {
                if(target.transform != null)
                {
                    GameObject rock = Instantiate(rockGO, spawnPoint.position, Quaternion.LookRotation(target.transform.position - transform.position));
                    yield return new WaitForSeconds(spawnRate);
                }

            }

        }

        
        
    }

    public void StartThrowing()
    {

        
        enemyOnRange = true;
        target = sensor.DetectedObjectsOrderedByDistance[0].transform;
        if (sensor.DetectedObjects.Count <= 1)
        {
            StartCoroutine(ThrowHatchet(target));   
        }
        
    }

    public void StopThrowing()
    {
        if (sensor.DetectedObjects.Count <= 0)
        {
            enemyOnRange = false;
        }
        else
        {
            target = sensor.DetectedObjectsOrderedByDistance[0].transform;
        }
    }

}
