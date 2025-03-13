using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPGGame
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 5f;
        private CharacterController _characterController;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private InputActionAsset controllerSettings;
        private InputAction _moveAction;
        private Coroutine _characterStart;
        private Vector2 _currentRotation;
        [SerializeField] private float mouseSensitivity;
        private Vector2 _rotationVelocity;
        [SerializeField] private float rotationSmoothTime = 0.12f;
        [SerializeField] private float maxYAxisUp;
        [SerializeField] private float maxYAxisDown;
        private float _maxYaxis;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _moveAction = controllerSettings.FindActionMap("Player").FindAction("Move");
        }

        private void OnEnable()
        {
            _moveAction.Enable();
        }

        private void Start()
        {
            StartCharacterMovement();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        public void StopCharacterMovement()
        {
            StopCoroutine(_characterStart);
        }

        public void StartCharacterMovement()
        {
            _characterStart = StartCoroutine(StartWalking());
        }

        private IEnumerator StartWalking()
        {
            while (true)
            {
                if (Camera.main != null)
                {
                    Vector3 cameraForward = Camera.main.transform.forward;
                    Vector3 cameraRight = Camera.main.transform.right;

                    cameraForward.y = 0;
                    cameraRight.y = 0;
                    cameraForward.Normalize();
                    cameraRight.Normalize();

                    Vector2 twoDMovement = _moveAction.ReadValue<Vector2>();
                    Vector3 movementWasd = (cameraRight * twoDMovement.x + cameraForward * twoDMovement.y) *
                                           (walkSpeed * Time.deltaTime);
                    Vector2 lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                    _currentRotation.y += lookInput.x * mouseSensitivity;
                    _currentRotation.x -= lookInput.y * mouseSensitivity; // Invert vertical look
                    _currentRotation.x = Mathf.Clamp(_currentRotation.x, -90f, 90f); // Clamp vertical rotation

                    Vector2 smoothRotation = Vector2.SmoothDamp(_currentRotation, _currentRotation,
                        ref _rotationVelocity, rotationSmoothTime);
                    _maxYaxis = Mathf.Clamp(smoothRotation.x, maxYAxisDown, maxYAxisUp);
                    transform.localRotation = Quaternion.Euler(_maxYaxis, smoothRotation.y, 0f);

                    if (_characterController.isGrounded)
                    {
                        movementWasd.y = -0.5f; // Small downward force when grounded
                    }
                    else
                    {
                        movementWasd.y += Physics.gravity.y * Time.deltaTime;
                    }

                    Vector3 finalMovement = movementWasd + new Vector3(0, movementWasd.y * Time.deltaTime, 0);
                    _characterController.Move(finalMovement);
                }

                yield return null;
            }
        }

        private void OnDisable()
        {
            _moveAction.Disable();
        }
    }
}
