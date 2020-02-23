using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement), typeof(CanvasGroup))]
public abstract class AutoScrollLayoutItem<T> : MonoBehaviour {

    private CanvasGroup canvasGroup;
    private LayoutElement layoutElementComponent;

    private IEnumerator fadeoutCorutine;
    private Tweener currentFadeAnimation;
    private Tweener currentHeightAnimation;
    private float defaultPreferedHeight;

    private Tweener currentFadeInAnimation;

    public delegate void FadeOutCompletedDelegate(LogPopUpText element);


    public float PreferredHeight
    {
        get
        {
            return layoutElementComponent.preferredHeight;
        }
        set
        {
            layoutElementComponent.preferredHeight = value;
        }
    }


    private float Alpha
    {
        get
        {
            return canvasGroup.alpha;
        }
        set
        {
            canvasGroup.alpha = value;
        }
    }

    // Use this for initialization
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        layoutElementComponent = GetComponent<LayoutElement>();
        defaultPreferedHeight = PreferredHeight;
    }

    public virtual void SetData(T data)
    {
        if (fadeoutCorutine != null)
            StopCoroutine(fadeoutCorutine);

        if (currentFadeAnimation != null)
            currentFadeAnimation.Kill();

        PreferredHeight = defaultPreferedHeight;

        if (currentFadeInAnimation != null)
            currentFadeInAnimation.Kill();

        Alpha = 0;
        currentFadeInAnimation = DOTween.To(x => Alpha = x, 0, 1f, 0.4f).SetEase(Ease.InOutCubic);
    }

    public void SetAlpha(float alpha)
    {
        if (currentFadeAnimation == null || !currentFadeAnimation.IsPlaying())
        {
            Alpha = alpha;
        }
    }

    public void StartShrinking(float waitSecondsBeforeFade)
    {
        if (fadeoutCorutine != null)
            StopCoroutine(fadeoutCorutine);

        if (currentFadeAnimation != null)
            currentFadeAnimation.Kill();

        if (currentHeightAnimation != null)
            currentHeightAnimation.Kill();
        fadeoutCorutine = Shrink(waitSecondsBeforeFade);
        StartCoroutine(fadeoutCorutine);
    }

    public IEnumerator Shrink(float waitSecondsBeforeFade)
    {
        yield return new WaitForSeconds(waitSecondsBeforeFade);

        currentFadeAnimation = DOTween.To(x => layoutElementComponent.preferredHeight = x, layoutElementComponent.preferredHeight, 0f, 1f).SetEase(Ease.InOutCubic);
    }
}
