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
    bool broken = true;
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
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            StartCoroutine(waiting());
        }

        time -= Time.deltaTime;

        if(time <0)
        {
            direction = -direction;
            time = changeTime;
        }   
    }

    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
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
        broken = true;
        enemyRb.simulated = true;
        smokeEffect.Stop();
        animator.SetBool("Fixed",false);
    } 

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        broken = false;
        enemyRb.simulated = false;
        //optional if you added the fixed animation
        animator.SetBool("Fixed",true);
        smokeEffect.Play();
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
