using UnityEngine;
using System.Collections;
using VRTK;

public class FurnitureStyleManager : MonoBehaviour {
    public GameObject[] furnitureStyleArray;
    public VRTK_UIPointer uiPointer;

    [HideInInspector]
    public int styleIndex = -1;

	// Use this for initialization
	void Start () {
        uiPointer.UIPointerElementEnter += new UIPointerEventHandler(UITriggered);

        DontDestroyOnLoad(this.gameObject);
	}
	
    private void UITriggered(object sender, UIPointerEventArgs e)
    {
        for (int i = 0; i < furnitureStyleArray.Length; ++i)
        {
            if (e.currentTarget == furnitureStyleArray[i])
            {
                styleIndex = i;
                break;
            }
        }

        Debug.Log("furniture style index: " + styleIndex);
    }
}
