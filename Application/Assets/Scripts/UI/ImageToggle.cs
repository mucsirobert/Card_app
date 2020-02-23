using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageToggle : MonoBehaviour {

    [SerializeField]
    private bool isOn = true;

    [SerializeField]
    private Sprite onSprite;

    [SerializeField]
    private Sprite offSprite;

    [SerializeField]
    private Image targetImage;

    // Use this for initialization
    void Awake () {
        Toggle(isOn);
    }

    public void Toggle(bool on)
    {
        isOn = on;
        targetImage.sprite = on ? onSprite : offSprite;

    }

}
