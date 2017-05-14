using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidIntentReceiver : MonoBehaviour {

	void Awake(){
		Debug.Log ("AWAKE");

		string arguments = "";

		AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

		AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

		//catch the EXTRA_STREAM being sent by snapchat
		string intentKey = "android.intent.extra.STREAM";

		bool hasExtra = intent.Call<bool> ("hasExtra", intentKey);

		Debug.Log ("looking for " + intentKey);
		if (hasExtra) {
			Debug.Log ("found " + intentKey);

			//Get Content Stream URI
			AndroidJavaObject extras = intent.Call<AndroidJavaObject> ("getExtras");
			AndroidJavaObject URI = extras.Call<AndroidJavaObject> ("get", intentKey);

			string URIstring = URI.Call<string> ("toString");
			Debug.Log ("URI STRING " + URIstring);

			//Resolve Content URI to File URI
			AndroidJavaObject contentResolver = context.Call<AndroidJavaObject>("getContentResolver");
			AndroidJavaObject cursor = contentResolver.Call<AndroidJavaObject> ("query", URI, null, null, null, null);
			cursor.Call<bool>("moveToFirst");

			string filePath = cursor.Call<string> ("getString", 0);
			cursor.Call ("close");

			Debug.Log ("file path " + filePath);

			//Save file provider info to file

			// mInputPFD = getContentResolver().openFileDescriptor(mVideoUri, "r");
			AndroidJavaObject inputFileDesc = contentResolver.Call<AndroidJavaObject> ("openFileDescriptor", URI, "r"); 

			// FileDescriptor fd = mInputPFD.getFileDescriptor();
			AndroidJavaObject fd = inputFileDesc.Call<AndroidJavaObject>("getFileDescriptor");

			//InputStream inputStream = new FileInputStream(fd);
			AndroidJavaObject inputStream = new AndroidJavaObject("java.io.FileInputStream", fd);

			//File targetFile = new File(Environment.getExternalStorageDirectory(), fileName);
			AndroidJavaObject env = new AndroidJavaObject("android.os.Environment");
			AndroidJavaObject externalStorageDir = env.CallStatic<AndroidJavaObject>("getExternalStorageDirectory");

			AndroidJavaObject targetFile = new AndroidJavaObject ("java.io.File", externalStorageDir, filePath);

			//FileOutputStream fileOutputStream = new FileOutputStream(targetFile);
			AndroidJavaObject fileOutputStream = new AndroidJavaObject("java.io.FileOutputStream", targetFile);

			int availableBytes = inputStream.Call<int>("available");
			byte[] csBuffer = new byte[availableBytes];

			AndroidJavaObject buffer = javaByteArrayFromCS (csBuffer);
			Debug.Log ("creating byte array of size " + availableBytes);

			//byte[] buffer = new byte[availableBytes];

			//System.IntPtr byteArrayPtr = AndroidJNIHelper.ConvertToJNIArray (csBuffer);
			//jvalue[] buffer = new jvalue[1];
			//buffer [0].l = byteArrayPtr;

			AndroidJavaClass arrayClass  = new AndroidJavaClass("java.lang.reflect.Array");
			//AndroidJavaObject buffer = arrayClass.CallStatic<AndroidJavaObject>("newInstance", new AndroidJavaClass("java.lang.Byte"), availableBytes);

			//initialize to all 0s
			/*for (int i=0; i< availableBytes; ++i) {
				arrayClass.CallStatic("set", arrayObject, i, new AndroidJavaObject("java.lang.Byte", 0));
			}*/



			int arrayLength = arrayClass.CallStatic<int>("getLength", buffer);

			Debug.Log("array length after creation " + arrayLength);

			//inputStream.read(buffer);
			int bytesRead = inputStream.Call<int>("read", buffer);

			Debug.Log ("read " + bytesRead + " bytes");

			//fileOutputStream.write(buffer);
			fileOutputStream.Call("write", buffer);

			//close streams
			inputStream.Call("close");
			fileOutputStream.Call ("close");
			inputFileDesc.Call ("close");

			//save buffer to mediaprovider

			//set values
			string fileAbsolutePath = targetFile.Call<string>("getAbsolutePath");

			//ContentValues values = new ContentValues(3);
			AndroidJavaObject values = new AndroidJavaObject ("android.content.ContentValues", 3);

			/*values.put(MediaStore.Video.Media.TITLE, "My video title");
			values.put(MediaStore.Video.Media.MIME_TYPE, "video/mp4");
			values.put(MediaStore.Video.Media.DATA, videoFile.getAbsolutePath());*/

			values.Call("put", "title", "Snapchat Video");
			values.Call ("put", "mime_type", "video/mp4");
			values.Call ("put", "_data", fileAbsolutePath);

			Debug.Log ("absolute path " + fileAbsolutePath);

			//return getContentResolver().insert(MediaStore.Video.Media.EXTERNAL_CONTENT_URI, values);
			//AndroidJavaObject mediaStore = new AndroidJavaObject("android.provider.MediaStore");
			//AndroidJavaObject mediaStoreVideoMedia = mediaStore.GetStatic<AndroidJavaObject>("Video");
			//AndroidJavaObject externalContentURI = mediaStoreVideoMedia.GetStatic<AndroidJavaObject> ("EXTERNAL_CONTENT_URI");

			//this should call MediaStore.Video.Media.EXTERNAL_CONTENT_URI 
			//but I couldn't figure out how to call the nested static classes... so hard coded.
			string externalContentURIString = "content://media/external/video/media";
			AndroidJavaObject AndroidURI = new AndroidJavaObject ("android.net.Uri");
			Debug.Log ("parsing...");
			AndroidJavaObject externalContentURI = AndroidURI.CallStatic<AndroidJavaObject> ("parse", externalContentURIString);
			string parsedString = externalContentURI.Call<string> ("toString");
			Debug.Log("parsed external content URI as " + parsedString);
			Debug.Log ("creating final URI...");

			AndroidJavaObject finalURI = contentResolver.Call<AndroidJavaObject> ("insert", externalContentURI, values);
			Debug.Log("inserted!");
			string videoURI = finalURI.Call<string> ("toString");
			string videoPath = finalURI.Call<string> ("getPath");

			Debug.Log ("videoUri: " + videoURI);
			Debug.Log("videoPath: " + videoPath);

		}


	}

	private AndroidJavaObject javaByteArrayFromCS(byte[] values) {
		Debug.Log ("creating byte array of size " + values.Length);
		//AndroidJavaObject defaultByte = new AndroidJavaObject ("java.lang.Byte");
		//AndroidJavaObject defaultByte = byteClass.GetStatic<AndroidJavaObject> ("MIN_VALUE");
		AndroidJavaClass arrayClass  = new AndroidJavaClass("java.lang.reflect.Array");
		AndroidJavaObject arrayObject = arrayClass.CallStatic<AndroidJavaObject>("newInstance",
										new AndroidJavaClass("java.lang.Byte"),
										values.Length);

		Debug.Log ("fill er up");
		for (int i=0; i<values.Length; ++i) {
			Debug.Log ("filling up #" + i);
			byte csDefaultByte = 0x20;
			AndroidJavaObject defaultByte = new AndroidJavaObject("java.lang.Byte", csDefaultByte);
			arrayClass.CallStatic("setByte", arrayObject, i, defaultByte);
		}
		Debug.Log ("done filling");
		return arrayObject;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
