using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class InteriorDecUtility : MonoBehaviour
{

    [MenuItem("VRUtility/Collider/Add BoxCollider")]
    static void AddBoxCollider()
    {
        GameObject go = Selection.activeGameObject;
        if (null != go)
        {
            AddBoxColliderToRootGO(go);
        }
    }

    [MenuItem("VRUtility/Collider/Add BC to Children")]
    static void AddBoxColliderToChildren()
    {
        GameObject go = Selection.activeGameObject;
        if (null != go)
        {
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                Transform child = go.transform.GetChild(i);
                AddBoxColliderToRootGO(child.gameObject);
            }
        }
    }

    static void AddBoxColliderToRootGO(GameObject go)
    {
        MeshFilter[] mfArray = go.GetComponentsInChildren<MeshFilter>();
        if (null != mfArray)
        {
            Bounds bd = mfArray[0].sharedMesh.bounds;
            for (int i = 1; i < mfArray.Length; ++i)
            {
                bd.Encapsulate(mfArray[i].sharedMesh.bounds);
            }

            BoxCollider bc = go.GetComponent<BoxCollider>();
            if (null == bc)
            {
                bc = go.AddComponent<BoxCollider>();
            }

            bc.center = bd.center;
            bc.size = bd.size;
        }
    }

    [MenuItem("VRUtility/RigidBody/Add to Children")]
    static void AddRigidBodyToChildren()
    {
        GameObject go = Selection.activeGameObject;
        if (null != go)
        {
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                Transform child = go.transform.GetChild(i);
                Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
                if (null == rb)
                {
                    rb = child.gameObject.AddComponent<Rigidbody>();
                }
            }
        }
    }

    [MenuItem("VRUtility/Collider/Add MeshCollider")]
    static void AddMeshCollider()
    {
        GameObject go = Selection.activeGameObject;
        if (null != go)
        {

        }
    }

    [MenuItem("VRUtility/Interact/Add InterObj Script")]
    static void AddInterObjToRootGO()
    {
        GameObject go = Selection.activeGameObject;
        if (null != go)
        {
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                GameObject childGo = go.transform.GetChild(i).gameObject;
                VRTK.VRTK_InteractableObject interObj = childGo.GetComponent<VRTK.VRTK_InteractableObject>();
                if (interObj == null)
                {
                    interObj = childGo.AddComponent<VRTK.VRTK_InteractableObject>();
                }

                interObj.isGrabbable = true;
                interObj.precisionSnap = true;
            }
        }
    }

    [MenuItem("VRUtility/Interact/Add PlaceHint Script")]
    static void AddPlaceHintToRootGO()
    {
        GameObject go = Selection.activeGameObject;
        if (null != go)
        {
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                GameObject childGo = go.transform.GetChild(i).gameObject;

                FurniturePlaceHint textHint = childGo.GetComponent<FurniturePlaceHint>();
                if (null == textHint)
                {
                    textHint = childGo.AddComponent<FurniturePlaceHint>();
                }

                string tarName = childGo.name.Substring(1);
                GameObject tarFurniture = GameObject.Find(tarName);
                if (null != tarFurniture)
                {
                    textHint.targetFurniture = tarFurniture;
                }
                else
                {
                    Debug.LogError("target furniture not found: " + tarName);
                }
            }
        }
    }

    [MenuItem("VRUtility/Sound/Add Coin Sound to Root")]
    static void AddCoinSound2Root()
    {
        GameObject go = Selection.activeGameObject;
        if (null != go)
        {
            string clipPath = "Assets/Sound/Coin.wav";
            AudioClip coin = AssetDatabase.LoadAssetAtPath<AudioClip>(clipPath);
            if (null == coin)
            {
                Debug.LogError("clip at path not found: " + clipPath);
                return;
            }

            for (int i = 0; i < go.transform.childCount; ++i)
            {
                GameObject child = go.transform.GetChild(i).gameObject;
                AudioSource src = child.GetComponent<AudioSource>();
                if (null == src)
                {
                    src = child.AddComponent<AudioSource>();
                }

                src.clip = coin;
                src.playOnAwake = false;
            }
        }
    }
}
