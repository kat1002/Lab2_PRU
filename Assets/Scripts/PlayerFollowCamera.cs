using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10); // Camera offset
    [SerializeField] private float smoothSpeed = 5f; // Camera follow speed

    private void LateUpdate()
    {
        if (player == null) return;

        // Target position with offset
        Vector3 targetPosition = player.position + offset;

        // Smoothly interpolate between current and target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Optionally, keep the camera always looking at the player
        transform.LookAt(player);
    }
}
