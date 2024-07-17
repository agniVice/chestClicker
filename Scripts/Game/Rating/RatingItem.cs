using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RatingItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _positionText;
    [SerializeField] private TextMeshProUGUI _nicknameText;
    [SerializeField] private GameObject _userBg;

    public void Initialize(int position, float score, string nickname, bool isUser = false)
    {
        _userBg.GetComponent<Image>().enabled = isUser;
        _positionText.text = (position+1).ToString();
        _scoreText.text = FormatNumHelper.FormatNum(score);
        _nicknameText.text = nickname;
    }
}
