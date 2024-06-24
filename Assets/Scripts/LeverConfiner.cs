using System;
using UnityEngine;

public class LeverConfiner : MonoBehaviour
{
    private Camera mainCamera;
    private bool isPicked = false;
    private Vector3 offset;

    public float moveSpeed = 10f;
    public float defaultDistance = 3f;
    public float scrollSensitivity = 1f;
    private float currentDistance;

    void Start()
    {
        mainCamera = Camera.main;
        currentDistance = defaultDistance;
    }

    void Update()
    {
        if (isPicked)
        {
            AdjustDistance();
            MoveObject();
        }
    }

    void OnMouseDown()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isPicked = true;
                offset = gameObject.transform.position - hit.point;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().freezeRotation = true;
            }
        }
    }

    void OnMouseUp()
    {
        if (isPicked)
        {
            isPicked = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().freezeRotation = false;
        }
    }

    void MoveObject()
    {
        Vector3 targetPosition = mainCamera.transform.position + mainCamera.transform.forward * currentDistance;
        GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime));
    }

    void AdjustDistance()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance += scroll * scrollSensitivity;
        currentDistance = Mathf.Clamp(currentDistance, 1f, 10f); // Limite la distance pour éviter les valeurs extrêmes
    }
}
