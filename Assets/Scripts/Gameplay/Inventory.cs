using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;
    private Weapon equipped;

    private int equippedIndex = -1;

    public Weapon EquippedWeapon
    {
        get => equipped; 
        set => equipped = value;
    }

    public Weapon[] Weapons => weapons;
    public int EquippedIndex => equippedIndex;

    public void Init(int equippedAtStart = 0)
    {
        Equip(equippedAtStart);
    }

    public Weapon Equip(int index)
    {
        if (weapons == null)
            return equipped;

        if (index > weapons.Length - 1)
            return equipped;

        if (equippedIndex == index)
            return equipped;

        if (equipped != null)
            equipped.gameObject.SetActive(false);

        equippedIndex = index;
        equipped = weapons[equippedIndex];
        equipped.gameObject.SetActive(true);

        return equipped;
    }

    public int GetLastIndex()
    {
        int newIndex = equippedIndex - 1;
        if (newIndex < 0)
            newIndex = weapons.Length - 1;

        return newIndex;
    }

    public int GetNextIndex()
    {
        int newIndex = equippedIndex + 1;
        if (newIndex > weapons.Length - 1)
            newIndex = 0;

        return newIndex;
    }
}