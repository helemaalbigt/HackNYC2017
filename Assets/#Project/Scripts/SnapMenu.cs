using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SnapMenu : MonoBehaviour {

    public GameObject _buttonPrefab;
    public Transform _buttonParent;
	private string filePath;
	private string filePrefix = "file://";

    private Dictionary<float, SnapButton> buttons = new Dictionary<float, SnapButton>();

	// Use this for initialization
	void Start () {
		if (Application.isEditor) {
			filePath = "./" + Application.dataPath + "/Resources/";           
		} else {
			filePath = "/storage/emulated/0/Snapchat/";
		}

		DirectoryInfo info = new DirectoryInfo(filePath);

		FileInfo[] fileInfo = info.GetFiles();
		foreach (FileInfo file in fileInfo) {
			if (file.ToString ().Contains ("thumbnail"))
				continue;
			else {
				string fullPath = filePrefix + filePath + file.ToString ();
				string thumbnailPath = "thumbnail_" + Path.GetFileNameWithoutExtension (file.ToString()) + ".png";
				string fullThumbnailPath = filePrefix + filePath + thumbnailPath;

				//Debug.Log ("Adding new memory...");
				//Debug.Log ("file path " + fullPath);
				//Debug.Log ("fullThumbnailPath " + fullThumbnailPath);
				StartCoroutine(AddUIButton (Path.GetFileNameWithoutExtension (fullPath), fullThumbnailPath));
			}
		}

    }

    private void AddUIButton() {
        AddUIButton( "./Resources/video.mp4", "");
    }

	private IEnumerator AddUIButton(string fileLocation, string thumbnailLocation) {
        GameObject buttonGo = Instantiate(_buttonPrefab, _buttonParent) as GameObject;
        buttonGo.transform.localPosition = Vector3.zero;
        buttonGo.transform.localRotation = Quaternion.identity;

        float angleID = buttons.Count * 15f;
        buttonGo.transform.localEulerAngles = new Vector3(0, angleID, 0);

        SnapButton button = buttonGo.GetComponent<SnapButton>();
		Debug.Log ("adding button #" + buttons.Count);
		buttons.Add(angleID, button);

		Texture2D tex;
		tex = new Texture2D(4, 4);
		Debug.Log ("loading thumbnail from " + thumbnailLocation + "...");
		WWW www = new WWW(thumbnailLocation);
		yield return www;
		www.LoadImageIntoTexture(tex);
		buttonGo.GetComponentInChildren<Renderer> ().material.mainTexture = tex;

    }
}
