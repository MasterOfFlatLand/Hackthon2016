using UnityEngine;
using System.Collections;

public class BasicPair : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Reset();
	}

    public virtual void Replace() { }
    public virtual void Reset() { }
}
