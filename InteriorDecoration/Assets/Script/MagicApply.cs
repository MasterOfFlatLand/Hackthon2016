using UnityEngine;
using System.Collections;

public class MagicApply : MonoBehaviour {
    public GameObject magicRoot;
    public GameObject jiaMagic;

    private GameObject[] magicArray;
    private int magicCount;

	// Use this for initialization
	void Awake () {
        magicCount = magicRoot.transform.childCount;
        magicArray = new GameObject[magicCount];
        for (int i = 0; i < magicCount; ++i)
        {
            magicArray[i] = magicRoot.transform.GetChild(i).gameObject;
        }

        Reset();
	}

    public void UseMagic(GameObject srcFurniture)
    {
        FurnitureStylePair pair = srcFurniture.GetComponent<FurnitureStylePair>();
        if (magicCount > 0 && null != pair)
        {
            magicArray[--magicCount].SetActive(false);
            MagicReplace(srcFurniture, pair.pairFurniture);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < magicArray.Length; ++i)
        {
            magicArray[i].SetActive(true);
        }

        if (null != jiaMagic)
        {
            jiaMagic.SetActive(false);
        }

        magicCount = magicArray.Length;
    }

    void MagicReplace(GameObject srcFurniture, GameObject dstFurniture)
    {
        srcFurniture.SetActive(false);

        BoxCollider bc = srcFurniture.GetComponent<BoxCollider>();

        jiaMagic.transform.localPosition = srcFurniture.transform.localToWorldMatrix.MultiplyPoint3x4(bc.center);
        jiaMagic.SetActive(true);

        AudioSource adSrc = jiaMagic.GetComponent<AudioSource>();
        adSrc.Play();

        iTween.RotateBy(jiaMagic, iTween.Hash(
            "y", 1.0,
            "easetype", "easeInOutQuad",
            "oncomplete", "ShowTargetFurniture", 
            "oncompletetarget", gameObject, 
            "oncompleteparams", dstFurniture, 
            "time", 3.0f));
    }

    void ShowTargetFurniture(GameObject dstFurniture)
    {
        jiaMagic.SetActive(false);

        dstFurniture.transform.localPosition = jiaMagic.transform.position;
        dstFurniture.transform.localEulerAngles = Vector3.zero;
        dstFurniture.SetActive(true);
    }
}
