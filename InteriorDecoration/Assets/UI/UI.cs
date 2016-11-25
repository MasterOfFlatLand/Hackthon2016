using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {
    //public int level;
    public int nBlood;
    public int nFurFixedNum;
    public int nFurTotalNum;
    public Texture2D imgBlood; 
    public Texture2D imgBackGround;
    public Texture2D imgSkill;
    public GameObject gameobject;
    public bool bIsSkillTriggered;
    public int nSkillNum;

    private int nBloodWidth = Screen.width / 5;
    private int nTextHeight = Screen.height / 25;
    private int nTextWidth = Screen.width / 25;
    private int nTextY = Screen.height / 50;
    private int nAngle = 10;
	void Start () 
    {
        InitGameObject();
        StartCoroutine(BloodTimer());
        StartCoroutine(GameObjectRotateTimer());  
        bIsSkillTriggered = false;
        nSkillNum = 5;
	}

    IEnumerator BloodTimer()
    {
        while (nBlood >= 0)
        {  
            yield return new WaitForSeconds(1.0f);   
            if (!bIsSkillTriggered)     
            {    
                nBloodWidth -= nBloodWidth / nBlood;                  
                nBlood--;     
            }
        }
    }

    IEnumerator GameObjectRotateTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            nAngle += 5;
            gameobject.transform.localRotation = Quaternion.Euler(nAngle, nAngle, nAngle);
        }
    }
    void OnGUI()
    {
        DrawLevelLabel();
        DrawNextLabel();
        DrawSkillLabel();
    }

    void DrawLevelLabel()
    {
        int nBloodX = Screen.width / 50;
        int nBloodY = Screen.height * 2 / 25;
        int nBloodHeight = Screen.height / 50;
        int nLevelX = Screen.width / 50;
        int nScoreX = Screen.width / 50;
        int nBackWidth = Screen.width / 5;
        GUI.DrawTexture(new Rect(nBloodX, nBloodY, nBackWidth, nBloodHeight), imgBackGround);
        GUI.DrawTexture(new Rect(nBloodX, nBloodY, nBloodWidth, nBloodHeight), imgBlood);
        GUIStyle style = new GUIStyle();
        style.normal.background = null;
        style.normal.textColor = new Color(1, 1, 1);
        style.fontSize = 13;
        //GUI.Label(new Rect(nLevelX, nTextY, nTextWidth, nTextHeight), "LEVEL " + level.ToString(), style);
        GUI.Label(new Rect(nScoreX, nTextY, nTextWidth, nTextHeight), "SCORE " + nFurFixedNum.ToString() + " / " + nFurTotalNum.ToString(), style);
    }
    void DrawNextLabel()
    {
        GUIStyle style = new GUIStyle();
        style.normal.background = null;
        style.normal.textColor = new Color(1, 1, 1);
        style.fontSize = 13;
        int nNextX = Screen.width * 10 / 21;
        GUI.Label(new Rect(nNextX, nTextY, nTextWidth, nTextHeight), "NEXT", style);
    }
    void DrawSkillLabel()
    {
        GUIStyle style = new GUIStyle();
        style.normal.background = null;
        style.normal.textColor = new Color(1, 1, 1);
        style.fontSize = 13;
        int nSkillX = Screen.width - nTextWidth * 3;
        GUI.Label(new Rect(nSkillX, nTextY, nTextWidth, nTextHeight), "SKILL " + nSkillNum.ToString(), style);
        int nSkillWidth = nTextWidth * 2 / 5;
        int nSkillHeight = Screen.height / 25;
        int nSkillY = Screen.height * 2 / 25;
        for (int nIndex = 0; nIndex < nSkillNum; nIndex++)
        {
            int nX = nSkillX + nIndex * nSkillWidth;
            GUI.DrawTexture(new Rect(nX, nSkillY, nSkillWidth, nSkillHeight), imgSkill);
        }
    }
    void InitGameObject()
    {
        float fX = 1000f;
        float fY = 0.8f;
        Vector3 loc = new Vector3(fX, fY, 5);
        gameobject.transform.position = loc;
        gameobject.layer = 6;
        Vector3 objectSize = Vector3.Scale(gameobject.transform.localScale, gameobject.GetComponent<Renderer>().bounds.size);
        float nMax = Mathf.Max(Mathf.Max(objectSize.x, objectSize.y), objectSize.z);
        //float fScale = (Screen.height / 360000.0f) / nMax;
        float fScale = 0.1f;
        Vector3 scale = new Vector3(fScale, fScale, fScale);
        gameobject.transform.localScale = scale;
    }
}
