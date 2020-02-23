using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AutoScrollLayout<T> : MonoBehaviour {

    private AutoScrollLayoutItem<T>[] items;

    [SerializeField]
    private LayoutElement dummyField;
    [SerializeField]
    private float waitSecondsBeforeDisapperar = 4f;
    [SerializeField]
    private int cacheSize = 3;
    private int numberOfVisibleItems;


    private int currentTextFieldIndex;

    private Tweener currentDummyAnimation;

    private float maxDummyHeight;

    // Use this for initialization
    protected virtual void Start()
    {
        dummyField.preferredHeight = 0;

        if (items.Length < cacheSize)
        {
            Debug.LogError("Number of textfields is not enough.");
            return;
        }
        maxDummyHeight = 2 * items[0].PreferredHeight;
        for (int i = 0; i < cacheSize; i++)
        {
            items[i].PreferredHeight = 0;
        }

        numberOfVisibleItems = items.Length - cacheSize;
    }

    protected void InitItems(AutoScrollLayoutItem<T>[] items)
    {
        this.items = items;
    }

    public void AddNewItem(T data)
    {
        if ((currentDummyAnimation == null || !currentDummyAnimation.IsActive() || !currentDummyAnimation.IsPlaying()) || dummyField.preferredHeight <= maxDummyHeight)
        {
            if (currentDummyAnimation != null)
                currentDummyAnimation.Kill();

            currentDummyAnimation = DOTween.To(x => dummyField.preferredHeight = x, dummyField.preferredHeight + items[currentTextFieldIndex].PreferredHeight, 0f, 0.7f);//.SetEase(Ease.InOutCubic);
            dummyField.preferredHeight = dummyField.preferredHeight + items[currentTextFieldIndex].PreferredHeight;
        }

        items[currentTextFieldIndex].transform.SetAsLastSibling();

        //Start shrinking one of the upper items
        items[(currentTextFieldIndex + items.Length - cacheSize) % items.Length].StartShrinking(waitSecondsBeforeDisapperar);
        items[currentTextFieldIndex].SetData(data);
        //SetTextAlphas();

        //Set the index to the top item
        currentTextFieldIndex = (currentTextFieldIndex + 1) % items.Length;
    }


    private void SetTextAlphas()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetAlpha(1f / items.Length * (items[i].transform.GetSiblingIndex() + 1));
        }
    }
}
