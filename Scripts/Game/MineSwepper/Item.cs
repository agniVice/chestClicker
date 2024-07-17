using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private bool _isItemOpened;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _winText;
    [SerializeField] private Sprite[] _sprites;

    private void Start()
    {
        _image = GetComponent<Image>();
    }
    public enum TypeItem
    {
        Empty,
        Bomb,
        Flag
    }
    public TypeItem Type;

    public void FinishOpen()
    {
        if (_isItemOpened)
            return;

        _image.rectTransform.DOScale(new Vector3(0, 1, 1), 0.2f).SetEase(Ease.OutCubic).SetLink(_image.gameObject)
                .OnKill(() => {
                    if (Type == TypeItem.Flag)
                        _winText.text = FormatNumHelper.FormatNum(UserBalance.Instance.Balance * ItemManager.Instance.Multiplier);
                    _image.sprite = _sprites[(int)Type];
                    _image.rectTransform.DOScale(new Vector3(1, 1, 1), 0.2f).SetEase(Ease.OutBack).SetLink(_image.gameObject);
                });
        _isItemOpened = true;
    }
    public void Open()
    {
        if (_isItemOpened)
            return;

        _image.rectTransform.DOScale(new Vector3(0, 1, 1), 0.2f).SetEase(Ease.OutCubic).SetLink(_image.gameObject)
                .OnKill(() => {
                    if (Type == TypeItem.Flag)
                        _winText.text = FormatNumHelper.FormatNum(UserBalance.Instance.Balance * ItemManager.Instance.Multiplier);
                    _image.sprite = _sprites[(int)Type];
                    _image.rectTransform.DOScale(new Vector3(1, 1, 1), 0.2f).SetEase(Ease.OutBack).SetLink(_image.gameObject);
                });
        ItemManager.Instance.ItemOpened(this);

        _isItemOpened = true;
    }
}
