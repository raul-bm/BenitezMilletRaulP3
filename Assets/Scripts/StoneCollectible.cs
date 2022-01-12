using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneCollectible : MonoBehaviour
{
    public GameObject stoneCollectiblePrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        RectTransform rtSpawnCollectibles = GameObject.Find("SpawnCollectibles").GetComponent<RectTransform>();

        if (player != null)
        {
            if(player.ammo < player.maxAmmo)
            {
                player.AddAmmo();
                Instantiate(stoneCollectiblePrefab, new Vector2(Random.Range(rtSpawnCollectibles.rect.xMin, rtSpawnCollectibles.rect.xMax), Random.Range(rtSpawnCollectibles.rect.yMin, rtSpawnCollectibles.rect.yMax)), Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
