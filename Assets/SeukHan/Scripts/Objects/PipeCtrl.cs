using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCtrl : MonoBehaviour
{
    public Vector2 playerCheckRange;
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        playerCollCheck();
    }

    private void playerCollCheck()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, playerCheckRange, 0, playerLayer);

        if(hit.gameObject.GetComponent<Animator>().GetBool("Down"))
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, playerCheckRange);
    }
}
