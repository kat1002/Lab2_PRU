using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private GameObject pillar;   // Reference to the pillar object
    [SerializeField] private GameObject platform; // Reference to the sphere platform

    private bool isMoving = false;
    private bool isScaling = false;
    private float moveSpeed = 2f;
    private float scaleSpeed = 0.5f;
    private float moveDirection = 1f;
    private float moveDistance = 3f;
    private float minHeight = 1f;
    private float maxHeight = 5f;

    private Vector3 startPosition;
    private bool growing = true;
    [SerializeField] private PlayerScripts playerScripts;

    private void Start()
    {
        playerScripts = FindAnyObjectByType<PlayerScripts>();
    }

    /// <summary>
    /// Sets the height of the pillar.
    /// </summary>
    /// <param name="height">New height of the pillar.</param>
    public void SetPillarHeight(float height)
    {
        Vector3 pillarScale = pillar.transform.localScale;
        pillarScale.y = height; // Adjust height
        pillar.transform.localScale = pillarScale;

        // Adjust position to keep the platform at the correct height
        Vector3 pillarPosition = pillar.transform.position;
        pillarPosition.y = height / 2f; // Move up by half height to keep the base at the ground
        pillar.transform.position = pillarPosition;

        // Ensure the platform moves with the pillar
        Vector3 platformPosition = platform.transform.position;
        platformPosition.y = height; // Position platform on top of the pillar
        platform.transform.position = platformPosition;
    }

    /// <summary>
    /// Sets the size of the platform (sphere) in the X and Z directions.
    /// </summary>
    /// <param name="size">New size of the platform.</param>
    public void SetPlatformSize(float size)
    {
        Vector3 platformScale = platform.transform.localScale;
        platformScale.x = size; // Adjust X size
        platformScale.z = size; // Adjust Z size
        platform.transform.localScale = platformScale;
    }

    private void Update()
    {
        if (playerScripts.isBuilding)
        {
            isMoving = false;
            isScaling = false;
            return; // Stop movement during building mode
        }

        if (isMoving)
        {
            MovePillar();
        }

        if (isScaling)
        {
            ScalePillar();
        }
    }


    private void MovePillar()
    {
        if (pillar == null || platform == null) return;

        // Move left and right
        float moveAmount = moveSpeed * moveDirection * Time.deltaTime;
        pillar.transform.position += new Vector3(moveAmount, 0, 0);

        // Reverse direction when reaching move limit
        if (Mathf.Abs(pillar.transform.position.x - startPosition.x) >= moveDistance)
        {
            moveDirection *= -1;
        }

        // Dynamically adjust the platform height based on the pillar's current scale
        float pillarHeight = pillar.transform.localScale.y;
        float platformHeight = pillar.transform.position.y + (pillarHeight / 2f);

        // Keep the platform on top of the pillar
        Vector3 platformPosition = platform.transform.position;
        platformPosition.x = pillar.transform.position.x; // Match horizontal movement
        platformPosition.y = platformHeight;  // Correct height positioning
        platform.transform.position = platformPosition;
    }



    private void ScalePillar()
    {
        if (pillar == null) return;

        float scaleChange = scaleSpeed * Time.deltaTime;
        Vector3 scale = pillar.transform.localScale;

        if (growing)
        {
            scale.y += scaleChange;
            if (scale.y >= maxHeight) growing = false;
        }
        else
        {
            scale.y -= scaleChange;
            if (scale.y <= minHeight) growing = true;
        }

        pillar.transform.localScale = scale;

        // Adjust pillar position to stay anchored to the ground
        Vector3 position = pillar.transform.position;
        position.y = scale.y / 2f; // Ensures the pillar scales from the base
        pillar.transform.position = position;

        // **Dynamically calculate platform height**
        float platformHeight = pillar.transform.position.y + (scale.y / 2f);
        Vector3 platformPosition = platform.transform.position;
        platformPosition.y = platformHeight;
        platform.transform.position = platformPosition;
    }


    public void StartPlatformMovement()
    {
        if (pillar == null || platform == null)
        {
            Debug.LogError("🚨 Cannot start: Pillar or Platform is missing!");
            return;
        }

        // Reset movement and scaling states
        isMoving = false;
        isScaling = false;
        startPosition = pillar.transform.position;

        int randomState = Random.Range(0, 4);
        Debug.Log("🎲 Random State: " + randomState);

        switch (randomState)
        {
            case 1:
                isMoving = true;
                break;
            case 2:
                isScaling = true;
                break;
            case 3:
                isMoving = true;
                isScaling = true;
                break;
            default:
                Debug.Log("🚀 No movement this time.");
                break;
        }
    }


    public void StopPlatformMovement()
    {
        isMoving = false;
        isScaling = false;
    }
}
