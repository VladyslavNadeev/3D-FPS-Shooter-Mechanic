using UnityEngine;

public class Crosshair : Element
{
    [Header("Settings")]
    [SerializeField] private float _smoothing = 8.0f;
    [SerializeField] private float _minimumScale = 0.15f;
    [SerializeField] private RectTransform _rectTransform;

    private float _current = 1.0f;
    private float _target = 1.0f;
        
    protected override void Tick()
    {
        bool visible = playerCharacter.IsCrosshairVisible();
        _target = visible ? 1.0f : 0.0f;

        _current = Mathf.Lerp(_current, _target, Time.deltaTime * _smoothing);
        _rectTransform.localScale = Vector3.one * _current;
            
        for (var i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(_current > _minimumScale);
    }
}