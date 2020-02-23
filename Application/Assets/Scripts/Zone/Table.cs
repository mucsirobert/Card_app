using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class Table : NetworkBehaviour {

    public void OnDrop(Droppable droppable)
    {
        CardView card = droppable.GetComponent<CardView>();

        //Return it back
        card.ReturnToLastZone();
    }

    // Use this for initialization
    void Start () {
        BoxCollider2D collider= GetComponent<BoxCollider2D>();

        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);

        collider.size = cameraSize;
    }
	
}
