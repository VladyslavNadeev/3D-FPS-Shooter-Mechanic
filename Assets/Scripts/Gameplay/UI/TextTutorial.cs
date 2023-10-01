using TMPro;
using UnityEngine;

public class TextTutorial : ElementText
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _prompt;
    [SerializeField] private TextMeshProUGUI _tutorial;

   public void Init()
    {
        _prompt.enabled = true;
        _tutorial.enabled = false;
    }

    protected override void Tick()
    {
        bool isVisible = playerCharacter.IsTutorialTextVisible();
        _prompt.enabled = !isVisible;
        _tutorial.enabled = isVisible;
    }
}