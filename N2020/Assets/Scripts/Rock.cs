using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]int damage;
    [SerializeField]float force;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(Vector3.forward * force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            
            other.GetComponent<EnemyController>().TakeDamage(damage);
            
        }
    }
}
