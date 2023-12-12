using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneLoad : MonoBehaviour
{
    public float delaytime;
    public string sname = "TitleMenu(Demo)";
    void Start()
    {
        StartCoroutine(Load());
        
    }
    IEnumerator Load() 
    {
        yield return new WaitForSeconds(delaytime);
        SceneManager.LoadScene(sname, LoadSceneMode.Single);
    }
}
