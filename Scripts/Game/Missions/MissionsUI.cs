using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsUI : MonoBehaviour
{
    public static MissionsUI Instance {get; private set;}

    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private List<Transform> _transforms;

    public bool IsOpen;

    private void Awake()
    {
        if(Instance != this && Instance != null)
            Destroy(Instance);
        Instance = this;
        _panel.blocksRaycasts  = false;
    }

    public void OpenMissions()
    {
        Missions.Instance.UpdateMissions();

        _panel.blocksRaycasts = true;
        _panel.alpha = 0f;
        _panel.DOFade(1, 0.3f).SetLink(_panel.gameObject);

        float delay = 0.15f;
        foreach (Transform t in _transforms)
        { 
            Vector2 startScale = t.localScale;
            t.localScale = Vector2.zero;
            t.DOScale(startScale, Random.Range(0.15f, 0.3f)).SetLink(transform.gameObject).SetEase(Ease.OutBack).SetDelay(delay);
            delay += 0.1f;
        }
    }
    public void CloseMissions()
    {
        _panel.blocksRaycasts = false;
        _panel.DOFade(0, 0.3f);
    }
}
