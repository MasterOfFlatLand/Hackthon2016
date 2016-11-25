using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using VRTK;

public class Matrix3x3
{
    public static Matrix3x3 identity = new Matrix3x3(1, 0, 0, 0, 1, 0, 0, 0, 1);
    public static Matrix3x3 zero = new Matrix3x3(0, 0, 0, 0, 0, 0, 0, 0, 0);
    public static Matrix3x3 sz = new Matrix3x3(1, 0, 0, 0, 0, 1, 0, 1, 0);

    public float[,] matrix;
    public Matrix3x3(float a11, float a12, float a13, float a21, float a22, float a23, float a31, float a32, float a33)
    {
        matrix = new float[3, 3];
        matrix[0, 0] = a11;
        matrix[1, 0] = a12;
        matrix[2, 0] = a13;
        matrix[0, 1] = a21;
        matrix[1, 1] = a22;
        matrix[2, 1] = a23;
        matrix[0, 2] = a31;
        matrix[1, 2] = a32;
        matrix[2, 2] = a33;
    }
    //MATRIX MULTIPLICATION
    public static Matrix3x3 operator *(Matrix3x3 m1, Matrix3x3 m2)
    {
        float a11 =
            m1.matrix[0, 0] * m2.matrix[0, 0] +
            m1.matrix[1, 0] * m2.matrix[0, 1] +
            m1.matrix[2, 0] * m2.matrix[0, 2];
        float a12 =
            m1.matrix[0, 0] * m2.matrix[1, 0] +
            m1.matrix[1, 0] * m2.matrix[1, 1] +
            m1.matrix[2, 0] * m2.matrix[1, 2];
        float a13 =
            m1.matrix[0, 0] * m2.matrix[2, 0] +
            m1.matrix[1, 0] * m2.matrix[2, 1] +
            m1.matrix[2, 0] * m2.matrix[2, 2];
        float a21 =
            m1.matrix[0, 1] * m2.matrix[0, 0] +
            m1.matrix[1, 1] * m2.matrix[0, 1] +
            m1.matrix[2, 1] * m2.matrix[0, 2];
        float a22 =
            m1.matrix[0, 1] * m2.matrix[1, 0] +
            m1.matrix[1, 1] * m2.matrix[1, 1] +
            m1.matrix[2, 1] * m2.matrix[1, 2];
        float a23 =
            m1.matrix[0, 1] * m2.matrix[2, 0] +
            m1.matrix[1, 1] * m2.matrix[2, 1] +
            m1.matrix[2, 1] * m2.matrix[2, 2];
        float a31 =
            m1.matrix[0, 2] * m2.matrix[0, 0] +
            m1.matrix[1, 2] * m2.matrix[0, 1] +
            m1.matrix[2, 2] * m2.matrix[0, 2];
        float a32 =
            m1.matrix[0, 2] * m2.matrix[1, 0] +
            m1.matrix[1, 2] * m2.matrix[1, 1] +
            m1.matrix[2, 2] * m2.matrix[1, 2];
        float a33 =
            m1.matrix[0, 2] * m2.matrix[2, 0] +
            m1.matrix[1, 2] * m2.matrix[2, 1] +
            m1.matrix[2, 2] * m2.matrix[2, 2];
        return new Matrix3x3(a11, a12, a13, a21, a22, a23, a31, a32, a33);
    }
    public static Vector3 MultiplyPoint(Matrix3x3 m, Vector3 point)
    {
        Vector3 newPoint;
        newPoint.x = m.matrix[0, 0] * point.x + m.matrix[1, 0] * point.y + m.matrix[2, 0] * point.z;
        newPoint.y = m.matrix[0, 1] * point.x + m.matrix[1, 1] * point.y + m.matrix[2, 1] * point.z;
        newPoint.z = m.matrix[0, 2] * point.x + m.matrix[1, 2] * point.y + m.matrix[2, 2] * point.z;
        return newPoint;
    }
}

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
                Matrix4x4 matrixi = funiture.transform.localToWorldMatrix;
                Matrix3x3 matrs = new Matrix3x3(
                    matrixi.m00, matrixi.m01, matrixi.m02,
                    matrixi.m10, matrixi.m11, matrixi.m12,
                    matrixi.m20, matrixi.m21, matrixi.m22);
                Vector3 transi = new Vector3(matrixi.m03, matrixi.m13, matrixi.m23);
                Matrix3x3 matrso = Matrix3x3.sz * matrs * Matrix3x3.sz;
                float[,] matrixo = matrso.matrix;
                Vector3 transo = Matrix3x3.MultiplyPoint(Matrix3x3.sz, transi);
                strFunitures += funiture.name
                    + ",["
                    + matrixo[0,0].ToString() + " " + matrixo[0,1].ToString() + " " + matrixo[0,2].ToString() + " " + transo.x.ToString() + " "
                    + matrixo[1,0].ToString() + " " + matrixo[1,1].ToString() + " " + matrixo[1,2].ToString() + " " + transo.y.ToString() + " "
                    + matrixo[2,0].ToString() + " " + matrixo[2,1].ToString() + " " + matrixo[2,2].ToString() + " " + transo.z.ToString() + " "
                    + matrixi.m30.ToString() + " " + matrixi.m31.ToString() + " " + matrixi.m32.ToString() + " " + matrixi.m33.ToString()
                    + "];";
            }
            //form.AddField("models", strFunitures);
        }


        WWW request = new WWW(serverUrl, form);

        yield return request;
        Debug.Log("return information from server: " + request.text);
    }
}
