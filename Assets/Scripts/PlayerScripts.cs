using UnityEngine;

public class PlayerScripts : MonoBehaviour
{
    [SerializeField] private Bridge bridgePrefab; // Prefab to instantiate
    [SerializeField] private float growSpeed = 2f; // Growth speed
    [SerializeField] private float pushForce = 5f; // Force to push bridge forward
    [SerializeField] private Vector3 spawnOffset = new Vector3(0, 0, 1); // Adjust forward placement
    [SerializeField] private GameManager gameManager;



    private Bridge currentBridge; // The active bridge instance
    public bool isBuilding = false;

    private void Update()
    {
        if (transform.position.y < 1f)
        {
            GameEvents.OnGameOver?.Invoke();
        }

        HandleBridgeBuilding();
    }

    private void HandleBridgeBuilding()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Start building
        {
            if (currentBridge == null) // Instantiate only if there's no active bridge
            {
                Vector3 spawnPosition = transform.position + transform.forward * spawnOffset.z; // Place in front of player
                Quaternion spawnRotation = transform.rotation; // Match player rotation
                currentBridge = Instantiate(bridgePrefab, spawnPosition, spawnRotation);
                isBuilding = true;
            }
        }

        if (Input.GetKey(KeyCode.Q) && isBuilding) // Continue building
        {
            if (currentBridge != null)
            {
                currentBridge.IncreaseHeight(growSpeed * Time.deltaTime);
            }
        }

        if (Input.GetKeyUp(KeyCode.Q)) // Stop building immediately
        {
            isBuilding = false;
            if (currentBridge != null)
            {
                currentBridge.AddForce(transform.forward * pushForce); // Push bridge forward
                currentBridge = null; // Reset so a new bridge can be created
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Point"))
        {
            Debug.Log("Collided with Point object!");
            GameEvents.OnCreateNextPlatform?.Invoke();
            GameEvents.OnEarnScore?.Invoke();
        }
    }

}
