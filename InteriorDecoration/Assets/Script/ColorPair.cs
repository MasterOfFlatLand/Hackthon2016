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

	public void Replace()
    {
        targetMaterial.color = colorPair;
    }

    public void Reset()
    {
        targetMaterial.color = originalColor;
    }
}
