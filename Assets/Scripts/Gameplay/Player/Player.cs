using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class Player : PlayerBehaviour
{
	[Header("Inventory")] [SerializeField] private Inventory _inventory;

	[Header("Camera")] [SerializeField] private Camera _cameraWorld;

	[Header("Animation")] 
	[SerializeField] private float _dampTimeLocomotion = 0.15f;
	[SerializeField] private float _dampTimeAiming = 0.3f;
	[SerializeField] private Animator _playerAnimator;

	[SerializeField] private PlayerVitality _playerVitality;
	[SerializeField] private PlayerKinematics _playerKinematics;

	private static readonly int HashAimingAlpha = Animator.StringToHash("Aiming");
	private static readonly int HashMovement = Animator.StringToHash("Movement");

	private bool _aiming;
	private bool _running;
	private bool _holstered;
	private float _lastShotTime;

	private int _layerOverlay;
	private int _layerHolster;
	private int _layerActions;

	private Weapon _equippedWeapon;
	private WeaponAttachmentManager _weaponAttachmentManager;
	private ScopeBehaviour _equippedWeaponScope;
	private Magazine _equippedWeaponMagazine;

	private Vector2 _axisLook;
	private Vector2 _axisMovement;

	private bool _reloading;
	private bool _inspecting;
	private bool _holstering;
	private bool _holdingButtonAim;
	private bool _holdingButtonRun;
	private bool _holdingButtonFire;
	private bool _tutorialTextVisible;
	private bool _cursorLocked;
	private bool _canJump;

	public bool CursorLocked
	{
		get => _cursorLocked;
		set => _cursorLocked = value;
	}

	public override Camera GetCameraWorld() => _cameraWorld;
	public override Inventory GetInventory() => _inventory;
	public override bool IsCrosshairVisible() => !_aiming && !_holstered;
	public override bool IsRunning() => _running;
	public override bool IsAiming() => _aiming;
	public override bool IsCursorLocked() => _cursorLocked;
	public override bool IsTutorialTextVisible() => _tutorialTextVisible;
	public override Vector2 GetInputMovement() => _axisMovement;
	public override Vector2 GetInputLook() => _axisLook;

	public void Init()
	{
		_cursorLocked = true;
		UpdateCursorState();
		_inventory.Init();
		RefreshWeaponSetup();
		
		_layerHolster = _playerAnimator.GetLayerIndex("Layer Holster");
		_layerActions = _playerAnimator.GetLayerIndex("Layer Actions");
		_layerOverlay = _playerAnimator.GetLayerIndex("Layer Overlay");
	}

	protected override void Update()
	{
		_aiming = _holdingButtonAim && CanAim();
		_running = _holdingButtonRun && CanRun();

		if (_holdingButtonFire)
		{
			if (CanPlayAnimationFire() && _equippedWeapon.HasAmmunition() && _equippedWeapon.IsAutomatic())
			{
				if (Time.time - _lastShotTime > 60.0f / _equippedWeapon.GetRateOfFire())
					Fire();
			}
		}

		UpdateAnimator();
	}

	protected override void LateUpdate()
	{
		if (_equippedWeapon == null)
			return;

		if (_equippedWeaponScope == null)
			return;

		if (_playerKinematics != null)
		{
			_playerKinematics.Compute();
		}
	}

	private void UpdateAnimator()
	{
		_playerAnimator.SetFloat(HashMovement, Mathf.Clamp01(Mathf.Abs(_axisMovement.x) + Mathf.Abs(_axisMovement.y)), _dampTimeLocomotion, Time.deltaTime);
			
		_playerAnimator.SetFloat(HashAimingAlpha, Convert.ToSingle(_aiming), 0.25f / 1.0f * _dampTimeAiming, Time.deltaTime);

		const string boolNameAim = "Aim";
		_playerAnimator.SetBool(boolNameAim, _aiming);
			
		const string boolNameRun = "Running";
		_playerAnimator.SetBool(boolNameRun, _running);
	}
	
	private void Inspect()
	{
		_inspecting = true;
		_playerAnimator.CrossFade("Inspect", 0.0f, _layerActions, 0);
	}

	private void Fire()
	{
		_lastShotTime = Time.time;
		_equippedWeapon.Fire();

		const string stateName = "Fire";
		_playerAnimator.CrossFade(stateName, 0.05f, _layerOverlay, 0);
	}

	private void PlayReloadAnimation()
	{
		string stateName = _equippedWeapon.HasAmmunition() ? "Reload" : "Reload Empty";
		_playerAnimator.Play(stateName, _layerActions, 0.0f);

		_reloading = true;

		_equippedWeapon.Reload();
	}
	
	private IEnumerator Equip(int index = 0)
	{
		if (!_holstered)
		{
			SetHolstered(_holstering = true);
			yield return new WaitUntil(() => _holstering == false);
		}

		SetHolstered(false);
		_playerAnimator.Play("Unholster", _layerHolster, 0);

		_inventory.Equip(index);
		RefreshWeaponSetup();
	}
	
	private void RefreshWeaponSetup()
	{
		if ((_equippedWeapon = _inventory.EquippedWeapon) == null)
			return;

		_playerAnimator.runtimeAnimatorController = _equippedWeapon.GetAnimatorController();

		_weaponAttachmentManager = _equippedWeapon.WeaponAttachmentManager;
		if (_weaponAttachmentManager == null)
			return;

		_equippedWeaponScope = _weaponAttachmentManager.GetEquippedScope();
		_equippedWeaponMagazine = _weaponAttachmentManager.GetEquippedMagazine();
	}

	private void FireEmpty()
	{
		_lastShotTime = Time.time;
		_playerAnimator.CrossFade("Fire Empty", 0.05f, _layerOverlay, 0);
	}
	
	public void UpdateCursorState()
	{
		Cursor.visible = !_cursorLocked;
		Cursor.lockState = _cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
	}

	public override bool CanJump()
	{
		return _canJump;
	}

	private void SetHolstered(bool value = true)
	{
		_holstered = value;

		const string boolName = "Holstered";
		_playerAnimator.SetBool(boolName, _holstered);
	}

	private bool CanPlayAnimationFire()
	{
		if (_holstered || _holstering)
			return false;

		if (_reloading)
			return false;

		if (_inspecting)
			return false;

		return true;
	}

	private bool CanPlayAnimationReload()
	{
		if (_reloading)
			return false;

		if (_inspecting)
			return false;

		return true;
	}

	private bool CanPlayAnimationHolster()
	{
		if (_reloading)
			return false;

		if (_inspecting)
			return false;

		return true;
	}

	private bool CanChangeWeapon()
	{
		if (_holstering)
			return false;

		if (_reloading)
			return false;

		if (_inspecting)
			return false;

		return true;
	}

	private bool CanPlayAnimationInspect()
	{
		if (_holstered || _holstering)
			return false;

		if (_reloading)
			return false;

		if (_inspecting)
			return false;

		return true;
	}

	private bool CanAim()
	{
		if (_holstered || _inspecting)
			return false;

		if (_reloading || _holstering)
			return false;

		return true;
	}

	private bool CanRun()
	{
		if (_inspecting)
			return false;

		if (_reloading || _aiming)
			return false;

		if (_holdingButtonFire && _equippedWeapon.HasAmmunition())
			return false;

		if (_axisMovement.y <= 0 || Math.Abs(Mathf.Abs(_axisMovement.x) - 1) < 0.01f)
			return false;

		return true;
	}

	public void OnTryFire(InputAction.CallbackContext context)
	{
		if (!_cursorLocked)
			return;

		switch (context)
		{
			case { phase: InputActionPhase.Started }:
				_holdingButtonFire = true;
				break;
			
			case { phase: InputActionPhase.Performed }:
				if (!CanPlayAnimationFire())
					break;

				if (_equippedWeapon.HasAmmunition())
				{
					if (_equippedWeapon.IsAutomatic())
						break;

					if (Time.time - _lastShotTime > 60.0f / _equippedWeapon.GetRateOfFire())
						Fire();
				}
				else
					FireEmpty();

				break;
			
			case { phase: InputActionPhase.Canceled }:
				_holdingButtonFire = false;
				break;
		}
	}

	public void OnTryPlayReload(InputAction.CallbackContext context)
	{
		if (!_cursorLocked)
			return;

		if (!CanPlayAnimationReload())
			return;

		switch (context)
		{
			case { phase: InputActionPhase.Performed }:
				PlayReloadAnimation();
				break;
		}
	}

	public void OnTryInspect(InputAction.CallbackContext context)
	{
		if (!_cursorLocked)
			return;

		if (!CanPlayAnimationInspect())
			return;

		switch (context)
		{
			case { phase: InputActionPhase.Performed }:
				Inspect();
				break;
		}
	}

	public void OnTryAiming(InputAction.CallbackContext context)
	{
		if (!_cursorLocked)
			return;

		switch (context.phase)
		{
			case InputActionPhase.Started:
				_holdingButtonAim = true;
				break;
			
			case InputActionPhase.Canceled:
				_holdingButtonAim = false;
				break;
		}
	}

	public void OnTryHolster(InputAction.CallbackContext context)
	{
		if (!_cursorLocked)
			return;

		switch (context.phase)
		{
			case InputActionPhase.Performed:
				if (CanPlayAnimationHolster())
				{
					SetHolstered(!_holstered);
					_holstering = true;
				}

				break;
		}
	}

	public void OnTryRun(InputAction.CallbackContext context)
	{
		if (!_cursorLocked)
			return;

		switch (context.phase)
		{
			case InputActionPhase.Started:
				_holdingButtonRun = true;
				break;
			
			case InputActionPhase.Canceled:
				_holdingButtonRun = false;
				break;
		}
	}

	public void OnTryJump(InputAction.CallbackContext context)
	{
		if (!_cursorLocked)
			return;

		switch (context.phase)
		{
			case InputActionPhase.Started:
				_canJump = true;
				break;
			
			case InputActionPhase.Canceled:
				_canJump = false;
				break;
		}
	}

	public void OnTryInventoryNext(InputAction.CallbackContext context)
	{
		if (!_cursorLocked)
			return;

		if (_inventory == null)
			return;

		switch (context)
		{
			case { phase: InputActionPhase.Performed }:
				float scrollValue = context.valueType.IsEquivalentTo(typeof(Vector2))
					? Mathf.Sign(context.ReadValue<Vector2>().y)
					: 1.0f;

				int indexNext = scrollValue > 0 ? _inventory.GetNextIndex() : _inventory.GetLastIndex();
				int indexCurrent = _inventory.EquippedIndex;

				if (CanChangeWeapon() && (indexCurrent != indexNext))
					StartCoroutine(nameof(Equip), indexNext);
				break;
		}
	}

	public void OnLockCursor(InputAction.CallbackContext context)
	{
		switch (context)
		{
			case { phase: InputActionPhase.Performed }:
				_cursorLocked = !_cursorLocked;
				UpdateCursorState();
				break;
		}
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		_axisMovement = _cursorLocked ? context.ReadValue<Vector2>() : default;
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		_axisLook = _cursorLocked ? context.ReadValue<Vector2>() : default;
	}

	public void OnUpdateTutorial(InputAction.CallbackContext context)
	{
		_tutorialTextVisible = context switch
		{
			{ phase: InputActionPhase.Started } => true,
			{ phase: InputActionPhase.Canceled } => false,
			_ => _tutorialTextVisible
		};
	}

	public override void EjectCasing()
	{
		if (_equippedWeapon != null)
			_equippedWeapon.EjectCasing();
	}

	public override void FillAmmunition(int amount)
	{
		if (_equippedWeapon != null)
			_equippedWeapon.FillAmmunition(amount);
	}

	public override void SetActiveMagazine(int active)
	{
		_equippedWeaponMagazine.gameObject.SetActive(active != 0);
	}

	public override void AnimationEndedReload()
	{
		_reloading = false;
	}

	public override void AnimationEndedInspect()
	{
		_inspecting = false;
	}

	public override void AnimationEndedHolster()
	{
		_holstering = false;
	}

}