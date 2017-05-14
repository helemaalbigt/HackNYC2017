using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapMenu : MonoBehaviour {

    public GameObject _buttonPrefab;
    public Transform _buttonParent;

    private Dictionary<float, SnapButton> buttons = new Dictionary<float, SnapButton>();

	// Use this for initialization
	void Start () {
        AddUIButton(); AddUIButton(); AddUIButton(); AddUIButton();

    }

    private void AddUIButton() {
        AddUIButton("TestName", "./Resources/video.mp4");
    }

    private void AddUIButton(string name, string fileLocation) {
        GameObject buttonGo = Instantiate(_buttonPrefab, _buttonParent) as GameObject;
        buttonGo.transform.localPosition = Vector3.zero;
        buttonGo.transform.localRotation = Quaternion.identity;

        float angleID = buttons.Count * 15f;
        buttonGo.transform.localEulerAngles = new Vector3(0, angleID, 0);

        SnapButton button = buttonGo.GetComponent<SnapButton>();
        buttons.Add(angleID, button);
    }
}
