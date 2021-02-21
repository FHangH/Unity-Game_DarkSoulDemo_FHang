using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //设置输入的键位
    [Header("==== 输入信号 ====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyA = "left shift";
    public string keyB= "space";
    public string keyC;
    public string keyD;

    public string keyJUp = "up";
    public string keyJDown = "down";
    public string keyJLeft = "left";
    public string keyJRight = "right";

    //将键位值转成抽象信号
    [Header("==== 输出信号 ====")]
    public float Dup;
    public float Dright;
    private float Dup2;
    private float Dright2;
    public float Dmag;
    public float Jup;
    public float Jright;
    public Vector3 Dvec;
    private Vector2 tempDAxis;
    public bool run;
    public bool jump;
    private bool lastJump;
    private bool newJump;
    public bool attack;
    private bool lastAttack;
    private bool newAttack;

    //设置目标信号和内存信号
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    [Header("==== 输入开关 ====")]
    public bool InputEnable = true;


    void Update()
    {
        Jup = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
        Jright = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);

        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if (InputEnable == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        Dright2 = tempDAxis.x;
        Dup2 = tempDAxis.y;
        
        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;
        
        run = Input.GetKey(keyA);

        newJump = Input.GetKey(keyB);
        if (jump != lastJump && newJump == true)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        lastJump = newJump;

        //newAttack = Input.GetKey(keyC);
        newAttack = Input.GetKey(KeyCode.Mouse0);
        if (attack != lastAttack && newAttack == true)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }
        lastAttack = newAttack;
    }

    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }
}
