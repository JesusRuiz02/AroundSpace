using UnityEngine;
using UnityEngine.UI;

public class ButtonTransparency : MonoBehaviour
{
    private Image _image;
    void Start()
    {
      _image = GetComponent<Image>();
    }

    public void Transparentbutton(byte transparentValue)
    {
        _image.color = new Color32(255, 255, 255, transparentValue);
    }

}
