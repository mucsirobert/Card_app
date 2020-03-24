using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberInputField : MonoBehaviour {

    [SerializeField]
    private InputField inputField;

    public int maxNumber = 1000;

    public int minNumber = 0;

    public int steps = 1;

    private int number;

    public int Number {
        get
        {
            return number;
        }
        set
        {
            number = value;
            inputField.text = number.ToString();
        }
    }

    public void Update()
    {
        if(inputField.text != "")
            Number = Int32.Parse(inputField.text);
    }

    public void Start()
    {
        inputField.text = Number.ToString();
    }

    public void OnPlusButtonPressed()
    {
        Number += steps;

        if (Number > maxNumber) Number = maxNumber;
        inputField.text = Number.ToString();
    }

    public void OnMinusButtonPressed()
    {
        Number-= steps;

        if (Number < minNumber) Number = minNumber;
        inputField.text = Number.ToString();
    }

}
