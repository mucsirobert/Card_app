using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class TableButton : MonoBehaviour {

    public Button Button { get; private set; }

    private Image image;

    private Sprite onSpirte;

    [SerializeField]
    private Sprite offSprite;



    // Use this for initialization
    void Awake () {
        Button = GetComponent<Button>(); 
        image = GetComponent<Image>();
        onSpirte = image.sprite;

        
    }

    public void Toggle(bool b)
    {
        image.sprite = b ? onSpirte : offSprite;
    }
	

}
