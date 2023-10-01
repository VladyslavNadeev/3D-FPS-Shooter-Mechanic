using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HitScreen : MonoBehaviour
{
    [SerializeField] private Image _image;
    private PlayerVitality _playerVitality;

    public void Init(PlayerVitality playerVitality)
    {
        _playerVitality = playerVitality;
        _playerVitality.OnTakeDamage += ShowHitScreen;
    }

    private void OnDestroy()
    {
        _playerVitality.OnTakeDamage -= ShowHitScreen;
    }

    private void ShowHitScreen(int damage)
    {
        StartCoroutine(ShowHitScreenRoutine());
    }

    private IEnumerator ShowHitScreenRoutine()
    {
        _image.DOColor(new Color(1f, 0f, 0f, 1f), 0.01f).OnComplete(() =>
            _image.DOColor(new Color(1f,0f,0f, 0f), 0.75f));
        yield return new WaitForEndOfFrame();
    }
}