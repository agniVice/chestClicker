using DG.Tweening;
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public static Interface Instance { get; private set; }

    public Action OnAnimOnMid;

    [Header("UI`s")]

    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _menuUI;

    [Space]
    [Header("FadeFX")]
    [SerializeField] private Color32 _fadeFXXColor;
    [SerializeField] private Image _fadeFX;

    [Space]
    [SerializeField] private float _fadeTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        _fadeFXXColor = _fadeFX.color;
        _fadeFX.color = new Color32(255, 255, 255, 255);

        OpenMenu();
    }
    public void OpenMenu()
    {
        _fadeFX.DOFade(1, _fadeTime).SetLink(_fadeFX.gameObject);
        gameObject.transform.DOMove(transform.position, _fadeTime).SetLink(_fadeFX.gameObject).OnKill(() => {
            _menuUI.SetActive(true);
            _gameUI.SetActive(false);
            ClickerInterface.Instance.CloseClicker();
        });
        _fadeFX.DOFade(0, _fadeTime).SetLink(_fadeFX.gameObject).SetDelay(_fadeTime*2);
    }
    public void OpenGame()
    {
        _fadeFX.DOFade(1, _fadeTime).SetLink(_fadeFX.gameObject).SetEase(Ease.Linear);
        gameObject.transform.DOMove(transform.position, _fadeTime).SetLink(_fadeFX.gameObject).SetEase(Ease.Linear).OnKill(() => {
            _menuUI.SetActive(false);
            _gameUI.SetActive(true);

            if (PlayerPrefs.GetInt("SlotOpened", 0) == 0)
                OpenClicker();
            else
                OpenSlot();

        });
        _fadeFX.DOFade(0, _fadeTime).SetLink(_fadeFX.gameObject).SetDelay(_fadeTime*2);
    }
    public void OpenClicker()
    {
        SlotUI.Instance.CloseSlot();
        ClickerInterface.Instance.OpenClicker();
    }
    public void OpenSlot()
    {
        ClickerInterface.Instance.CloseClicker();
        SlotUI.Instance.OpenSlot();
    }
}