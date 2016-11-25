using UnityEngine;
using System.Collections;

public class FurniturePlaceHint : MonoBehaviour {
    public GameObject targetFurniture;
    public int matchScore = 5;

    private bool matched = false;

    private TextMesh hintText;
    private AudioSource audioSrc;

    [HideInInspector]
    public FurnitureManager furnitureMgr;

	// Use this for initialization
	void Start () {
	    if (null == targetFurniture)
        {
            Debug.LogError("target furniture is not assigned to " + this.name);
            Destroy(this);
        }
        else
        {
            BoxCollider bc = gameObject.GetComponent<BoxCollider>();
            if (null == bc)
            {
                bc = gameObject.AddComponent<BoxCollider>();
            }

            bc.isTrigger = true;

            GameObject childText = gameObject.transform.GetChild(0).gameObject;
            hintText = childText.GetComponent<TextMesh>();

            // reset hint text.
            ResetHint();
        }

        // get audio source.
        audioSrc = this.GetComponent<AudioSource>();
        if (null == audioSrc)
        {
            audioSrc = gameObject.AddComponent<AudioSource>();
        }
    }
	
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetFurniture)
        {
            HideHint();
            PlaySound(furnitureMgr.scoreSound);

            furnitureMgr.AddScore(matchScore);

            matched = true;
        }
        else if (!matched)
        {
            ErrorHint();
            PlaySound(furnitureMgr.wrongSound);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetFurniture)
        {
            ResetHint();
        }
    }

    void PlaySound(AudioClip clip)
    {
        audioSrc.PlayOneShot(clip);
    }

    public void ResetHint()
    {
        if (null != hintText)
        {
            hintText.color = Color.yellow;
            hintText.fontStyle = FontStyle.Normal;

            hintText.gameObject.SetActive(true);
        }
    }

    void ErrorHint()
    {
        if (null != hintText)
        {
            hintText.color = Color.red;
            hintText.fontStyle = FontStyle.Bold;

            hintText.gameObject.SetActive(true);
        }
    }

    void HideHint()
    {
        if (null != hintText)
        {
            hintText.gameObject.SetActive(false);
        }
    }
}
