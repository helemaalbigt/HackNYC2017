using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidIntentReceiver : MonoBehaviour {

	void Awake(){
		Debug.Log ("AWAKE");
		string arguments = "";

		AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

		//catch the EXTRA_STREAM being sent by snapchat
		string intentKey = "android.intent.extra.STREAM";

		bool hasExtra = intent.Call<bool> ("hasExtra", intentKey);

		Debug.Log ("looking for " + intentKey);
		if (hasExtra) {
			Debug.Log ("found " + intentKey);
			AndroidJavaObject extras = intent.Call<AndroidJavaObject> ("getExtras");
			AndroidJavaObject URI = extras.Call<AndroidJavaObject> ("get", intentKey);
			string URIstring = URI.Call<string> ("toString");
			Debug.Log ("URI " + URIstring);
		}


	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
