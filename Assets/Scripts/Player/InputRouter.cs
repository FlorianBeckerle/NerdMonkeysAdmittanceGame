using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputRouter : MonoBehaviour
{
    
    //Singleton
    public static InputRouter instance;
    
    
    [Header("Inputs")] 
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    
    //Hold
    public bool SprintPressed { get; private set; }
    public bool CrouchPressed { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool AttackPressed { get; private set; }
    
    //Event
    //public UnityEvent InteractPressed;
    
    //Toggle
    public bool ControlsPressed { get; private set; }
    public bool InventoryPressed { get; private set; }
    public bool EscapePressed { get; private set; }


    //Make Input-Router a singleton because it should only exist once
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        
        //initialize Events
        
        //InteractPressed = new UnityEvent();
        
        //Set all inputs to false for start
        ControlsPressed = false;
        SprintPressed = false;
        CrouchPressed = false;
        JumpPressed = false;
        AttackPressed = false;
        EscapePressed = false;

    }
    
    /*
     * ======================
     *     Player Events
     * ======================
     */

    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log("Move performed");
        Move = context.ReadValue<Vector2>();
        
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        //Debug.Log("Look performed");
        Look = context.ReadValue<Vector2>();
    }
    
    public void OnCrouch(InputAction.CallbackContext context)
    {
        //Debug.Log("Crouch performed");
        //CrouchPressed = context.ReadValueAsButton();
    }
    
    public void OnSprint(InputAction.CallbackContext context)
    {
        //Debug.Log("Sprint performed");
        SprintPressed = context.ReadValueAsButton();
    }
    
    //Events to trigger specific actions in scripts --> other scripts will subscribe to these events if needed
    public void OnAttack(InputAction.CallbackContext context)
    {
        //Debug.Log($"Attack {context.phase} at {Time.time}");
        if (context.started)
        {
            AttackPressed = true;    
        }

        if (context.canceled)
        {
            AttackPressed = false;
        }
        
    }
    
    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interact performed");
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        //Debug.Log("Jump performed");
        JumpPressed = context.ReadValueAsButton();
        
    }
    
    //Toggle inputs like inventory and controls (otherwise you would need to hold the key down to stay in the inventory)
    public void OnControls(InputAction.CallbackContext context)
    {
        Debug.Log("Controls performed");
        if (context.started)
        {
            ControlsPressed = !ControlsPressed;    
        }
        
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        Debug.Log("Inventory performed");
        if (context.started)
        {
            InventoryPressed = !InventoryPressed;
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        Debug.Log("Escape performed");
        if (context.started)
        {
            EscapePressed = !EscapePressed;
        }
    }
    
    
}
