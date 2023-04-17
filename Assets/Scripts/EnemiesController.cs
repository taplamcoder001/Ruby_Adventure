using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public float speed;
    public float changeTime;
    private bool vertical;
    public ParticleSystem smokeEffect;
    float time;
    public int direction = 1;
    private Rigidbody2D enemyRb;
    Animator animator;
    bool broken = false;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        time = changeTime;

        animator = GetComponent<Animator>();

        audioSource=GetComponent<AudioSource>();
    }

    private void Update() {
        time -= Time.deltaTime;

        if(time <0)
        {
            direction = -direction;
            time = changeTime;
        }

        if(!broken)
        {
            return;
        }   
    }

    void FixedUpdate()
    {
        Vector2 position = enemyRb.position;

        if(vertical)
        {
            position.y = position.y + Time.deltaTime * speed *direction;
            animator.SetFloat("Move X",0);
            animator.SetFloat("Move Y", direction); 
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X",direction);
            animator.SetFloat("Move Y", 0);
        }
        
        enemyRb.MovePosition(position);
    }

    IEnumerator waiting()
    {
        yield return new WaitForSeconds(3);
    } 

    public void Fix()
    {
        StartCoroutine(waiting());
        broken = false;
        // smokeEffect.Stop();
        // Destroy(smokeEffect.gameObject);
        enemyRb.simulated = false;
        animator.SetTrigger("Fixed");
    }

    public void Broken()
    {
        broken = true;
        // smokeEffect.Play();
        enemyRb.simulated = false;
        animator.SetTrigger("Fixed");
    }

    void OnCollisionEnter2D(Collision2D other) {
        RubyController player = other.gameObject.GetComponent<RubyController>();
        
        if(player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
