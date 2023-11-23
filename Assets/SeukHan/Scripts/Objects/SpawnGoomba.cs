using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGoomba : MonoBehaviour
{
    public Vector3 SpawnPoint;
    public GameObject goomba;
    public int GoombaCount
    { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SummonGoomba());
    }

    IEnumerator SummonGoomba()
    {
        while (true)
        {
            if (GoombaCount < 5)
            {
                Instantiate(goomba);
            }

            yield return new WaitForSeconds(5.0f);
        }
    }
}
