using UnityEngine;
using System.Collections;

public class TargetToCamera : MonoBehaviour {
    public Camera targetCamera;

	// Use this for initialization
	void Start () {
	    if (null == targetCamera)
        {
            targetCamera = Camera.main;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Transform trans = this.gameObject.transform;
        Vector3 cmrTarVec = trans.position - targetCamera.transform.position;
        Vector3 tarDir = (cmrTarVec - Vector3.Dot(cmrTarVec, trans.up) * trans.up).normalized;

        //Vector3 tarForw = (trans.position - targetCamera.transform.position).normalized;
        //Vector3 cmrForw = targetCamera.transform.forward;
        //Vector3 upProj = Vector3.Dot(trans.up, cmrForw) * trans.up;
        //Vector3 tarProj = (cmrForw - upProj).normalized;
        float rotDeg = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(trans.forward, tarDir));
        if (rotDeg > Mathf.Epsilon)
        {
            Vector3 rotAxis = Vector3.Cross(trans.forward, tarDir);
            trans.Rotate(rotAxis, rotDeg, Space.World);
        }
    }
}
