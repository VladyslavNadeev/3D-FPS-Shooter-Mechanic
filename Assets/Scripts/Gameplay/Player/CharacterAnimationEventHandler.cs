using UnityEngine;

public class CharacterAnimationEventHandler : MonoBehaviour
{
    private PlayerBehaviour _playerCharacter;

    public void Init(PlayerBehaviour playerBehaviour)
    {
        _playerCharacter = playerBehaviour;
    }

    private void OnEjectCasing()
    {
        if(_playerCharacter != null)
            _playerCharacter.EjectCasing();
    }

    private void OnAmmunitionFill(int amount = 0)
    {
        if(_playerCharacter != null)
            _playerCharacter.FillAmmunition(amount);
    }
    
    private void OnSetActiveKnife(int active)
    {
    }
		
    private void OnGrenade()
    {
    }
    
    private void OnSetActiveMagazine(int active)
    {
        if(_playerCharacter != null)
            _playerCharacter.SetActiveMagazine(active);
    }

    private void OnAnimationEndedBolt()
    {
    }
    
    private void OnAnimationEndedReload()
    {
        if(_playerCharacter != null)
            _playerCharacter.AnimationEndedReload();
    }

    private void OnAnimationEndedGrenadeThrow()
    {
    }
    
    private void OnAnimationEndedMelee()
    {
    }

    private void OnAnimationEndedInspect()
    {
        if(_playerCharacter != null)
            _playerCharacter.AnimationEndedInspect();
    }
    
    private void OnAnimationEndedHolster()
    {
        if(_playerCharacter != null)
            _playerCharacter.AnimationEndedHolster();
    }
    
    private void OnSlideBack(int back)
    {
    }
}