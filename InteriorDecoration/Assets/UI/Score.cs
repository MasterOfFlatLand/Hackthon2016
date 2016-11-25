using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour 
{
    public int nBlood;
    public int nFurFixedNum;
    public int nFurTotalNum;
    public GameObject nextfurniture;
    public GameObject jiaModel;
    public bool bIsSkillTriggered;
    public int nSkillNum;

    private int nAngle = 10;
    
    //private Image imgBackGround;
    //private Image imgBlood;

    private Image imgSkill1;
    private Image imgSkill2;
    private Image imgSkill3;
    private Text scoreText;
    private Text skillText;
    private int nOriginBlood;
    private int nBloodWidth = Screen.width / 5;
    //private int nBloodHeight = Screen.height / 30;

	void Start () 
    {
        nOriginBlood = nBlood;
        InitUIElements();
        InitGameObject();
        StartCoroutine(BloodTimer());
        StartCoroutine(GameObjectRotateTimer()); 
	}

    void InitUIElements()
    {
        Transform child = transform.Find("BackImage");
        //imgBackGround = child.GetComponent<Image>();
        //imgBackGround.rectTransform.sizeDelta = new Vector2(nBloodWidth, nBloodHeight);
        //child = transform.Find("BloodImage");
        //nBloodWidth = (int)((child as RectTransform).rect.width);
        //imgBlood = child.GetComponent<Image>();
        child = transform.Find("SkillImage1");
        imgSkill1 = child.GetComponent<Image>();
        child = transform.Find("SkillImage2");
        imgSkill2 = child.GetComponent<Image>();
        child = transform.Find("SkillImage3");
        imgSkill3 = child.GetComponent<Image>();

        child = transform.Find("ScoreText");
        scoreText = child.GetComponent<Text>();
        child = transform.Find("SkillText");
        skillText = child.GetComponent<Text>();
    }

    void InitGameObject()
    {
        if (null == nextfurniture)
        {
            return;
        }

        float fX = 3.5f;
        float fY = 8.5f;

        Vector3 loc = new Vector3(fX, fY, 0);
        //nextfurniture.layer = 5;
        nextfurniture.transform.localPosition = loc;

        //Vector3 objectSize = Vector3.Scale(nextfurniture.transform.localScale, nextfurniture.GetComponent<Renderer>().bounds.size);
        
        //float nMax = Mathf.Max(Mathf.Max(objectSize.x, objectSize.y), objectSize.z);
        //float fScale = 0.0f;
        //if (nMax - 0.0 < 0.00001)
        //{
        //    fScale = 0.5f;
        //}
        //else
        //{
        //    fScale = (Screen.height / 500.0f) / nMax;
        //}

        BoxCollider bc = nextfurniture.GetComponent<BoxCollider>();
        if (null != bc)
        {
            float revScale = 1.0f / Mathf.Max(Mathf.Max(bc.size.x, bc.size.y), bc.size.z);

            nextfurniture.transform.localScale = bc.size * revScale;
        }
    }

    IEnumerator BloodTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (nBlood > 0)
            {
                nBlood--;
            }
            else
            {
                break;
            }
        }
    }

	void Update () 
    {
        UpdateScore();
        UpdateSkill();
        //UpdateBlood();
        //jiaModel.transform.Rotate(Vector3.up * 2 * Time.deltaTime);
        //jiaModel.transform.RotateAround(transform.position, transform.up, Time.deltaTime * 10f);
	}

    void UpdateScore()
    {
        //scoreText.text = "SCORE " + nFurFixedNum.ToString() + " / " + nFurTotalNum.ToString();
    }

    void UpdateBlood()
    {
        //imgBlood.rectTransform.sizeDelta = new Vector2(nBloodWidth * nBlood / nOriginBlood, nBloodHeight);
        //imgBlood.rectTransform.rect.width = ()
    }

    IEnumerator GameObjectRotateTimer()
    {
        while (true && nextfurniture)
        {
            yield return new WaitForSeconds(0.1f);
            nAngle += 15;
            //nextfurniture.transform.localRotation = Quaternion.Euler(nAngle, nAngle, nAngle);
            //jiaModel.transform.Rotate(0, 30.0f, 0);
        }
    }

    void UpdateSkill()
    {
        skillText.text = "贾魔法 > " + nSkillNum.ToString();
        switch (nSkillNum)
        {
            case 3:
                {
                    imgSkill1.enabled = true;
                    imgSkill2.enabled = true;
                    imgSkill3.enabled = true;
                    break;
                }
            case 2:
                {
                    imgSkill1.enabled = true;
                    imgSkill2.enabled = true;
                    imgSkill3.enabled = false;
                    break;
                }
            case 1:
                {
                    imgSkill1.enabled = true;
                    imgSkill2.enabled = false;
                    imgSkill3.enabled = false;
                    break;
                }
            case 0:
                {
                    imgSkill1.enabled = false;
                    imgSkill2.enabled = false;
                    imgSkill3.enabled = false;
                    break;
                }
            default:
                {
                    imgSkill1.enabled = true;
                    imgSkill2.enabled = true;
                    imgSkill3.enabled = true;
                    break;
                }
        }
    }
}

