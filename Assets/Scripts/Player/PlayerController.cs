using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private PlayerTouchMovement _playerInput;
    private CharacterController controller;
    [SerializeField] private Instructions _instructions;
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
    [SerializeField] private GameObject _attackButton = default;

    [Header("Scripts")] 
    private PlayerHealth _playerHealth;
    private ButtonTransparency _buttonTransparency;
    private PlayerStats _playerStats;
    private OnScreenButton _onScreenButton;
    private InvisiblePlayer _invisiblePlayer;
    
    [Header("Other")]
    private Vector3 playerVelocity;
    private Transform _cameraMain = default;
    private bool isDodging = false;
    private bool isAttacking = false;
    private float dodgeTimer = default;
    private bool _uCanDodge = true;
    private float velocityY = default;
    private float _invisibleCounter = default;
    private int _maxInvisibleCounter;
    private Vector2 MovementInput;
    private Vector3 direction;
    private float _dodgeTime = default;
    [SerializeField] private AudioClip _punchSFX;
    [SerializeField] private AudioClip _kickSFX;
    [SerializeField] private AudioClip _finalKickSFX;
    [SerializeField] private AudioClip _backgroundMusic;
    

    [Header("SystemAttack")] 
    [SerializeField] private float _noAttack;
    private float _attackSpeed = 4;
    private bool _canAttack = true;
  
    
    [Header("AnimationStuff")]
    [SerializeField] AnimationCurve dodgeCurve;

    [SerializeField]
    private TextMeshProUGUI _invisibleText;

    private void Awake()
    {
        _playerInput = new PlayerTouchMovement();
        controller = GetComponent<CharacterController>();
        _playerStats = GetComponent<PlayerStats>();
        _playerHealth = GetComponent<PlayerHealth>();
        _invisiblePlayer = GetComponent<InvisiblePlayer>();
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
        _movespeed = _playerStats.PlayerSpeed;
        _jumpForce = _playerStats.JumpForce;
        _dodgeTime = _playerStats.DodgeTimer;
        _rotationSpeed = _playerStats.RotateSpeed;
        AudioManager.instance.PlayMusic(_backgroundMusic);
    }

    private void FixedUpdate()
    {
       if (!_uCanDodge || !_groundedPlayer) 
       {
            StateButton(false, _buttonDodge);
       }
       else
       {
            StateButton(true, _buttonDodge);
       }
    }

    void Update()
    {
        RecordControls();
        GroundCheck();
        if (!isDodging && !isAttacking)
        {
            PlayerMovement();
        }
        PlayerRotation();
        if (_playerInput.PlayerMain.Dodge.triggered)
        {
            if (_uCanDodge && _groundedPlayer)
            {
                if (direction.magnitude != 0) //Only if the character is moving can dodge
                {
                    StartCoroutine(Dodge()); 
                    StartCoroutine(ResetTimeDodge());
                    StartCoroutine(_playerHealth.InmunityTime(dodgeTimer));
                }
            }
        }
     
        if (_playerInput.PlayerMain.Invisble.triggered)
        {
            BecomeInvisible();
        }
        if (_playerInput.PlayerMain.Attack.triggered)
        {
            SystemCombo();
        }
    }

    #region Principal Movement
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
    

    #endregion

    #region Advanced Movement
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

    private IEnumerator Dash()
    {
        isAttacking = true;
        float timer = 0;
        while (timer < 0.5f)
        {
            Vector3 dir = (transform.forward * _attackSpeed) + (Vector3.up * velocityY);
            controller.Move(dir * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        isAttacking = false;
    }

    private void StopDash()
    {
        StopCoroutine(Dash());
    }
  
    private IEnumerator ResetTimeDodge()
    {
        _uCanDodge = false;
        yield return new WaitForSeconds(_dodgeTime);
        _uCanDodge = true;
    }

    #endregion

    void StateButton(bool activateButton, GameObject ButtonDisable)
    {
        _onScreenButton = ButtonDisable.GetComponent<OnScreenButton>(); 
        _buttonTransparency = ButtonDisable.GetComponent<ButtonTransparency>();
       _onScreenButton.enabled = activateButton;
        var transparentValue = activateButton ? 255 : 127; //Define the transparency of the button
        var byteNumber =  Convert.ToByte(transparentValue); //Change int unit to byte s
        _buttonTransparency.Transparentbutton(byteNumber);
    }

    private void BecomeInvisible()
    {
        if (_invisibleCounter > 0)
        {
            StartCoroutine(_invisiblePlayer.Invisible());
            _invisibleCounter--;
            _invisibleText.text = _invisibleCounter.ToString("0");
        }
    }

    public void AddInvisibleCounter()
    {
        _invisibleCounter++;
        _maxInvisibleCounter++;
        if (_maxInvisibleCounter == 3)
        {
            StartCoroutine(_instructions.LastInstructions());
        }
        _invisibleText.text = _invisibleCounter.ToString("0");
    }
    
    #region Combo
    private void SystemCombo()
    {
        if (_canAttack)
        {
            _noAttack++;
        }
       
        if (_noAttack == 1)
        {
            _animator.SetInteger("AttackSystem",1);
        }
    }

    public void ComboCheck()
    {
        _canAttack = false;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Hit1") && _noAttack == 1)
        {
            _animator.SetInteger("AttackSystem",0);
            AudioManager.instance.PlaySFX(_punchSFX);
            _canAttack = true;
            _noAttack = 0;
        }
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Hit1") && _noAttack >= 2)
        {
            _animator.SetInteger("AttackSystem",2);
            AudioManager.instance.PlaySFX(_kickSFX);
            _canAttack = true;
        }
        else if ( _animator.GetCurrentAnimatorStateInfo(0).IsName("Hit2") && _noAttack == 2)
        {
            _animator.SetInteger("AttackSystem",0);
            _canAttack = true;
            _noAttack = 0;
        }
        else if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Hit2") && _noAttack >= 3)
        {
            _animator.SetInteger("AttackSystem",3);
            AudioManager.instance.PlaySFX(_finalKickSFX);
        }
        else if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Hit3"))
        {
            _animator.SetInteger("AttackSystem",0);
            _canAttack = true;
            _noAttack = 0;
        }
    }
    

    #endregion
    
}