using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float velocity = 3.0f;

    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    public int maxAmmo = 3;

    public int enemiesKilled { get { return currentEnemiesKilled; } }
    int currentEnemiesKilled;
    
    public int ammo {  get { return currentAmmo; } }
    int currentAmmo;

    public GameObject projectilePrefab;

    public Text bulletsUI;

    public AudioClip throwClip;
    public AudioClip heartBeatClip;
    public AudioClip deathClip;

    Rigidbody2D rigidbody2d;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    AudioSource audioSource;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 position = rigidbody2d.position;

        position = position + move * velocity * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Launch();
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");

            if (isInvincible) 
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        Debug.Log(currentHealth + "/" + maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

        if(currentHealth <= 2 && currentHealth > 0)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = heartBeatClip;
                audioSource.Play();
            }
        } else if(currentHealth == 0) {
            animator.SetTrigger("Death");
            audioSource.PlayOneShot(deathClip);
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    public void AddAmmo()
    {
        currentAmmo++;
        bulletsUI.text = currentAmmo.ToString();
    }

    void Launch()
    {
        if(currentAmmo > 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            StoneProjectile projectile = projectileObject.GetComponent<StoneProjectile>();
            projectile.Launch(lookDirection, 350);

            animator.SetTrigger("Launch");
            audioSource.PlayOneShot(throwClip);

            currentAmmo--;

            bulletsUI.text = currentAmmo.ToString();
        }
    }

    public void EnemyKilled()
    {
        currentEnemiesKilled++;
    }
}
