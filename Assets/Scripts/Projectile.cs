using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D projectileRb;

    public AudioClip hitClip;
    // Start is called before the first frame update
    void Awake()
    {
        projectileRb  = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        // if position more then 1000 destroy projectile
        if(transform.position.magnitude > 50.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force){
        projectileRb.AddForce(direction * force);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        EnemiesController e = other.collider.GetComponent<EnemiesController>();
        if (e != null)
        {
            e.Broken();
            e.PlaySound(hitClip);
        }
        Destroy(gameObject);
        
    }
}
