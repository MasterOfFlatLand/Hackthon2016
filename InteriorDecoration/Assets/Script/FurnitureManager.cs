using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using VRTK;

// attach this script to Camera obj, and assign the furniture root object;
public class FurnitureManager : MonoBehaviour {
    public GameObject furnitureRoot;
    public GameObject placeHintRoot;
    public GameObject roof;

    public AudioClip scoreSound;
    public AudioClip wrongSound;
    
    public float fallingInterval = 5;
    public float gameDuration = 60;
    public GameObject GameOverUI;
    public GameObject GameOverSound;

    public Text scoreText;

    public ProgressTrigger progTrigger;


    private bool isGameOver;

    private Vector3[] initialPosition;
    private GameObject[] furnitureArray;
    private float startFallingTm;
    private List<int> fallingIndexArray = new List<int>();
    private System.Random rand;

    private float gameOverTime;
    private IEnumerator uiFadeoffCo;

    private int totalScore = 0;

    private MagicApply magicApplier;

    public void AddScore(int score)
    {
        UpdateScore(totalScore + score);
    }

    public void SubtractScore(int score)
    {

        UpdateScore(Mathf.Max(0, totalScore - score));
    }

    void UpdateScore(int score)
    {
        totalScore = score;
        if (null != scoreText)
        {
            scoreText.text = "得分：" + totalScore;
        }
    }

	// Use this for initialization
	void Start () {
        int childCnt = furnitureRoot.transform.childCount;
        furnitureArray = new GameObject[childCnt];
        initialPosition = new Vector3[childCnt];
        for (int i=0; i<childCnt; ++i)
        {
            Transform childFurniture = furnitureRoot.transform.GetChild(i);
            initialPosition[i] = childFurniture.position;
            furnitureArray[i] = childFurniture.gameObject;
        }

        for (int i = 0; i < placeHintRoot.transform.childCount; ++i)
        {
            Transform child = placeHintRoot.transform.GetChild(i);
            FurniturePlaceHint hint = child.GetComponent<FurniturePlaceHint>();
            hint.furnitureMgr = this;
        }

        magicApplier = this.GetComponent<MagicApply>();

        RestartGame();

        VRTK_ControllerEvents[] events = gameObject.GetComponentsInChildren<VRTK_ControllerEvents>(true);
        if (null != events)
        {
            foreach (var ev in events)
            {
                ev.ApplicationMenuPressed += new ControllerInteractionEventHandler(RestartGameTriggered);
            }
        }
        else
        {
            Debug.LogError("VRTK_ControllerEvents scripts not found.");
        }
    }
	
	// Update is called once per frame
	void Update () {
        // trigger roof on complete.
        if (fallingIndexArray.Count == 0 && (Time.time - startFallingTm >= fallingInterval) &&
            roof.GetComponent<VRTK_InteractableObject>() == null)
        {
            roof.AddComponent<VRTK_InteractableObject>();
            roof.GetComponent<BoxCollider>().enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        else if (fallingIndexArray.Count > 0 && (Time.time - startFallingTm >= fallingInterval))
        {
            int idx = rand.Next(fallingIndexArray.Count);

            GameObject tarGo = furnitureArray[fallingIndexArray[idx]];
            Rigidbody rb = tarGo.GetComponent<Rigidbody>();
            if (null != rb)
            {
                rb.isKinematic = false;
            }
            tarGo.SetActive(true);

            fallingIndexArray.RemoveAt(idx);

            startFallingTm = Time.time;
        }
        else if (Time.time > gameOverTime && !isGameOver)
        {
            GameOver();
        }
	}

    public void UseMagic(float magicLength)
    {
        gameOverTime += magicLength;
    }

    private void RestartGameTriggered(object sender, ControllerInteractionEventArgs e)
    {
        RestartGame();
    }

    void GameOver()
    {
        if (null != GameOverUI)
        {
            GameObject scoreTextGo = GameOverUI.transform.GetChild(2).gameObject;
            Text scoreText = scoreTextGo.GetComponent<Text>();
            scoreText.text = "您的分数是：<b>" + totalScore + "</b> !";

            GameOverUI.SetActive(true);
            uiFadeoffCo = UIFadeoff(GameOverUI, 5);
            StartCoroutine(uiFadeoffCo);
        }

        if (null != GameOverSound)
        {
            AudioSource sound = GameOverSound.GetComponent<AudioSource>();
            sound.Play();
        }

        // disable laser pointer.
        VRTK_SimplePointer[] pointers = gameObject.GetComponentsInChildren<VRTK_SimplePointer>();
        foreach (VRTK_SimplePointer pointer in pointers)
        {
            pointer.enabled = false;
        }

        LaserPointerGrab[] pointGrabs = gameObject.GetComponentsInChildren<LaserPointerGrab>();
        foreach (LaserPointerGrab grab in pointGrabs)
        {
            grab.enabled = true;
        }

        isGameOver = true;
    }

    IEnumerator UIFadeoff(GameObject uiObj, float elapsedSec)
    {
        yield return new WaitForSeconds(elapsedSec);
        uiObj.SetActive(false);
    }

    void RestartGame()
    {
        Debug.Log("restarting game...");

        fallingIndexArray.Clear();

        for (int i = 0; i < initialPosition.Length; ++i)
        {
            GameObject go = furnitureArray[i];
            Rigidbody rb = go.GetComponent<Rigidbody>();
            if (null != rb)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
            go.transform.position = initialPosition[i];

            // reset furniture pair.
            FurnitureStylePair pair = go.GetComponent<FurnitureStylePair>();
            if (null != pair)
            {
                pair.Reset();
            }

            go.SetActive(false);

            fallingIndexArray.Add(i);
        }

        startFallingTm = Time.time - fallingInterval;
        gameOverTime = startFallingTm + gameDuration;

        // create new random generator.
        rand = new System.Random(Guid.NewGuid().GetHashCode());

        // reset score;
        UpdateScore(0);

        // enable laser pointers.
        VRTK_SimplePointer[] pointers = gameObject.GetComponentsInChildren<VRTK_SimplePointer>();
        foreach (VRTK_SimplePointer pointer in pointers)
        {
            pointer.enabled = true;
        }

        LaserPointerGrab[] pointGrabs = gameObject.GetComponentsInChildren<LaserPointerGrab>();
        foreach (LaserPointerGrab grab in pointGrabs)
        {
            grab.enabled = true;
        }

        // reset hint.
        FurniturePlaceHint[] hintArray = placeHintRoot.GetComponentsInChildren<FurniturePlaceHint>();
        foreach (FurniturePlaceHint hint in hintArray)
        {
            hint.ResetHint();
        }

        // reset photo rendering manager.
        PhotoRenderingManager photoRndMgr = gameObject.GetComponent<PhotoRenderingManager>();
        photoRndMgr.Reset();

        // clear roof resources;
        Destroy(roof.GetComponent<VRTK_InteractableObject>());
        Destroy(roof.GetComponent<Rigidbody>());
        roof.GetComponent<BoxCollider>().enabled = false;

        // reset magic.
        if (null != magicApplier)
        {
            magicApplier.Reset();
        }

        // reset progress bar.
        progTrigger.progressDuration = gameDuration;
        progTrigger.Reset();

        if (null != uiFadeoffCo)
        {
            StopCoroutine(uiFadeoffCo);
            uiFadeoffCo = null;
        }

        if (null != GameOverUI)
        {
            GameOverUI.SetActive(false);
        }
        isGameOver = false;
    }
}
