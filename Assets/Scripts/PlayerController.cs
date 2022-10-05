using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private PlayerTouchMovement _playerInput;
    private CharacterController controller;
    private Animator _animator = default;
    
    [Header("Players Stats")]
    [SerializeField] private bool _groundedPlayer; 
    private float _rotationSpeed = 15f; 
    private int _gravity = 25; 
    private float _movespeed = 4; 
    private float _jumpForce = 10f;
    [SerializeField] private Transform _groundCheckPostition;
    [SerializeField] private float _radiusdetection = 0.1f;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private GameObject _buttonDodge = default;
    [Header("Scripts")] 
    private PlayerHealth _playerHealth;
    private ButtonTransparency _buttonTransparency;
    private PlayerStats _playerStats;
    private OnScreenButton _onScreenButton;
    
    [Header("Other")]
    private Vector3 playerVelocity;
    private Transform _cameraMain = default;
    private bool isDodging = false;
    private float dodgeTimer = default;
    private bool _uCanDodge = true;
    private float velocityY = default;
    
    private Vector2 MovementInput;
    private Vector3 direction;
    private float _dodgeTime = default;
    [Header("AnimationStuff")]
    [SerializeField] AnimationCurve dodgeCurve;

    private void Awake()
    {
        _playerInput = new PlayerTouchMovement();
        controller = GetComponent<CharacterController>();
        _playerStats = GetComponent<PlayerStats>();
        _playerHealth = GetComponent<PlayerHealth>();
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
        _animator = GetComponent<Animator>();
        dodgeTimer = 1;
        _onScreenButton = _buttonDodge.GetComponent<OnScreenButton>();
        _buttonTransparency = _buttonDodge.GetComponent<ButtonTransparency>();
        _movespeed = _playerStats.PlayerSpeed;
        _jumpForce = _playerStats.JumpForce;
        _dodgeTime = _playerStats.DodgeTimer;
        _rotationSpeed = _playerStats.RotateSpeed;
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
            if (_uCanDodge)
            {
                if (direction.magnitude != 0) //Only if the character is moving can dodge
                {
                    StartCoroutine(Dodge()); 
                    StartCoroutine(ResetTimeDodge());
                    StartCoroutine(_playerHealth.InmunityTime(dodgeTimer));
                }
            }
        }
        bool activeState = _groundedPlayer;
        StateButton(activeState);
        if (_playerInput.PlayerMain.Damage.triggered)
        {
            _playerHealth.TakeDamage(34);
        }

        if (_playerInput.PlayerMain.Attack.triggered)
        {
            PlayerAttack();
        }
    }

    void PlayerAttack()
    {
       // _animator.SetInteger("AttackSystem",1);
       _animator.SetTrigger("Attack");
    }

    void StateButton(bool activateState)
    {
        _onScreenButton.enabled = activateState;
        var transparentValue = _groundedPlayer ? 255 : 127; //Define the transparency of the button
        var byteNumber =  Convert.ToByte(transparentValue); //Change int unit to byte 
        _buttonTransparency.Transparentbutton(byteNumber);
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

    private IEnumerator ResetTimeDodge()
    {
        _uCanDodge = false;
        yield return new WaitForSeconds(_dodgeTime);
        _uCanDodge = true;
    }
}