using System.Collections;
using System.Collections.Generic;
using SaveLoadSystem;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerFPSController : MonoBehaviour, IDataPersistence
{
    public GameObject photoCamera; // PhotoCamera GameObject
    bool activeCamera = false; 

    public static PlayerFPSController playerinstance;
    [Header("References")]
    [SerializeField] private Transform cameraPivot;
    [Header("Camera")]
    [SerializeField] private Vector2 mouseSensitivity = new Vector2(2f, 2f);

    [SerializeField] private Vector2 cameraMinMax = new Vector2(-90f, 90f);
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float crouchSpeed = 1f;
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float normalHeight = 2f;
    [SerializeField] private float cameraNormalHeight = 1f;
    [SerializeField] private float cameraCrouchHeight = 0.5f;
    [SerializeField] private float cameraCrouchSpeed = 1f;
    [SerializeField] private float crouchTransitionSpeed = 10f;
    
    [Header("Jump")]
    [SerializeField] private float jumpMultiplier = 6f;
    [SerializeField] private AnimationCurve jumpCurve;
    
    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;

    [Header("LayerMasks")] 
    [SerializeField] private LayerMask levelMask;
    
    private bool _isCrouching;
    private bool _isRunning;
    private bool _isJumping;
    private float _currentSpeed;
    private float _velocityY;
    private float _currentHeight;
    private float _targetHeight;
    private float _currentCameraHeight;
    private float _targetCameraHeight;
    private Vector3 _movement;
    private Vector2 _rotation;
    private CharacterController _controller;
    private bool _canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        if (playerinstance != null)
        {
            Destroy(gameObject);
        }
        playerinstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        MovePlayer();
        ApplyGravity();
        HandleCrouch();

        if (Input.GetKeyUp(KeyCode.F))
        {
            activeCamera = !activeCamera;
            photoCamera.SetActive(activeCamera);
        }

    }

    void GetInputs()
    {
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _rotation.x = Input.GetAxis("Mouse X") * mouseSensitivity.x;
        _rotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity.y;
        _rotation.y = Mathf.Clamp(_rotation.y, cameraMinMax.x, cameraMinMax.y);
        
        bool runInput = Input.GetKey(KeyCode.LeftShift);
        bool crouchInput = Input.GetKey(KeyCode.C);


        if (crouchInput)
        {
            _isCrouching = true;
            _isRunning = false;
        }else if (runInput)
        {
            _isRunning = true;
            _isCrouching = false;
        }
        else
        {
            _isCrouching = false;
            _isRunning = false;
        }

        if (checkAbove())
        {
            _isCrouching = true;
        }
        
        _currentSpeed = _isRunning ? runSpeed : (_isCrouching ? crouchSpeed : walkSpeed);

        if (Input.GetKey(KeyCode.Space) && _controller.isGrounded && !_isJumping)
        {
            StartJump();
        }
    }

    void MovePlayer()
    {
        if (!_canMove) return;
        _controller.Move(Time.deltaTime *  _currentSpeed * transform.TransformDirection(_movement.normalized) + (Time.deltaTime * _velocityY * Vector3.up));
        transform.Rotate(0, _rotation.x, 0);
        cameraPivot.localEulerAngles = new Vector3(_rotation.y, 0, 0);
    }
    void HandleCrouch()
    {
        _targetHeight = _isCrouching ? crouchHeight : normalHeight;
        _targetCameraHeight = _isCrouching ? cameraCrouchHeight : cameraNormalHeight;

        _currentHeight = Mathf.Lerp(_currentHeight, _targetHeight, Time.deltaTime * crouchTransitionSpeed);
        _currentCameraHeight = Mathf.Lerp(_currentCameraHeight, _targetCameraHeight, Time.deltaTime * cameraCrouchSpeed);

        _controller.height = _currentHeight;
        _controller.center = Vector3.up * _currentHeight / 2f;
        cameraPivot.localPosition = Vector3.up * _currentCameraHeight;
    }
    void ApplyGravity()
    {
        if (_controller.isGrounded)
        {
            _velocityY = -2f;
        }
        else
        {
            _velocityY += gravity * Time.deltaTime;
        }
    }

    void StartJump()
    {
        _isJumping = true;
        StartCoroutine(Jump());
    }

    IEnumerator Jump()
    {
        float timeInAir = 0f;
        do
        {
            float jumpForce = jumpCurve.Evaluate(timeInAir) * jumpMultiplier;
            _controller.Move(Vector3.up * jumpForce * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!_controller.isGrounded && _controller.collisionFlags != CollisionFlags.Above);

        _isJumping = false;
    }

    bool checkAbove()
    {
        Debug.DrawRay(transform.position,Vector3.up * (_controller.height + 1f), Color.red);
        bool hasAbove = Physics.Raycast(transform.position,Vector3.up,  normalHeight + 2f, levelMask.value);
        return hasAbove;
    }

    public void TogglePlayerMovement()
    {
        _canMove = !_canMove;
    }

    public void DisablePlayerMovement()
    {
        _canMove = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void EnablePlayerMovement()
    {
        _canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
