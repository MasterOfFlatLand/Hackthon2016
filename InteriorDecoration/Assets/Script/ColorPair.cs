using UnityEngine;
using System.Collections;

public class ColorPair :  BasicPair {
    public Color colorPair;
    public Material targetMaterial;

    private Color originalColor;

    void OnDestroy()
    {
        Reset();
    }

    public override void Replace()
    {
        targetMaterial.color = colorPair;
    }

    public override void Reset()
    {
        targetMaterial.color = originalColor;
    }
}
