using System.Collections.Generic;
using UnityEngine;

public class Missions : MonoBehaviour
{
    public static Missions Instance {get; private set;}

    [SerializeField] private Mission[] _missions;

    private void Awake()
    {
         if(Instance != null && Instance != this)
            Destroy(this);
         Instance = this;

        InitializeAll();
    }
    private void InitializeAll()
    {
        _missions[0].Initialize(0, Chest.Instance.ScorePerClick);
        _missions[1].Initialize(1, Chest.Instance.AutoScore);
        _missions[2].Initialize(2, Rating.Instance.UserIndex);
    }
    public void UpdateMissions()
    {
        for (int i = 0; i < _missions.Length; i++)
            _missions[i].UpdateMission();
    } 
    public void TakeInfo(string infoType)
    {
        switch (infoType)
        {
            case "Click":
                _missions[0].ChangeCount(Chest.Instance.ScorePerClick);
                break;
            case "Second":
                _missions[1].ChangeCount(Chest.Instance.AutoScore);
                break;
            case "Rating":
                _missions[2].ChangeCount(Rating.Instance.UserIndex);
                break;
        }
        Save();
    }
    public void UpdateAll()
    {
        _missions[0].ChangeCount(Chest.Instance.ScorePerClick);
        _missions[1].ChangeCount(Chest.Instance.AutoScore);
        _missions[2].ChangeCount(Rating.Instance.UserIndex);
    }
    public void Save()
    {
        for (int i = 0; i < _missions.Length; i++)
            _missions[i].Save(i);
    }
}
