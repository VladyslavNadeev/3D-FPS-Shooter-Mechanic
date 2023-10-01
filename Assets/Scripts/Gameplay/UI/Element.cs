using System;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    [SerializeField] protected WeaponTypeState weaponTypeState;
    
    protected PlayerBehaviour playerCharacter;
    protected Inventory playerCharacterInventory;
    protected Weapon equippedWeapon;

    protected Weapon[] allWeapons;

    public void Init(PlayerBehaviour playerBehaviour)
    {
        playerCharacter = playerBehaviour;
        playerCharacterInventory = playerCharacter.GetInventory();
    }

    private void Update()
    {
        if (Equals(playerCharacterInventory, null))
            return;

        equippedWeapon = playerCharacterInventory.EquippedWeapon;
        allWeapons = playerCharacterInventory.Weapons;
        
        Tick();
    }
    
    protected virtual void Tick() {}
}