using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player Stats")] 
    //Stand/Crouch Heights
    private float _standHeight = 1f;
    private float _crouchHeight = 0.5f;
    
    //Move
    private float _walkSpeed = 5f;
    private float _crouchSpeed = 2f;
    private float _sprintSpeed = 8f;
    
    //Physics
    private float _gravity = -18.62f;
    private float _jumpStrength = 2f;
    
    [Header("Components")]
    [SerializeField] private Camera _camera;
    [SerializeField] private CharacterController _controller;

    private Transform _playerBody;


    //Statistics to view in Inspector during runtime, also represents the current stats the player uses in code
    [Header("Runtime Info")] 
    [SerializeField] private float _curTargetSpeed;
    

    [SerializeField] private float _curHeight;
    
    [SerializeField]private bool _isGrounded;
    [SerializeField] private float _verticalVelocity;
    
    //Shows wether the character should be able to move at this moment --> should not be able to move if if inventory
    [SerializeField] private bool _canMove = true;
    [SerializeField] private PlayerPoseStateEnum _playerPoseState;


    private float _xRotation;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        _curTargetSpeed = _walkSpeed;
        _curHeight = _standHeight;
        _playerPoseState = PlayerPoseStateEnum.Standing;
        
        //mouse sensitivity
        

        //Fallbacks if components are not set yet
        _playerBody = this.transform;
        
        if (_camera == null)
        {
            //try to find camera
            _camera = gameObject.GetComponentInChildren<Camera>();
            if(_camera == null) Debug.LogError("Camera not found");
        }
        if (_controller == null)
        {
            _controller = gameObject.GetComponentInChildren<CharacterController>();
            if(_controller == null) Debug.LogError("CharacterController not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //can move if not in inventory and not in escape menu
        _canMove = (!InputRouter.instance.InventoryPressed && !InputRouter.instance.EscapePressed);
        if (_canMove)
        {
            Cursor.lockState = CursorLockMode.Locked;
            //crouch
            HandleCrouch();
            //sprint
            HandleSprint();
        
            //look
            HandleLook();
        
            //move
            HandleMovement();    
            
            //Jump
            HandleJump();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    

    /*
     * Handles the mouse movement for the camera
     * Rotates the player/camera to respond to mouse movements
     */
    private void HandleLook()
    {
        //Get look input from InputRouter
        Vector2 lookInput = InputRouter.instance.Look;

        float _lookSensitivity = GameManager.instance.mouseSensitivity;
        //Calculate Mouse Movement
        float mouseX = lookInput.x * _lookSensitivity;
        float mouseY = lookInput.y * _lookSensitivity;

        //Get Rotation of player
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); //limit rotation of camera

        //rotate camera and player
        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        this.transform.Rotate(Vector3.up * mouseX);
    }
    
    private void HandleMovement()
    {
        Vector2 storedInput = InputRouter.instance.Move;
        Vector3 moveDirection = (_playerBody.right * storedInput.x + _playerBody.forward * storedInput.y).normalized;
        _controller.Move(moveDirection * _curTargetSpeed * Time.deltaTime);

        //Apply Gravity
        _verticalVelocity += _gravity * Time.deltaTime;

        //Keep grounded
        if (_isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f; // small downward velocity to keep player grounded
        }

        _controller.Move(new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);
    }
    
    //Set player speed to sprint speed if not crouched already
    private void HandleSprint()
    {
        if (_playerPoseState != PlayerPoseStateEnum.Standing) return;

        if (InputRouter.instance.SprintPressed)
        {
            _curTargetSpeed = _sprintSpeed;    
        }
        else
        {
            _curTargetSpeed = _walkSpeed;
        }
        

    }

    //if player wants to crouch set height, speed and PlayerPoseState
    private void HandleCrouch()
    {
        if (InputRouter.instance.CrouchPressed)
        {
            _playerPoseState = PlayerPoseStateEnum.Crouched;
            _curTargetSpeed = _crouchSpeed;
            SetPlayerHeight(_crouchHeight);
        }
        else
        {
            _playerPoseState = PlayerPoseStateEnum.Standing;
            _curTargetSpeed = _walkSpeed;
            SetPlayerHeight(_standHeight);
        }
    }
    
    //jump if grounded and not crouched
    private void HandleJump()
    {
        //only jump if standing
        if (_playerPoseState != PlayerPoseStateEnum.Standing) return;
        
        _isGrounded = _controller.isGrounded;
        if (InputRouter.instance.JumpPressed)
        {
            if (_isGrounded)
            {
                _verticalVelocity = Mathf.Sqrt(_jumpStrength * -2f * _gravity);
            }
        }
    }
    
    
    //Set player LocalScale to target height
    private void SetPlayerHeight(float targetHeight)
    {
        this.transform.localScale = new Vector3(1f, targetHeight, 1f);
    }
}

enum PlayerPoseStateEnum 
{
    Crouched, Standing
}
