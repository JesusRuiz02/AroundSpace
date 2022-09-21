using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private PlayerTouchMovement _playerInput;
    private CharacterController controller;
    private Rigidbody _rigidbody = default;
    private Animator _animator = default;
    
    [Header("Players Stats")]
    [SerializeField] private bool _groundedPlayer;
    [SerializeField] private float _rotationSpeed = 15f;
    [SerializeField] private int _gravity = 25;
    [SerializeField] private float _movespeed = 4;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private Transform _groundCheckPostition;
    [SerializeField] private float _radiusdetection = 0.1f;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private GameObject _buttonDodge = default;

    private float velocityY = default;
    
    [Header("Other")]
    private Vector3 playerVelocity;
    private Transform _cameraMain = default;
    private Transform _child = default;
    private bool isDodging = false;
    private float dodgeTimer = default;

    private Vector2 MovementInput;
    private Vector3 direction;

    [Header("AnimationStuff")]
    [SerializeField] AnimationCurve dodgeCurve;

    private OnScreenButton _onScreenButton;

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
        dodgeTimer = 1;
        Debug.Log(dodgeTimer);
        _buttonDodge.GetComponent<MeshRenderer>();
       _onScreenButton = _buttonDodge.GetComponent<OnScreenButton>();
    }

    void Update()
    {
        RecordControls();
        GroundCheck();
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
        bool activeState = _groundedPlayer;
        StateDodgeButton(activeState);
     /*   if (_groundedPlayer)
        {
            TransparentImage(0.5f);
        }
        else
        {
            TransparentImage(1f);
        }
       */
    }

    void StateDodgeButton(bool activateState)
    {
        _onScreenButton.enabled = activateState;
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
        if (_groundedPlayer)
        {
            velocityY = -_gravity * Time.deltaTime;
            if (_playerInput.PlayerMain.Jump.triggered)
            {
                velocityY = _jumpForce;
            }
        }
        else
        {
            velocityY -= _gravity * Time.deltaTime;
        }
        velocityY = Mathf.Clamp(velocityY, -10, 10);
        Vector3 fallVelocity = Vector3.up * velocityY;
        Vector3 velocity = (direction * _movespeed) + fallVelocity;
        
        controller.Move(velocity * Time.deltaTime);
    }
    void PlayerRotation()
    {
        if (direction.magnitude == 0) return;
        float rs = _rotationSpeed;
        if (isDodging) rs = 3;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction),rs * Time.deltaTime);
    }

    private void GroundCheck()
    {
        _groundedPlayer = false;
        Collider[] collidersGround = Physics.OverlapSphere(_groundCheckPostition.position, _radiusdetection, _whatIsGround);
        if (collidersGround.Length > 0)
        {
            _groundedPlayer = true;
        }
    }

   /* void TransparentImage(float transparetValue)
    { Image imageButton = _buttonDodge.gameObject.GetComponent<Image>();
     var tempColor = imageButton.tintColor;
     tempColor.a = transparetValue;
     imageButton.tintColor = tempColor;
    }*/
}