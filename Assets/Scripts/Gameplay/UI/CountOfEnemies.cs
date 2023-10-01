using System;
using Infrastructure.Services.PersistenceProgress;
using TMPro;
using UnityEngine;
using Zenject;

public class CountOfEnemies : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    
    private IPersistenceProgressService _progress;

    [Inject]
    public void Construct(IPersistenceProgressService progress)
    {
        _progress = progress;
    }

    public void Init()
    {
        _progress.PlayerData.Progress.OnEnemyCountChanged += UpdateTextValue;
        UpdateTextValue(_progress.PlayerData.Progress.Enemies);
    }

    private void OnDestroy()
    {
        _progress.PlayerData.Progress.OnEnemyCountChanged -= UpdateTextValue;
    }

    private void UpdateTextValue(int enemyCount)
    {
        _text.text = "Enemies Count: " + enemyCount;
    }
}