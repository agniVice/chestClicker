using System;
using UnityEngine;

public class Machine : MonoBehaviour
{
    public static Machine Instance { get; private set; }

    public float FinalBet { get; private set; }
    public float PlayBet { get; private set; }
    public bool AutoSpinnig { get; private set; }

    public float LastBet {  get; private set; }

    [SerializeField] private int _wildChance;
    [SerializeField] private int _defaultWinChance;
    [SerializeField] private int _minBalanceForWin;

    [SerializeField] private int[] _bets;
    [SerializeField] private int _defaultWin;
    [SerializeField] private int _wildWin;

    [SerializeField] private Drum[] _drums;
    [SerializeField] private int _lines = 3;

    private int _currentBet;

    private int _winChance;

    private bool _isSpinning;
    private bool[] _canSpin = { true, true, true };

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        FinalBet = _bets[_currentBet];

        LastBet = PlayerPrefs.GetFloat("LastBet", 0);
    }
    private void FixedUpdate()
    {
        CheckForSpin();
    }
    private void CheckForSpin()
    {
        if (AutoSpinnig)
        {
            if (!_isSpinning)
            {
                if (_canSpin[0] && _canSpin[1] && _canSpin[2])
                {
                    if (UserBalance.Instance.Balance >= _bets[_currentBet])
                        Spin();
                    else
                        ToggleAutoSpin();
                }
            }
        }
        if (_isSpinning)
        {
            if (_canSpin[0] && _canSpin[1] && _canSpin[2])
                OnEndSpin();
        }
    }
    public void Spin()
    {
        if (_isSpinning)
            return;
        if (UserBalance.Instance.Balance < FinalBet)
        {
            if (AutoSpinnig)
                ToggleAutoSpin();
            return;
        }
        if (_canSpin[0] && _canSpin[1] && _canSpin[2] && SlotUI.Instance.CanSpin)
        {
            Audio.Instance.PlaySound(Audio.Instance.SpinSound, 1f);
            PlayBet = FinalBet;

            UpdateWinChance();

            for (int i = 0; i < _canSpin.Length; i++)
                _canSpin[i] = false;

            int winGame = WinGame();

            if (winGame != -1)
            {
                ItemType[] types = new ItemType[3];
                for (int i = 0; i < types.Length; i++)
                {
                    types[i] = (ItemType)UnityEngine.Random.Range(0, 10);
                }
                float spinDelay = 0f;
                for (int i = 0; i < _drums.Length; i++)
                {
                    _drums[i].OnDrumStartWinSpin(spinDelay, types, winGame);
                    spinDelay += 0.15f;
                }
            }
            else
            {
                float spinDelay = 0f;
                for (int i = 0; i < _drums.Length; i++)
                {
                    _drums[i].OnDrumStartSpin(spinDelay);
                    spinDelay += 0.15f;
                }
            }

            UserBalance.Instance.Change(-FinalBet);
            SlotUI.Instance.UpdateBalance();

            _isSpinning = true;

            SlotUI.Instance.OnStartSpin();
        }
    }
    private void CheckForWin()
    {
        float reward = 0;
        int wildCount = 0;
        int[] win = { 0, 0, 0 };
        for (int j = 0; j < _lines; j++)
        {
            ItemType type = _drums[0].GetLineElement(j + 1);
            bool elementsSame = true;
            for (int i = 0; i < _drums.Length; i++)
            {
                if (_drums[i].GetLineElement(j + 1) != type)
                {
                    elementsSame = false;
                }
            }
            if (elementsSame)
            {
                if (type == ItemType.Wild)
                {
                    wildCount++;
                    reward += PlayBet * _wildWin;
                }
                else
                    reward += PlayBet * _defaultWin;

                foreach (var drum in _drums)
                    drum.PlayWinFX(j + 1);

                win[j]++;
                LastBet = reward;
                UserBalance.Instance.Change(reward);
            }
            else
                LastBet = -PlayBet;
        }
        PlayerPrefs.SetFloat("LastBet", LastBet);
        SlotUI.Instance.UpdateHistory();
        SlotUI.Instance.OnPlayerWin(reward, win, wildCount);
    }
    private void UpdateWinChance()
    {
        _winChance = _defaultWinChance + (PlayerPrefs.GetInt("Upgrade3", 0) * 2);
    }
    public void SetAllIn()
    {
        FinalBet = UserBalance.Instance.Balance;
    }
    private void OnEndSpin()
    {
        CheckForWin();

        _isSpinning = false;
        SlotUI.Instance.OnEndSpin();
    }
    public void IncreaseBet()
    {
        if (_currentBet != _bets.Length - 1)
            _currentBet++;
        FinalBet = _bets[_currentBet];
    }
    public void DecreaseBet()
    {
        if (_currentBet != 0)
            _currentBet--;
        FinalBet = _bets[_currentBet];
    }
    public void ToggleAutoSpin()
    {
        AutoSpinnig = !AutoSpinnig;
    }
    public void DisableAutoSpin()
    {
        if (AutoSpinnig)
            ToggleAutoSpin();
    }
    private int WinGame()
    {
        float isWin = UnityEngine.Random.Range(0, 100f);
        if (isWin <= _winChance || UserBalance.Instance.Balance <= _minBalanceForWin)
        {
            float randomNumber = UnityEngine.Random.Range(0, 110f);
            if (randomNumber < 33)
                return 0;
            else if (randomNumber > 33 && randomNumber < 66)
                return 1;
            else if (randomNumber > 66 && randomNumber < 99)
                return 2;
            else
                return 3;
        }
        else
            return -1;
    }
    
    public bool IsCurrentBetMax() => _currentBet == (_bets.Length - 1);
    public bool IsCurrentBetMin() => _currentBet == 0;
    public float GetWildChance() => _wildChance;
    public void ReadyForSpin(int id) => _canSpin[id] = true;
    public bool CanOpenMenu() => (_canSpin[0] && _canSpin[1] && _canSpin[2]);
    public int GetDrumIndex(Drum drum) => Array.IndexOf(_drums, drum);
}
