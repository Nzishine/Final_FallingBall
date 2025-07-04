using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float hoverHeight = 5f;
    public float moveSpeed = 10f;
    public float maxDistanceFromStart = 20f;
    
    private Rigidbody rb;
    private bool isControlled = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void Update()
    {
        // Check for touch/click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit)
                && hit.collider.gameObject == gameObject)
            {
                isControlled = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
            }
        }
        
        // Release control
        if (Input.GetMouseButtonUp(0) && isControlled)
        {
            isControlled = false;
            rb.useGravity = true;
        }
        
        // Move while controlled
        if (isControlled)
        {
            // Get target position in world space
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, hoverHeight, 0));
            float distance;
            
            if (plane.Raycast(ray, out distance))
            {
                targetPosition = ray.GetPoint(distance);
                
                // Limit distance from start position
                targetPosition = startPosition + Vector3.ClampMagnitude(
                    targetPosition - startPosition, 
                    maxDistanceFromStart);
                
                // Keep the hover height
                targetPosition.y = hoverHeight;
                
                // Move towards target
                rb.MovePosition(Vector3.Lerp(
                    transform.position, 
                    targetPosition, 
                    moveSpeed * Time.deltaTime));
            }
        }
    }
}