using Infrastructure.Services.PersistenceProgress;
using TMPro;
using UnityEngine;
using Zenject;

public class CountOfLoses : MonoBehaviour
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
        _progress.PlayerData.Progress.OnLoseCountChanged += UpdateTextValue;
        UpdateTextValue(_progress.PlayerData.Progress.LoseCount);
    }

    private void OnDestroy()
    {
        _progress.PlayerData.Progress.OnLoseCountChanged -= UpdateTextValue;
    }

    private void UpdateTextValue(int loseCount)
    {
        _text.text = $"Loses: {loseCount}";
    }
}