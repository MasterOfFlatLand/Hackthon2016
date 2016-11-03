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
        WWWForm form = new WWWForm();

        // test only.
        form.AddField("camera", "3.876106 -4.371810 7.572081 3.535682 -3.937974 6.737875 -0.499441 0.668262 0.551348");
        form.AddField("models", "model_id0,[-0.006672079674900 0.000000001007456 0.000000000000000 0.000000000000000 -0.000000001007456 -0.006672079209238 0.000000000000000 0.000000000000000 0.000000000000000 0.000000000000000 0.068586871027946 0.000000000000000 0.836706995964050 1.845357656478882 -0.614888012409210 1.000000000000000];model_id1,[0.006672079674900 0.000000001007456 0.000000000000000 0.000000000000000 -0.000000001007456 -0.006672079209238 0.000000000000000 0.000000000000000 0.000000000000000 0.000000000000000 0.068586871027946 0.000000000000000 0.836706995964050 1.845357656478882 -0.614888012409210 1.000000000000000]");

        WWW request = new WWW(serverUrl, form);

        yield return request;

        Debug.Log("return information from server: " + request.text);
    }
}
