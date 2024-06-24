using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    
    [SerializeField]
    private PlayerInput playerInput;
    
    [SerializeField]
    private CharacterController characterController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        
        playerInput.actions["Move"].performed += Move;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        characterController.Move(move);
    }
}
