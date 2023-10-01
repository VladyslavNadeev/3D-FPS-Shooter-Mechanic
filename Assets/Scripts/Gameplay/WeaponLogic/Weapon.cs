using UnityEngine;

public class Weapon : MonoBehaviour
{
	[Header("Firing")]
	[SerializeField] private bool _automatic;
	[SerializeField] private float _projectileImpulse = 400.0f;
	[SerializeField] private int _roundsPerMinutes = 200;
	[SerializeField] private LayerMask _mask;
	[SerializeField] private float _maximumDistance = 500.0f;

	[Header("Animation")]
	[SerializeField] private Transform _socketEjection;
	[SerializeField] private Animator _animator;

	[Header("Resources")]
	[SerializeField] private GameObject _prefabCasing;
	[SerializeField] private WeaponAttachmentManager _attachmentManager;
	[SerializeField] private GameObject _prefabProjectile;
	[SerializeField] public RuntimeAnimatorController _controller;
	[SerializeField] private Sprite _spriteBody;
	[SerializeField] private WeaponTypeState _weaponTeamState;


	[Header("Configs")] 
	[SerializeField] private DamageConfigScriptableObject _damageConfig;

	private int _ammunitionCurrent;
	private Magazine _magazineBehaviour;
	private MuzzleBehaviour _muzzleBehaviour;
	private Transform _playerCamera;
	private PlayerBehaviour _characterBehaviour;

	public WeaponAttachmentManager WeaponAttachmentManager => _attachmentManager;

	public Sprite GetSpriteBody() => _spriteBody;

	public WeaponTypeState GetWeaponTypeState() => _weaponTeamState;

	public int GetAmmunitionCurrent() => _ammunitionCurrent;

	public int GetAmmunitionTotal() => _magazineBehaviour.GetAmmunitionTotal(_weaponTeamState);

	public bool IsAutomatic() => _automatic;
	public float GetRateOfFire() => _roundsPerMinutes;
        
	public bool IsFull() => _ammunitionCurrent == _magazineBehaviour.GetAmmunitionTotal(_weaponTeamState);
	public bool HasAmmunition() => _ammunitionCurrent > 0;

	public RuntimeAnimatorController GetAnimatorController() => _controller;
	public WeaponAttachmentManager GetAttachmentManager() => _attachmentManager;

	public void Init(PlayerBehaviour playerBehaviour)
	{
		_characterBehaviour = playerBehaviour;
		
		_playerCamera = _characterBehaviour.GetCameraWorld().transform;
		_magazineBehaviour = _attachmentManager.GetEquippedMagazine();
		_muzzleBehaviour = _attachmentManager.GetEquippedMuzzle();
		_ammunitionCurrent = _magazineBehaviour.GetAmmunitionTotal(_weaponTeamState);
	}

	public void Reload()
	{
		_animator.Play(HasAmmunition() ? "Reload" : "Reload Empty", 0, 0.0f);
	}

	public void Fire(float spreadMultiplier = 1.0f)
	{
		if (_muzzleBehaviour == null)
			return;

		if (_playerCamera == null)
			return;

		Transform muzzleSocket = _muzzleBehaviour.GetSocket();

		const string stateName = "Fire";
		_animator.Play(stateName, 0, 0.0f);
		_ammunitionCurrent = Mathf.Clamp(_ammunitionCurrent - 1, 0, _magazineBehaviour.GetAmmunitionTotal(_weaponTeamState));

		_muzzleBehaviour.Effect();

		Quaternion rotation = Quaternion.LookRotation(_playerCamera.forward * 1000.0f - muzzleSocket.position);

		if (Physics.Raycast(new Ray(_playerCamera.position, _playerCamera.forward),
			    out RaycastHit hit, _maximumDistance, _mask))
			rotation = Quaternion.LookRotation(hit.point - muzzleSocket.position);

		GameObject projectile = Instantiate(_prefabProjectile, muzzleSocket.position, rotation);
		projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * _projectileImpulse;
		
		if (hit.collider != null)
		{
			if (hit.collider.TryGetComponent(out IDamageable damageable))
			{
				if (damageable != null && !damageable.IsDead)
				{
					damageable.TakeDamage(_damageConfig.GetDamage());
				}
			}
		}
	}

	public void FillAmmunition(int amount)
	{
		_ammunitionCurrent = amount != 0
			? Mathf.Clamp(_ammunitionCurrent + amount,
				0, GetAmmunitionTotal())
			: _magazineBehaviour.GetAmmunitionTotal(_weaponTeamState);
	}

	public void EjectCasing()
	{
		if (_prefabCasing != null && _socketEjection != null)
			Instantiate(_prefabCasing, _socketEjection.position, _socketEjection.rotation);
	}
}