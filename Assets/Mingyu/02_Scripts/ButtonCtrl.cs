using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtrl : MonoBehaviour
{
    // 버튼이 스크린을 가지고 있다.
    public GameObject OK_Screen;

    private bool is_Coll;               // tv와 trigger했는가?
    public bool Is_CollPlayer { get => is_Coll; }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            OK_Screen.SetActive(true);
            is_Coll = true;
        }

    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            OK_Screen.SetActive(false);
            is_Coll = false;
        }
    }
}
