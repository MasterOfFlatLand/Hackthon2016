using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using VRTK;

// attach this script to Camera obj, and assign the furniture root object;
public class FurnitureManager : MonoBehaviour {
    public GameObject furnitureRoot;
    public float fallingInterval = 5;

    private Vector3[] initialPosition;
    private float startFallingTm;
    private List<int> fallingIndexArray = new List<int>();
    private System.Random rand;

	// Use this for initialization
	void Start () {
        initialPosition = new Vector3[furnitureRoot.transform.childCount];
        for (int i=0; i<initialPosition.Length; ++i)
        {
            initialPosition[i] = furnitureRoot.transform.GetChild(i).position;
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
        if (fallingIndexArray.Count > 0 && (Time.time - startFallingTm >= fallingInterval))
        {
            int idx = rand.Next(fallingIndexArray.Count);

            furnitureRoot.transform.GetChild(fallingIndexArray[idx]).gameObject.SetActive(true);
            fallingIndexArray.RemoveAt(idx);

            startFallingTm = Time.time;
        }
	}

    private void RestartGameTriggered(object sender, ControllerInteractionEventArgs e)
    {
        RestartGame();
        Debug.Log("restart game.");
    }


    void RestartGame()
    {
        fallingIndexArray.Clear();

        for (int i = 0; i < initialPosition.Length; ++i)
        {
            GameObject go = furnitureRoot.transform.GetChild(i).gameObject;
            Rigidbody rb = go.GetComponent<Rigidbody>();
            if (null != rb)
            {
                rb.position = initialPosition[i];
            }
            go.SetActive(false);

            fallingIndexArray.Add(i);
        }

        startFallingTm = Time.time - fallingInterval;

        // create new random generator.
        rand = new System.Random(Guid.NewGuid().GetHashCode());
    }
}
