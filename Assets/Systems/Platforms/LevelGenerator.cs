using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject platform;
    public Transform playerTransform;

    public float timeBetweenSpawns;
    private Vector3 platformSpawnPosition;

    private void Update()
    {
        transform.position = playerTransform.position; 

        float posy = Random.Range(transform.position.y + 2, transform.position.y + 4);
        float posx = Random.Range(transform.position.x -3, transform.position.x + 3);

        platformSpawnPosition = new Vector3 (posx, posy, 0);

        if (playerTransform.localPosition.y > 5)
        {
            SpawnPlatforms();
        }      
    }

    private void SpawnPlatforms()
    {
        Instantiate(platform, platformSpawnPosition, Quaternion.identity);
    }
}
