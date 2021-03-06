﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SidewayScrollable : MonoBehaviour {

    public GameObject leftButton;
    public GameObject rightButton;

    public Transform objectToMove;
    public Transform rightPoint;

    [SerializeField]
    private BoxCollider2D holderCollider;

    public float horizontalMoveAmount = 2.0f;



    // Use this for initialization
    void Awake () {
        InitButtons();
	}

    public void InitButtons()
    {
        leftButton.SetActive(objectToMove.localPosition.x < 0);
        rightButton.SetActive(objectToMove.childCount > 0 && objectToMove.GetChild(objectToMove.childCount - 1).position.x >= rightPoint.position.x);
    }

    public void OnChildAdded(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        if (objectToMove.childCount > 0 && objectToMove.GetChild(objectToMove.childCount - 1).position.x >= rightPoint.position.x)
        {
            rightButton.SetActive(true);
        }
        UpdateBoxColliders();
    }

    public void OnChildRemoved(SpriteRenderer spriteRenderer)
    {
        BoxCollider2D collider = spriteRenderer.GetComponent<BoxCollider2D>();
        collider.enabled = true;
        Vector2 originalSize = spriteRenderer.sprite.bounds.size;
        collider.size = originalSize;
        collider.offset = Vector2.zero;

        spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        if (objectToMove.childCount > 0 && objectToMove.GetChild(objectToMove.childCount - 1).position.x < rightPoint.position.x)
        {
            rightButton.SetActive(false);
        }
        UpdateBoxColliders();
    }

    public void UpdateBoxColliders()
    {
        for (int i = 0; i < objectToMove.childCount; i++)
        {
            BoxCollider2D collider = objectToMove.GetChild(i).GetComponent<BoxCollider2D>();
            Vector2 originalSize = collider.GetComponent<SpriteRenderer>().sprite.bounds.size;
            collider.size = originalSize;
            collider.offset = Vector2.zero;
            collider.enabled = true;

            if (holderCollider.bounds.Intersects(collider.bounds))
            {
                Vector3 colliderMin = collider.bounds.min;
                //Debug.Log(colliderMin.x + " " + holderCollider.bounds.max.x);
                Vector3 colliderMax = collider.bounds.max;
                if (colliderMin.x < holderCollider.bounds.min.x && colliderMax.x > holderCollider.bounds.min.x)
                {
                    //this is the left most visible item
                    Bounds newBounds = new Bounds();
                    colliderMin.x = holderCollider.bounds.min.x;
                    newBounds.SetMinMax(colliderMin, colliderMax);

                    collider.offset = collider.transform.InverseTransformPoint(newBounds.center);
                    collider.size = newBounds.size;
                } else if (colliderMin.x < holderCollider.bounds.max.x && colliderMax.x > holderCollider.bounds.max.x)
                {
                    //this is the right most visible item
                    Bounds newBounds = new Bounds();
                    colliderMax.x = holderCollider.bounds.max.x;
                    newBounds.SetMinMax(colliderMin, colliderMax);

                    collider.offset = collider.transform.InverseTransformPoint(newBounds.center);
                    collider.size = newBounds.size;

                } 
            }
            else
            {
                collider.enabled = false;
            }
        }
        
    }


    public void OnRightButtonClicked()
    {

        objectToMove.DOKill();
        if (objectToMove.childCount > 0 && objectToMove.GetChild(objectToMove.childCount - 1).position.x >= rightPoint.position.x)
        {
            objectToMove.DOLocalMoveX(objectToMove.localPosition.x - horizontalMoveAmount, 0.2f).OnComplete(() => {
                UpdateBoxColliders();
            });
            leftButton.SetActive(true);

            if (objectToMove.GetChild(objectToMove.childCount - 1).position.x - horizontalMoveAmount < rightPoint.position.x)
            {
                rightButton.SetActive(false);
            }
        }

    }

    public void OnLeftButtonClicked()
    {
        objectToMove.DOKill();
        if (objectToMove.localPosition.x + horizontalMoveAmount >= Vector3.zero.x)
        {
            objectToMove.DOLocalMoveX(0, 0.2f).OnComplete(() => {
                UpdateBoxColliders();
            });
            leftButton.SetActive(false);
        }
        else
        {
            objectToMove.DOLocalMoveX(objectToMove.localPosition.x + horizontalMoveAmount, 0.2f).OnComplete(() => {
                UpdateBoxColliders();
            });
            rightButton.SetActive(true);
        }


    }
}
