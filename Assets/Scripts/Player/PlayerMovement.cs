using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private PlayerTouchMovement _playerInput;
    private CharacterController controller;
    private Animator _animator = default;

    [Header("Players Stats")] 
    [SerializeField] private float rotatespeed = 15f;
    [SerializeField] private int gravity = 25;
    [SerializeField] private float movespeed = 4;

    private float velocityY = default;
    
    [Header("Other")]
    private Vector3 playerVelocity;
    private Transform _cameraMain = default;

    private Vector2 MovementInput;
    private Vector3 direction;

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
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        RecordControls();
        PlayerMovements();
        PlayerRotation();
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
    private void PlayerMovements()
    {
        velocityY -= Time.deltaTime * gravity;
        velocityY = Mathf.Clamp(velocityY, -10, 10);
        Vector3 fallVelocity = Vector3.up * velocityY;
        Vector3 velocity = (direction * movespeed) + fallVelocity;
        
        controller.Move(velocity * Time.deltaTime);
    }
    void PlayerRotation()
    {
        if (direction.magnitude == 0) return;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction),rotatespeed * Time.deltaTime);
    }

}
