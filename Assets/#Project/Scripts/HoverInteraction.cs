using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoverInteraction : MonoBehaviour {

    public UnityEvent OnHoverOn;
    public UnityEvent OnHoverOff;

	public void HoverOn() {
        Debug.Log("hovered on");
        OnHoverOn.Invoke();
    }

    public void HoverOff() {
        Debug.Log("hovered off");
        OnHoverOff.Invoke();
    }
}
