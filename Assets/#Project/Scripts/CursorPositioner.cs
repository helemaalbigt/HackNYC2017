using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPositioner : MonoBehaviour {

    public bool _followNormal = false;
    public bool _constantScale = false;

    public RaycastInput _raycast;
    public Transform _cursor;
    public Transform _playerEyes;
	
	// Update is called once per frame
	void Update () {
        if (_raycast.rayHit) {
            _cursor.gameObject.SetActive(true);
            _cursor.position = _raycast.hitPos;
            _cursor.LookAt(_playerEyes);
        } else {
            _cursor.gameObject.SetActive(false);
        }
	}
}
