#if UNITY_5_6
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using System.IO;

public class SnapScreen : MonoBehaviour {

    public VideoPlayer _videoPlayer;

    private string filePath;
    private string fullPath;

    // Use this for initialization
    void Start () {

        if (Application.isEditor) {
            filePath = "./" + Application.dataPath + "/Resources/";           
        } else {
			filePath = "file:///storage/emulated/0/Snapchat/";
        }

        _videoPlayer.loopPointReached += Play;
        _videoPlayer.prepareCompleted += Play;

        _videoPlayer.skipOnDrop = true;
        fullPath = filePath + "snapmemory1.mp4";
        Debug.Log("FILEPATH => " + fullPath);
        _videoPlayer.url = fullPath;//"file:/" + 
        _videoPlayer.Prepare();
        
    }

    private void Update() {
         //Debug.Log("is playing: " + _videoPlayer.isPlaying + " is prepared: " + _videoPlayer.isPrepared);

    }

    public void SetVideo(VideoClip clip) {
        _videoPlayer.clip = clip;
    }

    private void Play(UnityEngine.Video.VideoPlayer vp) {
        Play();
    }

    public void Play() {
        _videoPlayer.Play();
    }

    public void Pause() {
        _videoPlayer.Pause();
    }
}
#endif
