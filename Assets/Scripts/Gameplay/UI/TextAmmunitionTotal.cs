using System.Globalization;

public class TextAmmunitionTotal : ElementText
{
    protected override void Tick()
    {
        float ammunitionTotal = allWeapons[(int)weaponTypeState].GetAmmunitionTotal();
            
        _textMesh.text = ammunitionTotal.ToString(CultureInfo.InvariantCulture);
    }
}