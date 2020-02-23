using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectTransformFillScreen : MonoBehaviour {

    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        float cameraHeight = Camera.main.orthographicSize * 2;

        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);

    }
}
