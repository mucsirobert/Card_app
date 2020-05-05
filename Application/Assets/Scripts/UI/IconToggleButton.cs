using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconToggleButton : MonoBehaviour {

    [SerializeField]
    private Transform onIcon;

    [SerializeField]
    private Transform offIcon;

    [SerializeField]
    private bool isOn;

    // Use this for initialization
    void Awake () {
        offIcon.gameObject.SetActive(!isOn);
        onIcon.gameObject.SetActive(isOn);
    }

    public void Toggle()
    {
        isOn = !isOn;

        offIcon.gameObject.SetActive(!isOn);
        onIcon.gameObject.SetActive(isOn);

       
    }

}
