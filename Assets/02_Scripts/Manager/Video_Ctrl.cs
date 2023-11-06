using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Video_Ctrl : MonoBehaviour
{
    public GameObject videoPlayer;
    public GameObject Computer;
    public GameObject GameStartPannel;

    private GameObject mainCam;
    private UnityEngine.Video.VideoPlayer VideoComponent;
    private const float WaitTime = 1f;
    private float WaitCount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        VideoComponent = videoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>();
        mainCam = GameObject.Find("Main Camera");
    }

    private void Update()
    {
        WaitCount += Time.deltaTime;

        if (VideoComponent.isPlaying == false && WaitCount >= WaitTime)
        {
            mainCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            Computer.SetActive(false);
            GameStartPannel.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
