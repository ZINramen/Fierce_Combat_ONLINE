using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Video_Ctrl : MonoBehaviour
{
    public GameObject Video_Obj;
    public GameObject videoPlayer;
    public GameObject Computer;
    public GameObject GameStartPannel;

    private GameObject mainCam;
    private UnityEngine.Video.VideoPlayer VideoComponent;

    private bool is_StartVideo = false;

    private const float WaitTime = 1f;
    private float WaitCount = 0f;


    // Start is called before the first frame update
    private void Start()
    {
        VideoComponent = videoPlayer.GetComponent<UnityEngine.Video.VideoPlayer>();
        mainCam = GameObject.Find("Main Camera");
    }

    private void Update()
    {
        if (mainCam.GetComponent<Cam_AnimCtrl>().Get_isEndAnim() 
            && is_StartVideo == false)
        {
            is_StartVideo = true;

            VideoComponent.Play();
            Video_Obj.SetActive(true);
        }

        else if(is_StartVideo)
            WaitCount += Time.deltaTime;

        if (VideoComponent.isPlaying == false && WaitCount >= WaitTime)
        {
            mainCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            GameStartPannel.SetActive(true);
            Video_Obj.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
