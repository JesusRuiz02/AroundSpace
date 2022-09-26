using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _playerspeed = 4;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private float _dodgeTimer = 3f;

    [SerializeField] private float _rotateSpeed = 15f;

    public float DodgeTimer => _dodgeTimer;

    public float Health => _health;
    public float PlayerSpeed => _playerspeed;
    public float JumpForce => _jumpForce;
    public float RotateSpeed => _rotateSpeed;
}
