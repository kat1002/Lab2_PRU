using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Platform platformPrefab; // Prefab of the platform
    [SerializeField] private GameObject player;
    [SerializeField] public int Score { get; private set; }

    [SerializeField] private Platform firstPlatform;

    public Platform lastPlatform; // Track the last generated platform

    private void Awake()
    {
        Time.timeScale = 1f; // Resume time in case it was paused
        RemoveEvents();
        InitEvents();
    }

    private void Start()
    {
        Score = 0;
        GameEvents.OnUpdateScore?.Invoke(Score);

        SpawnInitialPlatforms();
        SetPlayerOnFirstPlatform();
    }

    #region Events

    private void InitEvents()
    {
        GameEvents.OnCreateNextPlatform.AddListener(GenerateNextPlatform);
        GameEvents.OnEarnScore.AddListener(EarnScore);
    }

    private void RemoveEvents()
    {
        GameEvents.OnCreateNextPlatform.RemoveListener(GenerateNextPlatform);
        GameEvents.OnEarnScore.RemoveListener(EarnScore);
    }

    #endregion

    private void SpawnInitialPlatforms()
    {
        // Spawn the second platform
        lastPlatform = Instantiate(platformPrefab, new Vector3(0f, 0f, 6f), Quaternion.identity);
        lastPlatform.SetPillarHeight(5f);
        lastPlatform.SetPlatformSize(3f);
    }

    private void SetPlayerOnFirstPlatform()
    {
        if (player != null)
        {
            Vector3 playerPosition = new Vector3(0f, 5.5f, 0f); // First platform position + height
            player.transform.position = playerPosition;
        }
    }

    private void Update()
    {
        if (player != null && lastPlatform != null)
        {
            float distance = Vector2.Distance(
                new Vector2(player.transform.position.x, player.transform.position.z),
                new Vector2(lastPlatform.transform.position.x, lastPlatform.transform.position.z)
            );


            if (distance < 1f) // If player reaches the last platform, spawn a new one
            {
                lastPlatform.StopPlatformMovement();
                GenerateNextPlatform();
                GameEvents.OnEarnScore?.Invoke();
            }
        }


    }

    private void GenerateNextPlatform()
    {
        float randomOffsetX = Random.Range(-2f, 2f); // Randomly shift the platform left/right
        float randomOffsetZ = Random.Range(5f, 10f); // Randomly change forward distance
        float randomPillarHeight = Random.Range(3f, 8f); // Random pillar height
        float randomPlatformSize = Random.Range(2f, 5f); // Random platform size

        Vector3 newPosition = lastPlatform.transform.position + new Vector3(randomOffsetX, 0f, randomOffsetZ); // Adjust position
        lastPlatform = Instantiate(platformPrefab, newPosition, Quaternion.identity);
        lastPlatform.SetPillarHeight(randomPillarHeight);
        lastPlatform.SetPlatformSize(randomPlatformSize);
        lastPlatform.StartPlatformMovement();
    }


    private void EarnScore()
    {
        Score += 1;
        GameEvents.OnUpdateScore?.Invoke(Score);
    }
}
