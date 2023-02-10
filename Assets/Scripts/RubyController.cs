using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;
    public float timeInvicible = 2.0f;

    // Define a property
    public int health { get { return currentHealth;  } }
    int currentHealth;

    bool isInvicible;
    float invicibleTimer;

    public GameObject projectilePrefab;
    Rigidbody2D rigidbody2d;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    float horizontal;
    float vertical;

    private AudioSource audioSource;
    public AudioClip hitClip;
    public AudioClip throwClip;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60; // Make unity rendering 10 frames per seconds

        // Get the rigidbody component
        rigidbody2d = GetComponent<Rigidbody2D>();
        // Get the animator
        animator = GetComponent<Animator>();

        // Init currentHealth
        currentHealth = maxHealth;

        // Get AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // Launch the projectile if press key c
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlaySound(throwClip);
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            // Four arguments for Raycast, the starting position, the direction, the maxium length, a layer mask
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));

            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                // There's chances that hit nothing, if call character displayDialog will cause error.
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

        if (isInvicible)
        {
            invicibleTimer -= Time.deltaTime;
            if (invicibleTimer < 0)
            {
                isInvicible = false;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    // Change health
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvicible) return;

            isInvicible = true;
            invicibleTimer = timeInvicible;

            animator.SetTrigger("Hit");
            PlaySound(hitClip);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
