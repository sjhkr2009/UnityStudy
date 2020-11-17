using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorIK : MonoBehaviour
{
    public Animator anim;
    [SerializeField] bool isJumping = false;

    public Transform target;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            anim.SetTrigger("Jump");
            isJumping = true;
            Invoke(nameof(JumpingExit), 0.9f);
        }

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", verticalInput);
        anim.SetFloat("Horizontal", horizontalInput);
    }

    void JumpingExit() { isJumping = false; }

    private void OnAnimatorIK() //애니메이션의 특정 부위가 대상을 추적하도록 한다. IK가 적용될 애니메이션과 대상이 있다면 자동으로 실행된다. 애니메이터에서 IK Pass가 체크되어야 한다.
    {
        //물건을 집거나 특정 지점을 바라볼 때 사용된다.
        
        //Weight는 해당 부위가 대상을 위치나 회전에서 얼마나 잘 추적할지 0부터 1의 값으로 입력한다.
        //1이면 기존 애니메이션을 무시하고 대상을 완전히 추적하며, 0과 1 사이의 값이면 기존 애니메이션과 해당 비율만큼 혼합된다.
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.6f);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        //추적할 부위와 대상을 지정한다.
        anim.SetIKPosition(AvatarIKGoal.RightHand, target.position);
        anim.SetIKRotation(AvatarIKGoal.RightHand, target.rotation);

        //시선은 SetLookAt으로 이동시킬 수 있다.
        anim.SetLookAtWeight(1.0f);
        anim.SetLookAtPosition(target.position);
    }
}
