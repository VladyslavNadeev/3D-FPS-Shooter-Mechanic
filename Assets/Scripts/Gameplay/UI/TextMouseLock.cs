public class TextMouseLock : ElementText
{
    protected override void Tick()
    {
        _textMesh.text = "Cursor " + (playerCharacter.IsCursorLocked() ? "Locked" : "Unlocked");
    }
}