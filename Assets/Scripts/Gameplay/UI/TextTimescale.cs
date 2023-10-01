using UnityEngine;

public class TextTimescale : ElementText
{
    protected override void Tick()
    {
        _textMesh.text = "Timescale : " + Time.timeScale;
    }        
}