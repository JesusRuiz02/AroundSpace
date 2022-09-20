using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private PlayerTouchMovement _playerInput;
    private CharacterController controller;
    private Rigidbody _rigidbody = default;
    private Animator _animator = default;
    
    [Header("Players Stats")]
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float pushAmnt = 3f;
    [SerializeField] private float rotatespeed = 15f;
    [SerializeField] private int gravity = 25;
    [SerializeField] private float movespeed = 4;

    private float velocityY = default;
    
    [Header("Other")]
    private Vector3 playerVelocity;
    private readonly float Velocidad = Animator.StringToHash("VelocidadHor");
    private Transform _cameraMain = default;
    private Transform _child = default;
    private bool isDodging = false;
    private float dodgeTimer = default;

    private Vector2 MovementInput;
    private Vector3 direction;

    [Header("AnimationStuff")]
    [SerializeField] AnimationCurve dodgeCurve;

    private void Awake()
    {
        _playerInput = new PlayerTouchMovement();
        controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
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
        Keyframe dodge_lastFrame = dodgeCurve[dodgeCurve.length - 1];
        dodgeTimer = dodge_lastFrame.time;
    }

    void Update()
    {
        RecordControls();
        if (!isDodging)
        {
            PlayerMovement();
        }
        PlayerRotation();
        if (_playerInput.PlayerMain.Dodge.triggered)
        {
            if (direction.magnitude != 0)
            {
                StartCoroutine(Dodge()); //Only if the character is moving can dodge
            }
        }
    }

    private void JumpAction()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

    private IEnumerator Dodge()
    {
        _animator.SetTrigger("Dodge");
        isDodging = true;
        float timer = 0;
        while (timer < dodgeTimer)
        {
            float speed = dodgeCurve.Evaluate(timer);
            Vector3 dir = (transform.forward * speed) + (Vector3.up * velocityY);
            controller.Move(dir * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        isDodging = false;
    }
    private void RecordControls()
    {
        MovementInput = _playerInput.PlayerMain.Move.ReadValue<Vector2>();
        Vector3 forward = _cameraMain.forward;
        Vector3 right = _cameraMain.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        direction = (right * MovementInput.x + forward * MovementInput.y).normalized;
        _animator.SetFloat("Movement", direction.magnitude, 0.1f, Time.deltaTime);
    }
    private void PlayerMovement()
    {
        velocityY = Time.deltaTime * gravity;
        velocityY = Mathf.Clamp(velocityY, -10, 10);
        Vector3 fallVelocity = Vector3.up * velocityY;
        Vector3 velocity = (direction * movespeed) + fallVelocity;
        
        controller.Move(velocity * Time.deltaTime);
    }
    void PlayerRotation()
    {
        if (direction.magnitude == 0) return;
        float rs = rotationSpeed;
        if (isDodging) rs = 3;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction),rs * Time.deltaTime);
        
    }

    void NewInputSystemMove()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector3 move = (_cameraMain.forward * MovementInput.y + _cameraMain.right * MovementInput.x);
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);
        if (_playerInput.PlayerMain.Jump.triggered && groundedPlayer)
        {
            JumpAction();
        }
        _animator.SetFloat("VelocidadVer",move.z);
        _animator.SetFloat("VelocidadHor",move.x);
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
