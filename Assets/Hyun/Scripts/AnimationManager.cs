using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private bool onGround = true;
    private bool ecActive = false;

    public float rotationZ = 0;

    public enum AnimationState 
    { Normal, Jump, Fall, Emotion, Stun }
    
    Animator ani;
    public AnimationState State = AnimationState.Normal;

    [Header("PlayerSet")] 
    [Tooltip("조종할 플레이어 캐릭터의 경우 True")]
    public bool isPlayer = false;
    public bool isHuman = false;

    public EffectCreator Ec;
    
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit;
        if (hit = Physics2D.Raycast(transform.position - new Vector3(0, transform.localScale.y / 2), Vector3.down, 0.2f))
        {
            if(hit.collider.transform.parent != transform)
            if (State == AnimationState.Fall)
            {
                onGround = true;
                if (isHuman)
                {
                    ani.ResetTrigger("Jump");
                    ani.SetTrigger("Landing");
                }
                State = AnimationState.Normal;
                if (Ec && ecActive)
                {
                    Ec.PlayEffect("bang", hit);
                    ecActive = false;
                }
            }
        }
        else
        {
            if (State == AnimationState.Normal)
            {
                if (isHuman)
                    ani.SetTrigger("fall");
            }
            if (onGround)
            {
                onGround = false;
                if (isHuman)
                {
                    ani.ResetTrigger("Landing");
                }
            }
            else
            {
                if (isPlayer)
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        ani.SetTrigger("Kick");
                        ecActive = true;
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        ani.SetTrigger("Punch");
                    }
                }
            }
        }
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
        ani.ResetTrigger("Dodge");
        ani.ResetTrigger("Dash");
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
            if (Input.GetKeyUp(KeyCode.W))
            {
                ani.SetTrigger("Dodge");
            }
            if (Input.GetKey(KeyCode.Z))
            {
                ani.SetBool("Heal", true);
            }
            if (Input.GetKeyUp(KeyCode.Z))
            {
                ani.SetBool("Heal", false);
            }
        }
    }
}
