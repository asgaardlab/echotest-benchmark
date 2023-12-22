using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Subtitle : MonoBehaviour
{
    [SerializeField] public GameObject subtitleObject = default;
    private TextMeshProUGUI subtitle;
    public static Subtitle Instance;

    private void Awake()
    {
        Instance = this;
        subtitle = subtitleObject.GetComponent<TextMeshProUGUI>();
        if (!subtitle)
        {
            subtitle = subtitleObject.GetComponentInChildren<TextMeshProUGUI>();
        }
        ClearSubtitle();
    }

    public void SetSubtitle(string text, float delay)
    {
        subtitle.text = text;
        StartCoroutine(ClearAfterSeconds(delay));
    }

    public void ClearSubtitle()
    {
        subtitle.text = "";
    }

    private IEnumerator ClearAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearSubtitle();
    }
}
