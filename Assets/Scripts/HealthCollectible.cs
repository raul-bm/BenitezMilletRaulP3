using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            if(player.health < player.maxHealth)
            {
                player.ChangeHealth(1);
                Destroy(gameObject);
            }
            
        }
    }
}
