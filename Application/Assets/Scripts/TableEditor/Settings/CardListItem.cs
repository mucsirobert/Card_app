using System;
using UnityEngine;
using UnityEngine.UI;

public class CardListItem : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    public NumberInputField inputField;

    public Sprite Sprite
    {
        get
        {
            return image.sprite;
        }
        set
        {
            image.sprite = value;
        }
    }

    public int NumberOfCards
    {
        get
        {
            return inputField.Number;
        }
        set
        {
            inputField.Number = value;
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
