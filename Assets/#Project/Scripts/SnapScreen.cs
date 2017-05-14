#if UNITY_5_6
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using System.IO;

public class SnapScreen : MonoBehaviour {

    public VideoPlayer _videoPlayer;
    public SnapMenu _menu;

    private string filePath;
    private string fullPath;

    // Use this for initialization
    void Start () {
        SubscribeToEvents();
        //SetUpLink();
        _videoPlayer.Prepare();
    }

    private void Update() {
       // Debug.Log("is playing: " + _videoPlayer.isPlaying + " is prepared: " + _videoPlayer.isPrepared);       
    }

    public void SetVideo(VideoClip clip) {
        _videoPlayer.clip = clip;
    }

    private void SubscribeToEvents() {
        _videoPlayer.loopPointReached += Play;
        _videoPlayer.prepareCompleted += Play;
        _videoPlayer.prepareCompleted += HideStartScreen;
    }

    private void HideStartScreen(UnityEngine.Video.VideoPlayer vp) {
        _menu.HideStartScreen();
    }

    private void SetUpLink() {
        if (Application.isEditor) {
            filePath = "./" + Application.dataPath + "/Resources/";
        } else {
            filePath = "./files/Resources/";//Application.persistentDataPath + " / Resources/"
        }
        _videoPlayer.url = fullPath;//"file:/" 
        Debug.Log("FILEPATH => " + fullPath);

        _videoPlayer.skipOnDrop = true;
        fullPath = filePath + "video.mp4";
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
