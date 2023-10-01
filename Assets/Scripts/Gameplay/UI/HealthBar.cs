using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthProgressBar;
    [SerializeField] private Slider _healthEffectProgressBar;
    [SerializeField] private float _goingDownTime = 2f;
    [SerializeField] private float _effectSpeed = 1f;
    
    private PlayerVitality _playerVitality;

    public void Init(PlayerVitality playerVitality)
    {
        _playerVitality = playerVitality;
        _playerVitality.OnTakeDamage += UpdateHealth;
    }

    private void OnDestroy()
    {
        _playerVitality.OnTakeDamage -= UpdateHealth;
    }

    private void UpdateHealth(int value)
    {
        _healthProgressBar.value -= value;
        StartCoroutine(HealthDecreaseEffect(value));
    }
    
    private IEnumerator HealthDecreaseEffect(float value)
    {
        float goingDownTimer = _goingDownTime;
        
        do
        {
            yield return new WaitForFixedUpdate();
            goingDownTimer -= Time.deltaTime;
            
            float smoothedValue = Mathf.Lerp(_healthEffectProgressBar.value, value, Time.fixedDeltaTime * _effectSpeed);
            _healthEffectProgressBar.value = smoothedValue;
            
        } while (goingDownTimer > 0);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SetActive(bool activated)
    {
        _healthProgressBar.gameObject.SetActive(activated);
    }
}