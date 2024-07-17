using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mission : MonoBehaviour
{
    public float[] TargetCounts;
    public float CurrentCount;

    public float[] Rewards;

    [SerializeField] private TextMeshProUGUI[] _rewardTexts;
    [SerializeField] private TextMeshProUGUI _infoText;
    [SerializeField] private Image _progressBar;

    [SerializeField] private GameObject _completedWindow;

    [SerializeField] private string _firstInfo;
    [SerializeField] private string _secondInfo;

    [SerializeField] private bool _reversedBar = false;

    private bool[] _missionsCompleted;
    private bool[] _rewardsTaked;

    private int _currentMission;
    //private bool _isCompleted;
    private bool _readyTakeReward;
    private int _missionForReward;

    public void Initialize(int id, float defaultCount)
    {
        _missionsCompleted = new bool[TargetCounts.Length];
        _rewardsTaked = new bool[TargetCounts.Length];

        for (int i = 0; i < TargetCounts.Length; i++)
        {
            _missionsCompleted[i] = Convert.ToBoolean(PlayerPrefs.GetInt((id + "MissionCompleted" + i), 0));
            _rewardsTaked[i] = Convert.ToBoolean(PlayerPrefs.GetInt((id + "RewardTaked" + i), 0));
        }
        //_currentMission = PlayerPrefs.GetInt(id + "CurrentMission");
        CurrentCount = PlayerPrefs.GetFloat(id + "CurrentCount", defaultCount);
    }
    private void CheckComplete()
    {
        for (int i = 0; i < TargetCounts.Length; i++)
        {
            if (!_reversedBar)
            {
                if (CurrentCount >= TargetCounts[i])
                    _missionsCompleted[i] = true;
            }
            else
            {
                if (CurrentCount <= TargetCounts[i])
                    _missionsCompleted[i] = true;
            }
        }
        for (int i = 0; i < _missionsCompleted.Length; i++)
        {
            if (_missionsCompleted[i])
                _currentMission = (i+1);
        }
    }
    private void ShowCompleted(int mission)
    {
        _missionForReward = mission;
        _rewardTexts[1].text = FormatNumHelper.FormatNum(Rewards[mission]);
        _completedWindow.SetActive(true);
    }
    private void HideCompleted() => _completedWindow.SetActive(false);
    public void ChangeCount(float count)
    {
        //if (_isCompleted)
           // return;

        //if (CurrentCount + count < TargetCounts[_currentMission])
            CurrentCount = count;

        CheckComplete();
        if (MissionsUI.Instance.IsOpen)
            UpdateMission();
    }
    public void TakeReward()
    {
        if (_missionsCompleted[_missionForReward])
        {
            UserBalance.Instance.Change(Rewards[_missionForReward]);
            _rewardsTaked[_missionForReward] = true;
            //_missionForReward++;
            Missions.Instance.UpdateAll();
            HideCompleted();
            UpdateMission();
            Missions.Instance.Save();
            ClickerInterface.Instance.UpdateBalance();
            Audio.Instance.PlaySound(Audio.Instance.Bonus, 1f);
        }
    }
    public void UpdateMission()
    {
        CheckComplete();
        if (_currentMission >= TargetCounts.Length)
        {
            _progressBar.fillAmount = 1;
            _rewardTexts[0].text = "MAX";
            _infoText.text = _firstInfo + FormatNumHelper.FormatNum(TargetCounts[_currentMission-1]) + _secondInfo;
        }
        else
        { 
            for (int i = 0; i < _rewardTexts.Length; i++)
                _rewardTexts[i].text = "+" + FormatNumHelper.FormatNum(Rewards[_currentMission]);
            _infoText.text = _firstInfo + FormatNumHelper.FormatNum(TargetCounts[_currentMission]) + _secondInfo;

            if (!_reversedBar)
                _progressBar.fillAmount = CurrentCount / TargetCounts[_currentMission];
            else
                _progressBar.fillAmount = TargetCounts[_currentMission] / CurrentCount;

            HideCompleted();

            for (int i = 0; i < _missionsCompleted.Length; i++)
            {
                if (_missionsCompleted[i] && !_rewardsTaked[i])
                {
                    ShowCompleted(i);
                    return;
                }
            }
            /*if (_isCompleted)
                ShowCompleted();
            else
                HideCompleted();*/
        }
    }
    public void Save(int id)
    {
        PlayerPrefs.SetFloat(id + "CurrentCount", CurrentCount);
        //PlayerPrefs.SetInt(id + "CurrentMission", _currentMission);

        for (int i = 0; i < TargetCounts.Length; i++)
        {
            PlayerPrefs.SetInt(id + "MissionCompleted" + i, Convert.ToInt32(_missionsCompleted[i]));
            PlayerPrefs.SetInt(id + "RewardTaked" + i, Convert.ToInt32(_rewardsTaked[i]));
        }
    }
}
