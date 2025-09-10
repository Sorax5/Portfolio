using System;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public float maxGrabDistance = 10f;
    public float minGrabDistance = 1.5f;
    
    public float scrollSpeed = 2f;
    public float dragSpeed = 10f;
    public string grabbableTag = "Grabbable";
    public FirstPersonController playerController;
    
    private Camera playerCamera;
    public GameObject grabbedObject;
    private float currentGrabDistance;
    
    public bool isDragging = false;
    
    private Action OnObjectDetected;
    private Action OnObjectLost;

    void Start()
    {
        playerCamera = Camera.main;
        
        OnObjectDetected += ObjectDetected;
        OnObjectLost += ObjectLost;
    }

    private void ObjectDetected()
    {
        Debug.Log("Object detected");
        playerController.crosshairColor = Color.green;
    }

    private void ObjectLost()
    {
        Debug.Log("Object lost");
        playerController.crosshairColor = Color.white;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnMouseUp();
        }
        
        if (Input.GetMouseButtonDown(2) && grabbedObject != null) // 2 correspond au clic droit de la souris (molette)
        {
            ResetGrabbedObject();
        }

        if (!isDragging)
        {
            TryGrabObject();
        }
    }

    private void ResetGrabbedObject()
    {
        grabbedObject.transform.rotation = Quaternion.identity;
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        rb.angularVelocity = Vector3.zero;
    }

    void OnMouseDown()
    {

        if (grabbedObject != null)
        {
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            isDragging = true;
            currentGrabDistance = Vector3.Distance(grabbedObject.transform.position, playerCamera.transform.position);
            rb.useGravity = false;
        }
    }
    
    void OnMouseUp()
    {
        if (grabbedObject != null)
        {
            isDragging = false;
            Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            ReleaseObject();
        }
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            AdjustGrabbedObjectDistance();
            MoveGrabbedObject();
        }
    }

    void TryGrabObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(ray, out hit, maxGrabDistance);

        if (hitSomething && hit.collider.gameObject.CompareTag(grabbableTag))
        {
            GameObject gob = hit.collider.gameObject;

            if (grabbedObject == null || grabbedObject != gob)
            {
                // Object detected
                if (grabbedObject == null)
                {
                    OnObjectDetected?.Invoke();
                }
                grabbedObject = gob;
            }
            else if (grabbedObject != null && grabbedObject == gob)
            {
                // Object still the same
            }
        }
        else
        {
            // No valid object detected
            if (grabbedObject != null)
            {
                // Object lost
                OnObjectLost?.Invoke();
                grabbedObject = null; // Reset grabbedObject when nothing is hit
            }
        }
    }


    void ReleaseObject()
    {
        grabbedObject = null;
    }

    void MoveGrabbedObject()
    {
        Transform gTransform = grabbedObject.transform;
        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
        
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = currentGrabDistance;
        Vector3 targetPosition = playerCamera.ScreenToWorldPoint(mousePosition);

        // Calculate the force needed to move the object towards the target position
        Vector3 force = (targetPosition - gTransform.position) * dragSpeed;
        rb.linearVelocity = force; // Apply the calculated force to the Rigidbody
    }

    void AdjustGrabbedObjectDistance()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            currentGrabDistance = Mathf.Clamp(currentGrabDistance + scroll * scrollSpeed, minGrabDistance, maxGrabDistance);
        }
    }
}
