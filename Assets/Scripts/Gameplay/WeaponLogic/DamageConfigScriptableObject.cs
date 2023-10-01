using UnityEngine;

[CreateAssetMenu(fileName = "Damage Config", menuName = "Configs/Damage Config", order = 1)]
public class DamageConfigScriptableObject : ScriptableObject
{
    public int Damage;

    private void Reset()
    {
        Damage = 0;
    }

    public int GetDamage()
    {
        return Damage;
    }
}



















