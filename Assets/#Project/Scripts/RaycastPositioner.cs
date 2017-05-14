using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPositioner : MonoBehaviour {

    public Transform _playerHead;
    public Transform _leftHand;
    public Transform _rightHand;
    public GameObject _line;

    void Start() {
        if(OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote) || OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote)) {
            ParentToController();
            _line.SetActive(true);
        } else {
            ParentToHead();
            _line.SetActive(false);
        }

        ParentToHead();
        _line.SetActive(false);
    }

    private void ParentToController() {
        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote)) {
            transform.parent = _rightHand;
        } else {
            transform.parent = _leftHand;
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void ParentToHead() {
        transform.parent = _playerHead;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
