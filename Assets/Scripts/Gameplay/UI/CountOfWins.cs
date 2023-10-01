using Infrastructure.Services.PersistenceProgress;
using TMPro;
using UnityEngine;
using Zenject;

public class CountOfWins : MonoBehaviour
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
        _progress.PlayerData.Progress.OnWinCountChanged += UpdateTextValue;
        UpdateTextValue(_progress.PlayerData.Progress.WinCount);
    }

    private void OnDestroy()
    {
        _progress.PlayerData.Progress.OnWinCountChanged -= UpdateTextValue;
    }

    private void UpdateTextValue(int winCount)
    {
        _text.text = $"Wins: {winCount}";
    }
}