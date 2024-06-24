using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody rb;
    private bool isDragging = false;
    private float distanceToCamera;
    private float dragSpeed = 10f; // Adjust the drag speed as necessary

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        if (rb != null)
        {
            isDragging = true;
            distanceToCamera = Vector3.Distance(transform.position, mainCamera.transform.position);
            rb.useGravity = false; // Disable gravity while dragging
        }
    }

    void OnMouseUp()
    {
        if (rb != null)
        {
            isDragging = false;
            rb.useGravity = true; // Re-enable gravity when released
        }
    }

    void FixedUpdate()
    {
        if (isDragging && rb != null)
        {
            // Calculate the target position based on the mouse position
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = distanceToCamera; // Set the correct distance from the camera
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            // Calculate the force needed to move the object towards the target position
            Vector3 force = (targetPosition - transform.position) * dragSpeed;
            rb.linearVelocity = force; // Apply the calculated force to the Rigidbody
        }
    }
}
