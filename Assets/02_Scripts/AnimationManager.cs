using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    enum AnimationState 
    { Normal, Jump, Fall, Emotion, Attack, Stun }
    
    CapsuleCollider2D c2;
    Animator ani;
    AnimationState State = AnimationState.Normal;

    [Header("PlayerSet")] 
    [Tooltip("조종할 플레이어 캐릭터의 경우 True")]
    public bool isPlayer = false; 

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        c2 = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        BasicAnimation();
        if (isPlayer)
            PlayerAnimation();
    }
    void StateChange(AnimationState newState) 
    {
        State = newState;
        ani.ResetTrigger("Jump");
        ani.ResetTrigger("Breaktime");
    }
    void BasicAnimation() // 모든 캐릭터의 애니메이션 관리 -> 상황에 반응
    {
        if (Physics2D.Raycast(transform.position - new Vector3(0, c2.size.y * 0.7f), Vector3.down, 0.2f))
        {
            if (State == AnimationState.Fall)
            {
                ani.ResetTrigger("Jump");
                ani.SetTrigger("Landing");
                State = AnimationState.Normal;
            }
        }
        else 
        {
            if (State == AnimationState.Jump)
                State = AnimationState.Fall;
        }
    }
    void PlayerAnimation() // 조종하는 플레이어 캐릭터의 애니메이션 관리 -> 입력에 반응
    {
        Debug.DrawLine(transform.position - new Vector3(0, c2.size.y*0.7f), transform.position - new Vector3(0, c2.size.y*0.7f) + Vector3.down * 0.2f, Color.red);
        if (Physics2D.Raycast(transform.position - new Vector3(0, c2.size.y/2), Vector3.down, 0.2f) && Input.GetKeyDown(KeyCode.Space))
        {
            State = AnimationState.Jump;
            ani.SetTrigger("Jump");
        }
        if (State == AnimationState.Normal && Input.GetAxis("Horizontal") == 0 && Input.GetKeyDown(KeyCode.F1)) 
        {
            State = AnimationState.Emotion;
            ani.SetTrigger("Breaktime");
        }
    }
}
