using UnityEngine;

public class TextureMove : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 0.1f;

    private Renderer _renderer;
    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }
    
    void Update()
    {
        float offset = Time.time * _scrollSpeed;
        _renderer.material.SetTextureOffset("_MainTex", new Vector2(offset,offset));
    }
}
