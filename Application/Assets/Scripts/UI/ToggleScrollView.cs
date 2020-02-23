using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScrollView : MonoBehaviour {

    [SerializeField]
    private Toggle togglePrefab;

    [SerializeField]
    private Transform contentTransform;

	// Use this for initialization
	void Start () {
		
	}

    public void Init(string[] labels, bool defaultIsOn = false)
    {
        bool[] checkArray = new bool[labels.Length];
        for (int i = 0; i < checkArray.Length; i++)
        {
            checkArray[i] = defaultIsOn;
        }

        Init(labels, checkArray);
    }


    public void Init(string[] labels, bool[] checksArray)
    {
        for (int i = 0; i < labels.Length; i++) {
            var toggle = Instantiate(togglePrefab, contentTransform);

            toggle.isOn = checksArray[i];
            toggle.GetComponentInChildren<Text>().text = labels[i];
        }
    }

    public bool[] GetCheckData()
    {
        bool[] checkArray = new bool[contentTransform.childCount];

        for (int i = 0; i < contentTransform.childCount; i++)
        {
            checkArray[i] = contentTransform.GetChild(i).GetComponent<Toggle>().isOn;
        }

        return checkArray;
    }

}
