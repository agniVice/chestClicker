using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingUI : MonoBehaviour
{
    public static RatingUI Instance { get; private set; }

    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private List<Transform> _transforms;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(Instance);
        Instance = this;
        _panel.blocksRaycasts = false;
    }

    public void OpenRating()
    {
        Rating.Instance.InitializeAll();
        _panel.blocksRaycasts = true;
        _panel.alpha = 0f;
        _panel.DOFade(1, 0.3f).SetLink(_panel.gameObject);

        float delay = 0.1f;
        foreach (Transform t in _transforms)
        {
            Vector2 startScale = t.localScale;
            t.localScale = Vector2.zero;
            t.DOScale(startScale, Random.Range(0.15f, 0.3f)).SetLink(transform.gameObject).SetEase(Ease.OutBack).SetDelay(delay);
            delay += 0.03f;
        }
    }
    public void CloseRating()
    {
        _panel.blocksRaycasts = false;
        _panel.DOFade(0, 0.3f);
    }
}
