using UnityEngine;
using System.Collections;

public class MaterialPair : BasicPair {
    public Material sourceMaterial;
    public Material destMaterial;

    MeshRenderer mr;
	// Use this for initialization
	void Start () {
        mr = gameObject.GetComponent<MeshRenderer>();
	}
	
    public void Replace()
    {
        mr.material = destMaterial;
    }

    public void Reset()
    {
        mr.material = sourceMaterial;
    }
}
