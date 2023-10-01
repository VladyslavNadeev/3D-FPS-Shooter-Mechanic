using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageWeapon : Element
{
    [Header("Settings")]
    [SerializeField] private Image _imageWeaponLine;

    private Coroutine _alphaTransitionCoroutine;

    protected override void Tick()
    {
        WeaponTypeState currentWeaponType = equippedWeapon.GetWeaponTypeState();

        if (weaponTypeState == currentWeaponType)
        {
            if (_alphaTransitionCoroutine != null)
            {
                StopCoroutine(_alphaTransitionCoroutine);
            }

            _alphaTransitionCoroutine = StartCoroutine(ChangeAlpha(1f));
        }
        else
        {
            if (_alphaTransitionCoroutine != null)
            {
                StopCoroutine(_alphaTransitionCoroutine);
            }

            _alphaTransitionCoroutine = StartCoroutine(ChangeAlpha(0f));
        }
    }

    private IEnumerator ChangeAlpha(float targetAlpha)
    {
        Color currentColor = _imageWeaponLine.color;
        float startAlpha = currentColor.a;
        float elapsedTime = 0f;
        float duration = 0.1f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            currentColor.a = alpha;
            _imageWeaponLine.color = currentColor;

            yield return null;
        }

        currentColor.a = targetAlpha;
        _imageWeaponLine.color = currentColor;

        _alphaTransitionCoroutine = null;
    }
}