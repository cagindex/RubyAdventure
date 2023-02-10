using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2.0f;
    public float changeTime = 3.0f;
    public bool vertical;
    public ParticleSystem smokeEffect;

    Rigidbody2D controller;
    Animator animator;
    AudioSource audioSource;
    float timer;
    int direction = 1;
    bool broken;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        timer = changeTime;
        broken = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!broken) return;

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -1 * direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if (!broken) return;

        Vector2 position = controller.position;
        
        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        controller.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        broken = false;
        controller.simulated = false;

        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        audioSource.Stop();
    }
}
