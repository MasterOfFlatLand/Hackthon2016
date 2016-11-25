using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ProgressTrigger : MonoBehaviour {
    [HideInInspector]
    public float progressDuration = 10;

    private float startTime;
    private float progressLength;
    private RectTransform trans;
	// Use this for initialization
	void Start () {
        startTime = Time.time;

        trans = transform as RectTransform;
        progressLength = trans.rect.width;
	}
	
	// Update is called once per frame
	void Update () {
        float deltaTime = Time.time - startTime;
        float tarWidth = (1 - deltaTime / progressDuration) * progressLength;

        if (tarWidth <= 0)
        {
            //SceneManager.LoadScene("BigBang");
        }
        else
        {
            trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tarWidth);
        }
	}
}
