using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class InteriorDecUtility : MonoBehaviour {

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
            for (int i=0; i<go.transform.childCount; ++i)
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
            for (int i=1; i<mfArray.Length; ++i)
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

//         MeshRenderer[] rendArray = go.GetComponentsInChildren<MeshRenderer>();
//         if (null != rendArray)
//         {
//             Bounds rootBounds = rendArray[0].bounds;
//             for (int i = 1; i < rendArray.Length; ++i)
//             {
//                 rootBounds.Encapsulate(rendArray[i].bounds);
//             }
// 
//             BoxCollider bc = go.GetComponent<BoxCollider>();
//             if (null == bc)
//             {
//                 bc = go.AddComponent<BoxCollider>();
//             }
// 
//             // transform bounds into local space.
//             Vector3 minPt = go.transform.worldToLocalMatrix.MultiplyPoint(rootBounds.min);
//             Vector3 maxPt = go.transform.worldToLocalMatrix.MultiplyPoint(rootBounds.max);
// 
//             rootBounds.center = minPt;
//             rootBounds.size = Vector3.zero;
//             rootBounds.Encapsulate(maxPt);
// 
//             bc.center = rootBounds.center;
//             bc.size = rootBounds.size;
//         }
    }

    [MenuItem("VRUtility/RigidBody/Add to Children")]
    static void AddRigidBodyToChildren()
    {
        GameObject go = Selection.activeGameObject;
        if (null != go)
        {
            for (int i=0; i<go.transform.childCount; ++i)
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
            for (int i=0; i<go.transform.childCount; ++i)
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
}
