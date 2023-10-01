using UnityEngine;

public class Magazine : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private int _ammunitionTotal = 10;
	[SerializeField] private WeaponTypeState _weaponTypeState;

	[Header("Interface")]
	[SerializeField] private Sprite _sprite;
	
	public int GetAmmunitionTotal(WeaponTypeState weaponTypeState)
	{
		if (weaponTypeState == WeaponTypeState.M4A1)
		{
			_ammunitionTotal = 30;
		}
		else if(weaponTypeState == WeaponTypeState.Pistol)
		{
			_ammunitionTotal = 10;
		}

		return _ammunitionTotal;
	}

	public Sprite GetSprite() => _sprite;
}