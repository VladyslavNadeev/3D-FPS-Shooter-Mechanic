using UnityEngine;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    
    private void OnEjectCasing()
    {
        if(_weapon != null)
            _weapon.EjectCasing();
    }
}