using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPGGame
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 5f;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private InputActionAsset controllerSettings;
        private InputAction _moveAction;
        private Coroutine _characterStart;
        private InputAction _sprintAction;
        private Vector2 _currentRotation;
        [SerializeField] private float mouseSensitivity;
        private Vector2 _rotationVelocity;
        [SerializeField] private float rotationSmoothTime = 0.12f;
        [SerializeField] private float maxYAxisUp;
        [SerializeField] private float maxYAxisDown;
        private float _maxYaxis;
        [SerializeField] private Animator characterAnimation;
        [SerializeField] private string movementParamName = "Speed";
        private float currentSpeed = 0f;

        private void Awake()
        {
            //_characterController = GetComponent<CharacterController>();
            _moveAction = controllerSettings.FindActionMap("Player").FindAction("Move");
            _sprintAction = controllerSettings.FindActionMap("Player").FindAction("Sprint");
        }

        private void OnEnable()
        {
            _moveAction.Enable();
            if (_sprintAction != null) _sprintAction.Enable();
        }

        private void Start()
        {
            StartCharacterMovement();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            _currentRotation = new Vector2(0f, transform.eulerAngles.y);
        }

        public void StopCharacterMovement()
        {
            if (_characterStart != null)
                StopCoroutine(_characterStart);
                
            // Reset animation when stopping
            if (characterAnimation != null)
                characterAnimation.SetFloat(movementParamName, 0);
        }

        public void StartCharacterMovement()
        {
            _characterStart = StartCoroutine(StartWalking());
        }

        private IEnumerator StartWalking()
        {
            transform.localRotation = Quaternion.Euler(0f, _currentRotation.y, 0f);
            yield return null;

            // Create a velocity vector to track vertical movement
            Vector3 verticalVelocity = new Vector3(0, -0.5f, 0);

            while (true)
            {
                if (Camera.main != null)
                {
                    // Camera direction vectors
                    Vector3 cameraForward = Camera.main.transform.forward;
                    Vector3 cameraRight = Camera.main.transform.right;

                    cameraForward.y = 0;
                    cameraRight.y = 0;
                    cameraForward.Normalize();
                    cameraRight.Normalize();

                    // Input handling
                    Vector2 twoDMovement = _moveAction.ReadValue<Vector2>();
                    bool isSprinting = _sprintAction != null && _sprintAction.ReadValue<float>() > 0.5f;
                    float currentMoveSpeed = isSprinting ? runSpeed : walkSpeed;

                    // Mouse look
                    Vector2 lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                    _currentRotation.y += lookInput.x * mouseSensitivity;
                    _currentRotation.x -= lookInput.y * mouseSensitivity;
                    _currentRotation.x = Mathf.Clamp(_currentRotation.x, maxYAxisDown, maxYAxisUp);

                    // Apply rotation
                    transform.parent.localRotation = Quaternion.Euler(0, _currentRotation.y, 0);
                    Camera.main.transform.localRotation = Quaternion.Euler(_currentRotation.x, 0, 0);

                    // Calculate horizontal movement
                    Vector3 moveDirection = (cameraRight * twoDMovement.x + cameraForward * twoDMovement.y).normalized;
                    Vector3 horizontalMovement = moveDirection * (currentMoveSpeed * Time.deltaTime);

                    // Handle animation
                    if (characterAnimation != null)
                    {
                        float targetSpeed = twoDMovement.magnitude <= 0.1f ? 0 : (isSprinting ? 1f : 0.5f);
                        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 10f);
                        characterAnimation.SetFloat(movementParamName, currentSpeed);
                    }

                    // Apply gravity properly
                    if (_characterController.isGrounded)
                    {
                        // Reset vertical velocity when grounded
                        verticalVelocity.y = -0.5f;
                    }
                    else
                    {
                        // Apply gravity to vertical velocity
                        verticalVelocity.y += gravity * Time.deltaTime;
                    }

                    // Combine horizontal movement with vertical velocity
                    Vector3 movement = new Vector3(horizontalMovement.x, verticalVelocity.y * Time.deltaTime,
                        horizontalMovement.z);

                    // Move character
                    _characterController.Move(movement);
                }

                yield return null;
            }
        }
        
        private void OnDisable()
        {
            _moveAction.Disable();
            if (_sprintAction != null) _sprintAction.Disable();
        }
    }
}
