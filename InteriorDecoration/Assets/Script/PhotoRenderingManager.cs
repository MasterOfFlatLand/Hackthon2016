using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using VRTK;

public class PhotoRenderingManager : MonoBehaviour
{
    public string serverUrl;
    public GameObject funitureRoot;
    public Camera eye;
    public GameObject photoRndInfoUI;
    public float uiVisibleDuration = 5;

    private GameObject[] funitureArray_;
    private bool photoRndConfirmed = false;
    private bool photoRndSent = false;

    private IEnumerator uiCoroutine;

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

        VRTK_ControllerEvents[] events = gameObject.GetComponentsInChildren<VRTK_ControllerEvents>(true);
        if (null != events)
        {
            foreach (var ev in events)
            {
                ev.GripPressed += new ControllerInteractionEventHandler(PhotoRenderingRequest);
            }
        }
        else
        {
            Debug.LogError("VRTK_ControllerEvents scripts not found.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RequestPhotoRendering();
        }
    }

    public void Reset()
    {
        if (null != uiCoroutine)
        {
            StopCoroutine(uiCoroutine);
            uiCoroutine = null;
        }

        if (null != photoRndInfoUI)
        {
            photoRndInfoUI.SetActive(false);
        }

        photoRndConfirmed = false;
        photoRndSent = false;
    }

    private void PhotoRenderingRequest(object sender, ControllerInteractionEventArgs e)
    {
        if (photoRndSent)
        {
            Text rndInfo = photoRndInfoUI.transform.GetChild(1).GetComponent<Text>();
            rndInfo.text = "渲染请求已发送，请检查云渲染网站！";

            ShowInfoUI();
        }
        else if (!photoRndConfirmed)
        {
            Text rndInfo = photoRndInfoUI.transform.GetChild(1).GetComponent<Text>();
            rndInfo.text = "想要渲染当前场景？\n请再次按下“侧按钮”!";

            ShowInfoUI();
            photoRndConfirmed = true;
        }
        else
        {
            RequestPhotoRendering();
            photoRndSent = true;
        }
    }

    void ShowInfoUI()
    {
        if (null != uiCoroutine)
        {
            StopCoroutine(uiCoroutine);
        }

        uiCoroutine = ShowInfoUIRoutine(photoRndInfoUI, uiVisibleDuration);
        StartCoroutine(uiCoroutine);
    }

    IEnumerator ShowInfoUIRoutine(GameObject uiObj, float duration)
    {
        uiObj.SetActive(true);
        yield return new WaitForSeconds(duration);
        uiObj.SetActive(false);
        uiCoroutine = null;
    }

    private void RequestPhotoRendering()
    {   
        StartCoroutine(RequestPhotoRendering(eye, funitureArray_));
    }

    delegate string SerializeVecFunc(Vector3 v);
    SerializeVecFunc serializeVector = (Vector3 v) =>
    {
        return string.Format("{0} {1} {2}", v.x, v.y, v.z);
    };

    IEnumerator RequestPhotoRendering(Camera cmr, GameObject[] funitureArray)
    {
        Debug.Log("send rendering request.");

        WWWForm form = new WWWForm();

        Transform transform = cmr.transform;
        string strCamer = serializeVector(transform.position) + " " +
            serializeVector(transform.forward) + " " +
            serializeVector(transform.up);
        Vector3 campos = transform.position;

        form.AddField("camera", strCamer);

        if (null != funitureArray)
        {
            Matrix4x4 mLU = new Matrix4x4();
            mLU.SetRow(0, new Vector4(-1, 0, 0, 0));
            mLU.SetRow(1, new Vector4(0, 0, 1, 0));
            mLU.SetRow(2, new Vector4(0, -1, 0, 0));
            mLU.SetRow(3, new Vector4(0, 0, 0, 1));
            Matrix4x4 mUL = new Matrix4x4();
            mUL.SetRow(0, new Vector4(-1, 0, 0, 0));
            mUL.SetRow(1, new Vector4(0, 0, -1, 0));
            mUL.SetRow(2, new Vector4(0, 1, 0, 0));
            mUL.SetRow(3, new Vector4(0, 0, 0, 1));
            //Matrix4x4 mt = mLU * mUL;
            //Vector3 camposLux = mUL.MultiplyPoint(campos);
            string strFunitures = "";
            foreach (GameObject funiture in funitureArray)
            {
                Matrix4x4 matrixi = funiture.transform.localToWorldMatrix;
                Matrix4x4 matrixo = mUL * matrixi * mLU;
                strFunitures += funiture.name
                    + ",["
                    + matrixo.m00.ToString() + " " + matrixo.m01.ToString() + " " + matrixo.m02.ToString() + " " + matrixo.m03.ToString() + " "
                    + matrixo.m10.ToString() + " " + matrixo.m11.ToString() + " " + matrixo.m12.ToString() + " " + matrixo.m13.ToString() + " "
                    + matrixo.m20.ToString() + " " + matrixo.m21.ToString() + " " + matrixo.m22.ToString() + " " + matrixo.m23.ToString() + " "
                    + matrixo.m30.ToString() + " " + matrixo.m31.ToString() + " " + matrixo.m32.ToString() + " " + matrixo.m33.ToString()
                    + "];";
            }
            form.AddField("models", strFunitures);
        }


        WWW request = new WWW(serverUrl, form);

        yield return request;
        Debug.Log("return information from server: " + request.text);
    }
}
