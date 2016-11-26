using UnityEngine;
using System.Collections;
using VRTK;

[RequireComponent(typeof(VRTK_InteractTouch)), RequireComponent(typeof(VRTK_ControllerEvents)), RequireComponent(typeof(VRTK_SimplePointer))]
public class LaserPointerGrab : MonoBehaviour {
    public Transform vrCamera;
    public MagicApply magicApplier;
    
    private GameObject targetGO = null;

    VRTK_SimplePointer pointer;

    private void Start()
    {
        pointer = GetComponent<VRTK_SimplePointer>();
        if (pointer == null)
        {
            Debug.LogError("VRTK_ControllerPointerEvents_ListenerExample is required to be attached to a Controller that has the VRTK_SimplePointer script attached to it");
            return;
        }

        //Setup controller event listeners
        pointer.DestinationMarkerEnter += new DestinationMarkerEventHandler(DoPointerIn);
        pointer.DestinationMarkerExit += new DestinationMarkerEventHandler(DoPointerOut);
        pointer.DestinationMarkerSet += new DestinationMarkerEventHandler(DoPointerDestinationSet);

        var events = GetComponent<VRTK_ControllerEvents>();
        events.TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        events.TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

        events.TouchpadPressed += new ControllerInteractionEventHandler(TryUseMagic);

        //magicApplier = this.GetComponent<MagicApply>();
    }

    private void DebugLogger(uint index, string action, Transform target, float distance, Vector3 tipPosition)
    {
        string targetName = (target ? target.name : "<NO VALID TARGET>");
        Debug.Log("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named " + targetName + " - the pointer tip position is/was: " + tipPosition);
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (targetGO != null && pointer.pointerTip)
        {
            // disable collision detection for picked object.
            Rigidbody rb = targetGO.GetComponent<Rigidbody>();
            rb.detectCollisions = false;

            // lookAt laser.
            RotateToCamera(targetGO.transform);

            FixedJoint joint = targetGO.AddComponent<FixedJoint>();
            joint.connectedBody = pointer.pointerTip.GetComponent<Rigidbody>();

            pointer.grabbingTarget = true;
        }
    }

    private void TryUseMagic(object sender, ControllerInteractionEventArgs e)
    {
        // test if tar is imagepair.
        if (null != targetGO)
        {
            BasicPair pair = targetGO.GetComponent<BasicPair>();
            if (null != pair)
            {
                pair.Replace();
            }
            else if (magicApplier)
            {
                magicApplier.UseMagic(targetGO);
            }
        }
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (targetGO != null)
        {
            Destroy(targetGO.GetComponent<FixedJoint>());

            // add collision detection.
            Rigidbody rb = targetGO.GetComponent<Rigidbody>();
            rb.detectCollisions = true;

            pointer.grabbingTarget = false;
        }
    }

    private void DoPointerIn(object sender, DestinationMarkerEventArgs e)
    {
        if (e.target.GetComponent<VRTK_InteractableObject>() != null)
        {
            targetGO = e.target.gameObject;
        }
    }

    private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
    {
        targetGO = null;
    }

    private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)
    {
        DebugLogger(e.controllerIndex, "POINTER DESTINATION", e.target, e.distance, e.destinationPosition);
    }

    void ApplyRotate(Transform tarTrans, Vector3 from, Vector3 to, Vector3 pivot)
    {
        //Debug.Log("acos(-1): " + Mathf.Acos(-1));
        float dotVal = Mathf.Clamp(Vector3.Dot(from, to), -1, 1);
        float angle = Mathf.Acos(dotVal) * Mathf.Rad2Deg;
        if (angle > Mathf.Epsilon)
        {
            Vector3 rotAxis = Vector3.forward;
            if (angle < 179.99f)
            {
                rotAxis = Vector3.Cross(from, to);
            }
            tarTrans.RotateAround(pivot, rotAxis, angle);
        }
    }

    private void RotateToCamera(Transform tarTrans)
    {
        BoxCollider bc = tarTrans.GetComponent<BoxCollider>();
        if (null == bc)
        {
            return;
        }

        Vector3 rotCenter = tarTrans.localToWorldMatrix.MultiplyPoint3x4(bc.center);

        // rotate up.
        ApplyRotate(tarTrans, tarTrans.up, Vector3.up, rotCenter);

        // calc new rotate center.
        rotCenter = tarTrans.localToWorldMatrix.MultiplyPoint3x4(bc.center);
        Vector3 tarForward = vrCamera.position - rotCenter;
        // target on XoZ plane.
        tarForward.y = 0;
        tarForward.Normalize();

        // rotate to target.
        ApplyRotate(tarTrans, tarTrans.forward, tarForward, rotCenter);
    }
}
