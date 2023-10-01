using UnityEngine;

public class Scope : ScopeBehaviour
{
    [Header("Interface")]
    [SerializeField] private Sprite sprite;
    
    public override Sprite GetSprite() => sprite;
}