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
    
    void Update()
    {
        float velocidad = Input.GetAxis("Vertical");
        float zaxis = Input.GetAxis("Vertical");
        float xaxis = Input.GetAxis("Horizontal");
        _isGround = _characterController.isGrounded;
        Vector3 move = transform.forward * zaxis;
        _characterController.transform.Rotate(Vector3.up * xaxis * (100f * Time.deltaTime));
        if (_isGround && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }
      
        if (Input.GetKeyDown(KeyCode.Space) && _isGround)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }
      
        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
      
       // if (Input.GetKey(KeyCode.LeftShift))
    //    { 
    //        anim.SetInteger(ahVelocidad, Mathf.FloorToInt(velocidad) * 2 );
   //         _characterController.Move(move * Time.deltaTime * _speed);
     //   }
      //  else
      //  {
            anim.SetInteger( ahVelocidad, Mathf.FloorToInt(velocidad)); 
            _characterController.Move(move * Time.deltaTime);
      //  }
    }

}
