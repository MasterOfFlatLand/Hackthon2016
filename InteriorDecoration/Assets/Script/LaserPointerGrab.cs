using UnityEngine;
using System.Collections;
using VRTK;

public class LaserPointerGrab : MonoBehaviour {

    public GameObject pointerTip = null;

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
    }

    private void DebugLogger(uint index, string action, Transform target, float distance, Vector3 tipPosition)
    {
        string targetName = (target ? target.name : "<NO VALID TARGET>");
        Debug.Log("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named " + targetName + " - the pointer tip position is/was: " + tipPosition);
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (targetGO != null)
        {
            FixedJoint joint = targetGO.AddComponent<FixedJoint>();
            joint.connectedBody = pointerTip.GetComponent<Rigidbody>();

            pointer.grabbingTarget = true;
        }
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (targetGO != null)
        {
            Destroy(targetGO.GetComponent<FixedJoint>());

            pointer.grabbingTarget = false;
        }
    }

    private void DoPointerIn(object sender, DestinationMarkerEventArgs e)
    {
        //DebugLogger(e.controllerIndex, "POINTER IN", e.target, e.distance, e.destinationPosition);
        if (e.target.GetComponent<VRTK_InteractableObject>() != null)
        {
            targetGO = e.target.gameObject;
        }
    }

    private void DoPointerOut(object sender, DestinationMarkerEventArgs e)
    {
        //DebugLogger(e.controllerIndex, "POINTER OUT", e.target, e.distance, e.destinationPosition);
        targetGO = null;
    }

    private void DoPointerDestinationSet(object sender, DestinationMarkerEventArgs e)
    {
        DebugLogger(e.controllerIndex, "POINTER DESTINATION", e.target, e.distance, e.destinationPosition);
    }

}
