using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muzzle : MuzzleBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform _socket;
    [SerializeField] private Sprite _sprite;

    [Header("Particles")]
    [SerializeField] private GameObject _prefabFlashParticles;
    [SerializeField] private int _flashParticlesCount = 5;

    [Header("Flash Light")]
    [SerializeField] private GameObject _prefabFlashLight;
    [SerializeField] private float _flashLightDuration;
    [SerializeField] private Vector3 _flashLightOffset;
    
    private ParticleSystem _particles;
    private Light _flashLight;

    public override Transform GetSocket() => _socket;
    public override Sprite GetSprite() => _sprite;
    public override ParticleSystem GetParticlesFire() => _particles;
    public override int GetParticlesFireCount() => _flashParticlesCount;
    public override Light GetFlashLight() => _flashLight;
    public override float GetFlashLightDuration() => _flashLightDuration;

    public void Init()
    {
        if(_prefabFlashParticles != null)
        {
            GameObject spawnedParticlesPrefab = Instantiate(_prefabFlashParticles, _socket);
            spawnedParticlesPrefab.transform.localPosition = default;
            spawnedParticlesPrefab.transform.localEulerAngles = default;
            
            _particles = spawnedParticlesPrefab.GetComponent<ParticleSystem>();
        }

        if (_prefabFlashLight)
        {
            GameObject spawnedFlashLightPrefab = Instantiate(_prefabFlashLight, _socket);
            spawnedFlashLightPrefab.transform.localPosition = _flashLightOffset;
            spawnedFlashLightPrefab.transform.localEulerAngles = default;
                
            _flashLight = spawnedFlashLightPrefab.GetComponent<Light>();
            _flashLight.enabled = false;
        }
    }


    public override void Effect()
    {
        if(_particles != null)
            _particles.Emit(_flashParticlesCount);

        if (_flashLight != null)
        {
            _flashLight.enabled = true;
            StartCoroutine(nameof(DisableLight));
        }
    }

    private IEnumerator DisableLight()
    {
        yield return new WaitForSeconds(_flashLightDuration);
        _flashLight.enabled = false;
    }
}