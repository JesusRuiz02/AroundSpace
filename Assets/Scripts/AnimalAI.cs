using System.Collections;
using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    [SerializeField] private float movSpeed;
    [SerializeField] private float rotSpeed = 100f;
    private Animator _animator;

    private bool _isWandering = false;
    private bool _isRotL = false;
    private bool _isRotR = false;
    private bool _isWalking = false;

   private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!_isWandering)
        {
            StartCoroutine(Wander());
        }
        if (_isRotR)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }
        if (_isRotL)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }
        _animator.SetBool("IsRunning", _isWalking);
        if (_isWalking)
        {
            rb.transform.position += transform.forward * movSpeed;
        }
      
    }
    IEnumerator Wander()
    {
        int rotTime = Random.Range(1, 3);
        int rotWait = Random.Range(1, 3);
        int rotatelorR = Random.Range(1, 5);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 5);
        _isWandering = true;
        yield return new WaitForSeconds(walkWait);
        _isWalking = true;
        yield return new WaitForSeconds(walkTime);
        _isWalking = false;
        yield return new WaitForSeconds(rotWait);
        switch (rotatelorR)
        {
            case 1:
                _isRotR = true;
                yield return new WaitForSeconds(rotTime);
                _isRotR = false;
                break;
            case 2:
                _isRotL = true;
                yield return new WaitForSeconds(rotTime);
                _isRotL = false;
                break;
            case 3 :
                _isRotR = true;
                yield return new WaitForSeconds(rotTime);
                _isRotR = false;
                break;
            case 4:
                _isRotL = true;
                yield return new WaitForSeconds(rotTime);
                _isRotL = false;
                break;
        }
        _isWandering = false;
    }
}
