using UnityEngine;
using System.Collections;

public class MaterialPair : BasicPair {
    public Material[] materialPair;

    int currentIndex;
    MeshRenderer mr;
	// Use this for initialization
	void Start () {
        mr = gameObject.GetComponent<MeshRenderer>();
        mr.material = materialPair[0];
        currentIndex = 0;
	}
	
    public override void Replace()
    {
        mr.material = materialPair[(++currentIndex)%2];
    }

    public override void Reset()
    {
        mr.material = materialPair[0];
        currentIndex = 0;
    }
}
