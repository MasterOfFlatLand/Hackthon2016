using UnityEngine;
using System.Collections;

public class FurniturePlaceHint : MonoBehaviour {
    public GameObject targetFurniture;

    private TextMesh hintText;

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
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetFurniture)
        {
            HideHint();
        }
        else
        {
            ErrorHint();
        }
    }

    void OnTriggerExit(Collider other)
    {
        ResetHint();
    }

    void ResetHint()
    {
        hintText.color = Color.yellow;
        hintText.fontStyle = FontStyle.Normal;

        hintText.gameObject.SetActive(true);
    }

    void ErrorHint()
    {
        hintText.color = Color.red;
        hintText.fontStyle = FontStyle.Bold;

        hintText.gameObject.SetActive(true);
    }

    void HideHint()
    {
        hintText.gameObject.SetActive(false);
    }
}
