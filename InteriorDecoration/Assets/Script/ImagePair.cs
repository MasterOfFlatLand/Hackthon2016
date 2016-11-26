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

    public override void Replace()
    {
        targetMaterial.mainTexture = imagePair;
    }

    public override void Reset()
    {
        targetMaterial.mainTexture = origianlImage;
    }
}
