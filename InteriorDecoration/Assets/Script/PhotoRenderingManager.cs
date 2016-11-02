using UnityEngine;
using System.Collections;
using System.Text;

public class PhotoRenderingManager : MonoBehaviour {
    public string serverUrl;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.R)) {
            //Camera cmr = Camera.main;
            string s = "this is a test!";
            StartCoroutine(RequestRendering(Encoding.ASCII.GetBytes(s)));
        }
	}

    IEnumerator RequestRendering(byte[] parameters) {
        WWW request = new WWW(serverUrl, parameters);

        yield return request;

        Debug.Log("return information from server: " + request.text);
    }
}
