using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioGroup : MonoBehaviour {

	public int GetSelectedRadioButtonIndex()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Toggle>().isOn)
            {
                return i;
            }
        }

        return 0;
    }

    public void SetSelectedRadioButton(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Toggle>().isOn = false;
        }

        transform.GetChild(index).GetComponent<Toggle>().isOn = true;
    }
}
