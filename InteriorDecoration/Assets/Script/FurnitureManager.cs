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

    public AudioClip scoreSound;
    public AudioClip wrongSound;
    
    public float fallingInterval = 5;
    public float gameLength = 60;
    public GameObject GameOverUI;
    public GameObject GameOverSound;

    private bool isGameOver;

    private Vector3[] initialPosition;
    private float startFallingTm;
    private List<int> fallingIndexArray = new List<int>();
    private System.Random rand;

    private float gameOverTime;
    private IEnumerator uiFadeoffCo;

    private int totalScore = 0;

    public void AddScore(int score)
    {
        totalScore += score;
    }

	// Use this for initialization
	void Start () {
        initialPosition = new Vector3[furnitureRoot.transform.childCount];
        for (int i=0; i<initialPosition.Length; ++i)
        {
            Transform childFurniture = furnitureRoot.transform.GetChild(i);
            initialPosition[i] = childFurniture.position;
        }

        for (int i = 0; i < placeHintRoot.transform.childCount; ++i)
        {
            Transform child = placeHintRoot.transform.GetChild(i);
            FurniturePlaceHint hint = child.GetComponent<FurniturePlaceHint>();
            hint.furnitureMgr = this;
        }

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        else if (fallingIndexArray.Count > 0 && (Time.time - startFallingTm >= fallingInterval))
        {
            int idx = rand.Next(fallingIndexArray.Count);
            //int idx = 0;

            GameObject tarGo = furnitureRoot.transform.GetChild(fallingIndexArray[idx]).gameObject;
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
            uiFadeoffCo = UIFadeoff(GameOverUI, 2);
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
            GameObject go = furnitureRoot.transform.GetChild(i).gameObject;
            Rigidbody rb = go.GetComponent<Rigidbody>();
            if (null != rb)
            {
                rb.isKinematic = true;
            }
            go.transform.position = initialPosition[i];
            go.SetActive(false);

            fallingIndexArray.Add(i);
        }

        startFallingTm = Time.time - fallingInterval;
        gameOverTime = startFallingTm + gameLength;

        // create new random generator.
        rand = new System.Random(Guid.NewGuid().GetHashCode());

        // reset score;
        totalScore = 0;

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
