using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject platform;
    public Transform playerTransform;
    public BoxCollider2D mapBoundsCollider;

    public float distanceBetweenPlatforms = 3f;
    public int initialPlatformCount = 5;

    private List<GameObject> platforms = new List<GameObject>();
    private float lastPlatformY;

    private void Start()
    {
        InitializePlatforms();
    }

    private void Update()
    {
        if (playerTransform.position.y - lastPlatformY > distanceBetweenPlatforms)
        {
            SpawnPlatform();
        }
    }

    private void InitializePlatforms()
    {
        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform();
        }
    }

    private void SpawnPlatform()
    {
        Bounds bounds = mapBoundsCollider.bounds;
        float posy = lastPlatformY + distanceBetweenPlatforms;
        float posx = Random.Range(bounds.min.x, bounds.max.x);
        

        Vector3 platformSpawnPosition = new Vector3(posx, posy, 0);
        GameObject newPlatform = Instantiate(platform, platformSpawnPosition, Quaternion.identity);
        platforms.Add(newPlatform);

        lastPlatformY = newPlatform.transform.position.y;
    }
}
