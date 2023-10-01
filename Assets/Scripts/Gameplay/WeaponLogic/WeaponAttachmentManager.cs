using UnityEngine;

public class WeaponAttachmentManager : MonoBehaviour
{
	[Header("Scope")]
	[SerializeField] private bool _scopeDefaultShow = true;
	[SerializeField] private ScopeBehaviour _scopeDefaultBehaviour;
        
	[Header("Muzzle")]
	[SerializeField] private int _muzzleIndex;
	[SerializeField] private MuzzleBehaviour[] _muzzleArray;
        
	[Header("Magazine")]
	[SerializeField] private int _magazineIndex;
	[SerializeField] private Magazine[] magazineArray;

	private ScopeBehaviour _scopeBehaviour;
	private MuzzleBehaviour _muzzleBehaviour;
	private Magazine _magazineBehaviour;
	
	public ScopeBehaviour GetEquippedScope() => _scopeBehaviour;
	public ScopeBehaviour GetEquippedScopeDefault() => _scopeDefaultBehaviour;
	public Magazine GetEquippedMagazine() => _magazineBehaviour;
	public MuzzleBehaviour GetEquippedMuzzle() => _muzzleBehaviour;

	public void Init()
	{
		if (_scopeBehaviour == null)
		{
			_scopeBehaviour = _scopeDefaultBehaviour;
			_scopeBehaviour.gameObject.SetActive(_scopeDefaultShow);
		}
            
		_muzzleBehaviour = _muzzleArray.SelectAndSetActive(_muzzleIndex);

		_magazineBehaviour = magazineArray.SelectAndSetActive(_magazineIndex);
	}
}