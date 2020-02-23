using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillScreen : MonoBehaviour {

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);

        spriteRenderer.size = cameraSize;

    }
}
