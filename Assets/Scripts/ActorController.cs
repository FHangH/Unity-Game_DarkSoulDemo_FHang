using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput pi;
    public float walkSpeed = 1.4f;
    public float runMultiplier = 2.7f;
    public float jumpVelocity = 3.0f;
    public float rollVelocity = 3.0f;

    [Space(10)]
    [Header("==== Firction Setting ====")]
    public PhysicMaterial firctionOne;
    public PhysicMaterial firctionZero;

    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrushVec;
    private Vector3 targetForward;
    private Vector3 deltaPos;
    private float lerpTarget;
    private float currentWeight;
    private float targetRunMulti;
    private bool lockPlanar = false;
    private bool canAttack;
    private CapsuleCollider col;

    private void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        targetRunMulti = ((pi.run) ? 2.0f : 1.0f);
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), targetRunMulti, 0.5f));

        if (rigid.velocity.magnitude > 0f)
        {
            anim.SetTrigger("roll");
        }

        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }

        if (pi.Dmag > 0.1f)
        {
            targetForward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.15f);
            model.transform.forward = targetForward;
        }

        if (lockPlanar == false)
        {
            planarVec = model.transform.forward * pi.Dmag * walkSpeed * ((pi.run) ? runMultiplier : 1.0f);
        }

        if (pi.attack && CheckState("ground") && canAttack)
        {
            anim.SetTrigger("attack");
        }
    }

    private void FixedUpdate()
    {
        rigid.position += deltaPos;
        //rigid.position += planarVec * Time.fixedDeltaTime;
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrushVec;
        thrushVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    private bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }

    //
    //Messages progressing
    //
    public void OnJumpEnter()
    {
        pi.InputEnable = false;
        lockPlanar = true;
        thrushVec = new Vector3(0, jumpVelocity, 0);
    }
    
    //public void OnJumpExit()
    //{
    //    pi.InputEnable = true;
    //    lockPlanar = false;
    //    Debug.Log("OnJumpExit");
    //}

    public void IsGround()
    {
        anim.SetBool("isGround", true);
        canAttack = true;
    }

    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        pi.InputEnable = true;
        lockPlanar = false;
        col.material = firctionOne;
    }

    public void OnGroundExit()
    {
        col.material = firctionZero;
    }

    public void OnFallEnter()
    {
        pi.InputEnable = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        pi.InputEnable = false;
        lockPlanar = true;
        thrushVec = new Vector3(0, rollVelocity, rollVelocity);
    }

    public void OnJabEnter()
    {
        pi.InputEnable = false;
        lockPlanar = true;
    }

    public void OnJabUpdate()
    {
        thrushVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

    public void OnAttack1hAEnter()
    {
        pi.InputEnable = false;
        //lockPlanar = true;
        lerpTarget = 1.0f;
    }

    public void OnAttack1hAUpdate()
    {
        thrushVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
        currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("attack"));
        currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.8f);
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), currentWeight);
    }

    public void OnAttackIdleEnter()
    {
        pi.InputEnable = true;
        //lockPlanar = false;
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), 0);
        lerpTarget = 0;
    }

    public void OnAttackIdleUpdate()
    {
        currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("attack"));
        currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.5f);
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), currentWeight);
    }

    public void OnUpdateRM(object _deltaPos)
    {
        if (CheckState("attack1hC", "attack"))
        {
            deltaPos += (Vector3)_deltaPos;
        }
    }
}
