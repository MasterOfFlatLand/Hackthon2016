using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using VRTK;

public class PhotoRenderingManager : MonoBehaviour
{
    public string serverUrl;
    public GameObject funitureRoot;
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
        StartCoroutine(RequestPhotoRendering(Camera.main, funitureArray_));
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

        Transform trans = cmr.transform;
        string strCamer = serializeVector(trans.position) + " " +
            serializeVector(trans.forward) + " " +
            serializeVector(trans.up);

        form.AddField("camera", strCamer);

        if (null != funitureArray)
        {
            string strFunitures = "";

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
}
