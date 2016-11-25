using UnityEngine;
using System.Collections;

public class FurnitureStylePair : MonoBehaviour {
    public GameObject pairFurniture;

	// Use this for initialization
	void Start () {
        pairFurniture.SetActive(false);
	}

    public void Reset()
    {
        pairFurniture.SetActive(false);
    }
}
