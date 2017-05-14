using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// RaycastInput handles input in worldspace for both the mouse and VR controllers. 
/// Sends messages to hit GameObjects to trigger hover/click states.</summary>
public class RaycastInput : MonoBehaviour
{
    public bool rayHit = false;
    public Vector3 hitPos;
    public Vector3 hitNorm;

    public LayerMask _interactableLayers;
    public float _rayRange = 10f;
    public Transform _rayAnchor;
    public bool __debug;

    private Ray _ray;
    private RaycastHit _hit;
    private Transform _lastHitObject;
    private bool _lastHitWasInteractive = false;

    private void Start() {
        //set default values for empty variables
        if (_rayAnchor == null)
            _rayAnchor = this.transform;
    }

    private void Update() {
        SetupRay();

        if (Physics.Raycast(_ray, out _hit, _rayRange)) {
            rayHit = true;
            hitPos = _hit.point;
            hitNorm = _hit.normal;

            SendMessages();
            if (__debug)
                Debug.DrawLine(_rayAnchor.position, _hit.point, Color.red);
        } else{
            rayHit = false;

            if (_lastHitWasInteractive && _lastHitObject != null) {
                HoverOff();
                _lastHitWasInteractive = false;
                _lastHitObject = null;
            }
        } 
    }

    private void SetupRay() {
        _ray.origin = _rayAnchor.position;
        _ray.direction = _rayAnchor.forward;
    }

    private void SendMessages() {
        //hit something interactive?
        bool isInteractive = _interactableLayers == (_interactableLayers | (1 << _hit.transform.gameObject.layer));

        //check if we hoverd off an object
        if (_hit.transform != _lastHitObject && _lastHitWasInteractive && _lastHitObject != null)
            HoverOff();

        //check if we hover on an interactive object
        if (isInteractive) {
            if (_hit.transform != _lastHitObject)
                HoverOn();

            //TODO: abstract input for mouse/other controllers, and reference it in this script
            if (Input.GetMouseButtonDown(0))
                _hit.transform.SendMessageUpwards("OnClick", this, SendMessageOptions.DontRequireReceiver);
        }

        //save hit info for next frame
        _lastHitObject = _hit.transform;
        _lastHitWasInteractive = isInteractive;
    }

    private void HoverOn() {
        _hit.transform.SendMessageUpwards("HoverOn", this, SendMessageOptions.DontRequireReceiver);
    }

    private void HoverOff() {
        _lastHitObject.transform.SendMessageUpwards("HoverOff", this, SendMessageOptions.DontRequireReceiver);
    }
}
