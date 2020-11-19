using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchetScript : MonoBehaviour
{

    public Vector3 destination;
    public float velocity;
    //TowerController tController;
    public float speed;

    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(GetLocalDirection() * speed * Time.deltaTime);
    }

    Vector3 GetLocalDirection()
    {
        return transform.InverseTransformDirection((destination - transform.position).normalized);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
        }
    }
}
