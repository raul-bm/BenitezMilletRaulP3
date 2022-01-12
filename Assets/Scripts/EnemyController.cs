using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;
    public int damageToPlayer = 1;

    bool killed;

    Rigidbody2D rigidbody2d;
    Animator animator;
    AudioSource audioSource;

    public AudioClip clipDeath;
    public AudioClip clipAttack;

    float timer;
    int direction = 1;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(killed)
        {
            return;
        }
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        Vector2 position = rigidbody2d.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        } else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController playerCollision = other.gameObject.GetComponent<PlayerController>();

        if(playerCollision != null)
        {
            playerCollision.ChangeHealth(-damageToPlayer);
            audioSource.PlayOneShot(clipAttack);
        }
    }

    public void Kill()
    {
        GameObject player = GameObject.Find("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();

        killed = true;
        rigidbody2d.simulated = false;

        playerController.EnemyKilled();
        audioSource.PlayOneShot(clipDeath);

        animator.SetTrigger("Death");
    }
}
