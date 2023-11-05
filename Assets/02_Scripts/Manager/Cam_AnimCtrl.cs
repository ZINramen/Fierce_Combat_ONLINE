using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_AnimCtrl : MonoBehaviour
{
    public GameObject Computer;

    private bool IsEnd_Anim = false;
    private Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    public void EndClose_Anim()
    {
        IsEnd_Anim = true;
    }

    public bool Get_isEndAnim()
    {
        return IsEnd_Anim;
    }

    private void Update()
    {
        if (Computer.GetComponent<Computer_Anim>().Get_isComputer_On())
            anim.SetBool("IsStart_Anim", true);
    }
}
