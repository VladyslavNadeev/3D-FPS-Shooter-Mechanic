using System.Globalization;
using UnityEngine;

public class TextAmmunitionCurrent : ElementText
{
    [Header("Colors")]
    [SerializeField] private bool _updateColor = true;
    [SerializeField] private float _emptySpeed = 1.5f;
    [SerializeField] private Color _emptyColor = Color.red;

    private float _current;
    private float _total;

    protected override void Tick()
    {
        if (allWeapons[(int)weaponTypeState].gameObject.activeSelf)
        {
            _current = allWeapons[(int)weaponTypeState].GetAmmunitionCurrent();
            _total = allWeapons[(int)weaponTypeState].GetAmmunitionTotal();
        }

        _textMesh.text = _current.ToString(CultureInfo.InvariantCulture);

        if (_updateColor)
        {
            float colorAlpha = (_current / _total) * _emptySpeed;
            _textMesh.color = Color.Lerp(_emptyColor, Color.white, colorAlpha);   
        }
    }
}








