using UnityEngine;
using System.Collections;
using System.Text;

public class PhotoRenderingManager : MonoBehaviour
{
    public string serverUrl;
    public GameObject funitureRoot;

    private GameObject[] funitureArray_;

    // Use this for initialization
    void Start()
    {
        if (null != funitureRoot)
        {
            funitureArray_ = new GameObject[funitureRoot.transform.childCount];
            for (int i=0; i<funitureRoot.transform.childCount; ++i)
            {
                funitureArray_[i] = funitureRoot.transform.GetChild(i).gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //StartCoroutine(RequestRendering(Encoding.ASCII.GetBytes(s)));
            //GameObject[] players = GameObject.FindGameObjectsWithTag("player");

            StartCoroutine(RequestPhotoRendering(Camera.main, funitureArray_));
        }
    }

    IEnumerator RequestPhotoRendering(Camera cmr, GameObject[] funitureArray)
    {
        Debug.Log("send rendering request.");

        WWWForm form = new WWWForm();
        string strCamer = "";
        Transform trans = cmr.transform;
        strCamer = trans.position.x.ToString() + " " + trans.position.y.ToString() + " " + trans.position.z.ToString() + " "
                   + trans.forward.x.ToString() + " " + trans.forward.y.ToString() + " " + trans.forward.z.ToString() + " "
                   + trans.up.x.ToString() + " " + trans.up.y.ToString() + " " + trans.up.z.ToString();
        form.AddField("camera", strCamer);
        string strFunitures = "";

        if (null != funitureArray)
        {
            foreach (GameObject funiture in funitureArray)
            {
                Matrix4x4 matrix = funiture.transform.localToWorldMatrix;
                strFunitures += funiture.name
                    + ",["
                    + matrix.m00.ToString() + " " + matrix.m01.ToString() + " " + matrix.m02.ToString() + " " + matrix.m03.ToString() + " "
                    + matrix.m10.ToString() + " " + matrix.m11.ToString() + " " + matrix.m12.ToString() + " " + matrix.m13.ToString() + " "
                    + matrix.m20.ToString() + " " + matrix.m21.ToString() + " " + matrix.m22.ToString() + " " + matrix.m23.ToString() + " "
                    + matrix.m30.ToString() + " " + matrix.m31.ToString() + " " + matrix.m32.ToString() + " " + matrix.m33.ToString()
                    + "];";
            }
            //form.AddField("models", strFunitures);
        }


        WWW request = new WWW(serverUrl, form);

        yield return request;
        Debug.Log("return information from server: " + request.text);
    }

    //IEnumerator RequestRendering(byte[] parameters)
    //{
    //    WWWForm form = new WWWForm();

    //    // test only.
    //    form.AddField("camera", "3.876106 -4.371810 7.572081 3.535682 -3.937974 6.737875 -0.499441 0.668262 0.551348");
    //    form.AddField("models", "model_id0,[-0.016672079674900 0.000000001007456 0.000000000000000 0.000000000000000 -0.000000001007456 -0.006672079209238 0.000000000000000 0.000000000000000 0.000000000000000 0.000000000000000 0.068586871027946 0.000000000000000 0.836706995964050 1.845357656478882 -0.614888012409210 1.000000000000000];model_id1,[0.006672079674900 0.000000001007456 0.000000000000000 0.000000000000000 -0.000000001007456 -0.006672079209238 0.000000000000000 0.000000000000000 0.000000000000000 0.000000000000000 0.068586871027946 0.000000000000000 0.836706995964050 1.845357656478882 -0.614888012409210 1.000000000000000]");
    //    serverUrl = "http://localhost/";
    //    WWW request = new WWW(serverUrl, form);

    //    yield return request;

    //    Debug.Log("return information from server: " + request.text);
    //}
}
