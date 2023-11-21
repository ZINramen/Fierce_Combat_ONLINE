using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV_ButtonCtrl : MonoBehaviour
{
    public GameObject OK_Screen;

    private bool is_Coll;               // tv와 trigger했는가?
    public bool Is_CollPlayer { get => is_Coll; }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            is_Coll = true;
        }

        if (OK_Screen != null)
            OK_Screen.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            is_Coll = false;
        }

        if (OK_Screen != null)
            OK_Screen.SetActive(false);
    }
}
