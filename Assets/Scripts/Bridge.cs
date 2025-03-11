using UnityEngine;

public class Bridge : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float fallForce = 0.1f; // Force applied when bridge falls
    [SerializeField] private Transform headpoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Ensure Rigidbody exists
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true; // Prevent movement until released
        rb.useGravity = false; // Disable gravity until bridge falls
    }

    public void AddForce(Vector3 dir)
    {
        rb.isKinematic = false; // Enable physics
        rb.useGravity = true;   // Allow gravity to take effect
        rb.AddForce(dir.normalized * fallForce, ForceMode.Impulse); // Apply force to push it forward/down

        Destroy(gameObject, 10f);
    }

    public void IncreaseHeight(float amount)
    {
        // Use a clamped small amount to prevent rapid growth
        float adjustedAmount = Mathf.Clamp(amount, 0.001f, 0.05f);

        // Apply the height increase
        transform.localScale += new Vector3(0, adjustedAmount, 0);

        // Ensure the bridge grows upwards rather than stretching from the center
        transform.position += new Vector3(0, adjustedAmount / 2, 0);

        headpoint.transform.position = transform.position + new Vector3(0f, transform.localScale.y / 2f, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Dead"))
        {
            GameEvents.OnGameOver?.Invoke();
        }
    }
}
