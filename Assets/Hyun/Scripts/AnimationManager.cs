using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private bool ecActive = false;

    public float rotationZ = 0;

    public enum AnimationState 
    { Normal, Jump, Fall, Emotion, Stun }
    
    Animator ani;
    public AnimationState State = AnimationState.Normal;

    [Header("PlayerSet")] 
    [Tooltip("조종할 플레이어 캐릭터의 경우 True")]
    public bool isPlayer = false; 

    public EffectCreator Ec;
    
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer)
            PlayerAnimation();
        
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotationZ);
    }
    void ResetTriggerEvent(string name) 
    {
        ani.ResetTrigger(name);
    }
    void ResetAttackTriggerEvent()
    {
        ani.ResetTrigger("Punch_Up");
        ani.ResetTrigger("Punch");
        ani.ResetTrigger("Kick");
        ani.ResetTrigger("Catch");
    }

    void StateChange(AnimationState newState) 
    {
        State = newState;
        ani.ResetTrigger("Jump");
        ani.ResetTrigger("Breaktime");
    }
    public void Hit(float power)
    {
        if(Math.Abs(power) > 10)
            ani.SetTrigger("Hit_Upgrade");
        else
            ani.SetTrigger("Hit");

    }
    void PlayerAnimation() // 조종하는 플레이어 캐릭터의 애니메이션 관리 -> 입력에 반응
    {
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(transform.position - new Vector3(0, transform.localScale.y / 2), Vector3.down, 0.2f))
        {
            if (State == AnimationState.Fall)
            {
                ani.ResetTrigger("Jump");
                ani.SetTrigger("Landing");
                if (Ec && ecActive)
                {
                    Ec.PlayEffect("bang", hit);
                    ecActive = false;
                }
                State = AnimationState.Normal;
            }
        }
        else
        {
            if (State == AnimationState.Jump)
                State = AnimationState.Fall;

            if (Input.GetKeyDown(KeyCode.D))
            {
                ani.SetTrigger("Kick");
                ecActive = true;
            }
        }
        if (Physics2D.Raycast(transform.position - new Vector3(0, transform.localScale.y/2), Vector3.down, 0.2f) && Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.DownArrow))
        {
            State = AnimationState.Jump;
            ani.SetTrigger("Jump");
        }
        if (State == AnimationState.Normal) 
        {
            if (Input.GetAxis("Horizontal") == 0 && Input.GetKeyDown(KeyCode.F1))
            {
                State = AnimationState.Emotion;
                ani.SetTrigger("Breaktime");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    ani.SetTrigger("Punch_Up");
                }
                else if(!ani.GetBool("Down"))
                    ani.SetTrigger("Punch");
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                ani.SetTrigger("Kick");
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                ani.SetBool("Down", true);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                ani.SetBool("Down", false);
            }
            if (Input.GetKey(KeyCode.A))
            {
                ani.SetBool("Defense", true);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                ani.SetBool("Defense", false);
            }
            if (Input.GetAxis("Horizontal") == 0)
            {
                if (Input.GetKeyDown(KeyCode.F) && !ani.GetBool("Down"))
                {
                    ani.SetTrigger("Dash");
                }
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                ani.SetTrigger("Catch");
            }
        }
    }
}
