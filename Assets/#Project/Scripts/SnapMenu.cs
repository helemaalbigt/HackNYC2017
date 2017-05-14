using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapMenu : MonoBehaviour {

    public GameObject _buttonPrefab;
    public Transform _buttonParent;
    public VRInput _input;
    public SnapScreen _screen;
    public Animator _animator;
    public GameObject _leftArrow;
    public GameObject _rightArrow;
    public GameObject _instructions;

    private Dictionary<float, SnapButton> buttons = new Dictionary<float, SnapButton>();
    private int _currentIndex = 0;
    private float _targetAngle;

	// Use this for initialization
	void Start () {
        AddUIButton(); AddUIButton(); AddUIButton(); AddUIButton();

        SubscribeToInput();
    }

    private void Update() {
        //Debug.Log(360 - _buttonParent.transform.localEulerAngles.y + "-" + _targetAngle);
        float angle = Mathf.Lerp((360 - _buttonParent.transform.localEulerAngles.y) % 360, _targetAngle, Time.deltaTime * 5);
        _buttonParent.transform.localEulerAngles = new Vector3(0, -angle, 0);
    }

    public void Next() {
        if(_currentIndex + 1 < buttons.Count) {
            _currentIndex++;
            _targetAngle = _currentIndex * 15;
        }
        SetArrows();
    }

    public void Prev() {
        if (_currentIndex  > 0) {
            _currentIndex--;
            _targetAngle = _currentIndex * 15;
        }
        SetArrows();
    }

    public void HideStartScreen() {
        Debug.Log("Hide Loading");
        _animator.Play("Loaded");
    }

    private void SetArrows() {
        if(_currentIndex == 0) {
            _rightArrow.SetActive(true);
            _leftArrow.SetActive(false);
        } else if(_currentIndex + 1 == buttons.Count) {
            _rightArrow.SetActive(false);
            _leftArrow.SetActive(true);
        } else {
            _rightArrow.SetActive(true);
            _leftArrow.SetActive(true);
        }
    }

    private void HideInstructions() {
        _instructions.SetActive(false);
    }

    private void SubscribeToInput() {
        _input.OnSwipeRight += Prev;
        _input.OnSwipeLeft += HideInstructions;
        _input.OnSwipeLeft += Next;
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
