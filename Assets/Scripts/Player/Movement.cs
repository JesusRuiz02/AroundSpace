using UnityEngine;


public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private GameObject _turbo1 = default;
    [SerializeField] private GameObject _turbo2 = default;
    [SerializeField] private bool isturboOn = false;
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !isturboOn )
        {
            transform.Translate(Vector3.forward *Time.deltaTime* speed);
        }
        if (Input.GetKey(KeyCode.S) && !isturboOn)
        {
            transform.Translate(Vector3.back *Time.deltaTime*speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0,0.1f,0 *Time.deltaTime * 0.1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0,-0.1f,0 *Time.deltaTime * 0.1f);  
        }

       /* if (Input.GetKey(KeyCode.LeftShift))
        {
            isturboOn = true;
            transform.Translate(Vector3.forward *Time.deltaTime* speed * 2);
            _turbo1.SetActive(true);
            _turbo2.SetActive(true);
        }
        else
        {
            isturboOn = false;
            _turbo1.SetActive(false);
            _turbo2.SetActive(false);
        }*/
    }
}
