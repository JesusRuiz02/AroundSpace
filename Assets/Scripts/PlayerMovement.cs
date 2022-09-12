using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private readonly int ahVelocidad = Animator.StringToHash("Velocidad");
    [SerializeField] private CharacterController _characterController = default;
    [SerializeField] private bool _isGround = default;
    [SerializeField] private Vector3 _playerVelocity = default;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _jumpHeight = 1.0f;
    [SerializeField] private float _gravityValue = -9.81f;
    [SerializeField] private Transform _checkGroundPosition = default;
    [SerializeField] private float _checkGroundRadius = 0.1f;
    [SerializeField] private LayerMask _layerMask = default;
    private void FixedUpdate()
    {   float zaxis = Input.GetAxis("Vertical"); //Input hacia adelante
        float xaxis = Input.GetAxis("Horizontal");//Input para rotar la cámara
        Vector3 move = transform.forward * zaxis;
        _characterController.transform.Rotate(Vector3.up * xaxis * (100f * Time.deltaTime));
        if (_isGround && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftShift))
        { 
            anim.SetInteger(ahVelocidad, Mathf.FloorToInt(zaxis) * 2 );
            _characterController.Move(move * Time.deltaTime * _speed);
        }
        else
        {
            anim.SetInteger( ahVelocidad, Mathf.FloorToInt(zaxis)); 
            _characterController.Move(move * Time.deltaTime * _speed);
        }
    }

    private void Update()
    {
        Jump();
        if (Input.GetKeyDown(KeyCode.Space) && _isGround)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }
    }

    private void Jump()
    {
        _isGround = false;
        Collider[] colliders = Physics.OverlapSphere(_checkGroundPosition.position, _checkGroundRadius, _layerMask);
        if (colliders.Length > 0)
        {
            _isGround = true;
        }
    }
}
