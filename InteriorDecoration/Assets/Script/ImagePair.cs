using UnityEngine;
using System.Collections;

public class ImagePair : BasicPair {
    public Texture imagePair;
    public Material targetMaterial;

    private Texture origianlImage;

    void OnDestroy()
    {
        Reset();
    }

    public void Replace()
    {
        targetMaterial.mainTexture = imagePair;
    }

    public void Reset()
    {
        targetMaterial.mainTexture = origianlImage;
    }
}
