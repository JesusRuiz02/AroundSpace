using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerTouchMovement _playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private readonly int animVelocity = Animator.StringToHash("Velocidad");
   [SerializeField] private bool groundedPlayer;
   [SerializeField] private float playerSpeed = 2.0f;
   [SerializeField] private float jumpHeight = 1.0f;
   [SerializeField] private float gravityValue = -9.81f;
   [SerializeField] private float rotationSpeed = 4f;
   private Animator _animator = default;
   private Transform _cameraMain = default;
   private Transform _child = default;

   private void Awake()
    {
        _playerInput = new PlayerTouchMovement();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Start()
    {
        _cameraMain = Camera.main.transform;
        _child = transform.GetChild(0).transform;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 MovementInput = _playerInput.PlayerMain.Move.ReadValue<Vector2>();
        Vector3 move = (_cameraMain.forward * MovementInput.y + _cameraMain.right * MovementInput.x);
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);
        if (_playerInput.PlayerMain.Jump.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        _animator.SetInteger(animVelocity,Mathf.FloorToInt(MovementInput.y));
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (MovementInput != Vector2.zero)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(_child.localEulerAngles.x,
                _cameraMain.localEulerAngles.y, _child.localEulerAngles.z));
            _child.rotation = Quaternion.Lerp(_child.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
}
