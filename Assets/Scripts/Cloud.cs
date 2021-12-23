using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class Cloud : MonoBehaviour
{
    [Header("Action visualization")]
    [SerializeField] private TextMeshProUGUI textElement;
    [SerializeField] private Transform scalarTransform;
    [SerializeField] private float cloudTime;
    [SerializeField] private bool tweenActive;
    private float currentCloudTime = 0;

    private void Update()
    {
        if (tweenActive)
        {
            if (currentCloudTime > 0)
            {
                currentCloudTime -= Time.deltaTime;
            }
            else
            {
                tweenActive = false;
                scalarTransform.DOScale(0f, .5f);
            }
        }
    }

    public void SetText(string text)
    {
        textElement.text = text;
        ActivateCloud();
    }

    private void ActivateCloud()
    {
        if (!tweenActive)
        {
            tweenActive = true;
            scalarTransform.DOScale(1f, 1f).SetEase(Ease.OutBounce);
            currentCloudTime += (cloudTime + 1f);
        }
        else
        {
            currentCloudTime = cloudTime;
        }
    }
}
