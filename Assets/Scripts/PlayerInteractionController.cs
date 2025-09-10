using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float maxInteractionDistance = 10f;
    [SerializeField] private string interactableTag = "Interactable";
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionRadius = 0.5f;
    [SerializeField] private PlayerInput playerInput;
    
    [Header("Interaction Events")]
    [SerializeField] private UnityEvent<GameObject> onInteractableDetected;
    [SerializeField] private UnityEvent<GameObject> onInteractableLost;
    [SerializeField] private UnityEvent<GameObject> onInteractableClicked;
    [SerializeField] private UnityEvent<GameObject> onInteractableReleased;
    
    

    private InputAction _interactAction;
    
    private GameObject currentInteractableObject;
    private bool interacted = false;
    
    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        
        _interactAction = playerInput.actions["Interact"];
        _interactAction.started += OnInteractStarted;
        _interactAction.canceled += OnInteractCanceled;
    }
    
    private void Update()
    {
        DetectInteractableObject();
    }
    
    private void DetectInteractableObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.SphereCast(ray, interactionRadius, out hit, maxInteractionDistance, interactableLayerMask))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag(interactableTag))
            {
                if (hitObject != currentInteractableObject)
                {
                    currentInteractableObject = hitObject;
                    onInteractableDetected?.Invoke(currentInteractableObject);
                }
                return;
            }
        }

        if (currentInteractableObject != null)
        {
            onInteractableLost?.Invoke(currentInteractableObject);
            currentInteractableObject = null;
        }
    }
    
    private void OnInteractStarted(InputAction.CallbackContext context)
    {
        if (!currentInteractableObject)
        {
            return;
        }
        
        if (currentInteractableObject.TryGetComponent<IInteractable>(out var interactable))
        {
            interactable.OnClicked(this.gameObject);
            onInteractableClicked?.Invoke(currentInteractableObject);
            interacted = true;
        }
    }
    
    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        if (!currentInteractableObject || !interacted)
        {
            return;
        }
        
        if (currentInteractableObject.TryGetComponent<IInteractable>(out var interactable))
        {
            interactable.OnRelease(this.gameObject);
            onInteractableReleased?.Invoke(currentInteractableObject);
            interacted = false;
        }
    }
}