using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// attach this script to Camera obj, and assign the furniture root object;
public class FurnitureManager : MonoBehaviour {
    public GameObject furnitureRoot;
    public KeyCode restartButton = KeyCode.R;
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
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(restartButton))
        {
            RestartGame();
        }
        else if (fallingIndexArray.Count > 0 && (Time.time - startFallingTm >= fallingInterval))
        {
            int idx = rand.Next(fallingIndexArray.Count);

            furnitureRoot.transform.GetChild(fallingIndexArray[idx]).gameObject.SetActive(true);
            fallingIndexArray.RemoveAt(idx);

            startFallingTm = Time.time;
        }
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
