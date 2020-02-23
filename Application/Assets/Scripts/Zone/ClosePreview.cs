using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClosePreview : MonoBehaviour, IPointerClickHandler {
    public PreviewZone previewZone;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PointerEventData.InputButton.Left == eventData.button)
        {
            previewZone.Close();
        }
    }

}
