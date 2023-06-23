using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerScript : MonoBehaviour
{

    CharacterController con;
    Animator anim;
    GrowScript grow;
    Vector3 moveZ;
    Vector3 moveX;

    float normalSpeed = 3f; // 通常時の移動速度
    float sprintSpeed = 5f; // ダッシュ時の移動速度
    float gravity = 10f;    // 重力の大きさ

    //GrowJump関連変数
    bool jumpEnd;           //ジャンプボタン離した判定
    [SerializeField] float jumpTime;//ジャンプ時間測定
    float jump = 6f;        // ジャンプ力
    public int jumpGrowLevel = 1; //ジャンプレベル
    bool doubleJumpFlg = false; //2段ジャンプフラグ

    //GrowDash関連変数
    public int dashGrowLevel = 1;
    float speed;
    //回避無敵フラグ
    bool invincibleFlg;
    //空中回避フラグ
    bool airDodgeFlg;


    Vector3 moveDirection = Vector3.zero;

    Vector3 startPos;


    void Start()
    {
        con = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        grow = GetComponent<GrowScript>();

        // マウスカーソルを非表示にし、位置を固定
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        startPos = transform.position;

        //空中回避可能フラグをオフに
        airDodgeFlg = false;
        
    }

    void Update()
    {
        DashGrow();

        // カメラの向きを基準にした正面方向のベクトル
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 前後左右の入力（WASDキー）から、移動のためのベクトルを計算
        // Input.GetAxis("Vertical") は前後（WSキー）の入力値
        // Input.GetAxis("Horizontal") は左右（ADキー）の入力値
        moveZ = cameraForward * Input.GetAxis("Vertical") * speed;  //　前後（カメラ基準）　 
        moveX = Camera.main.transform.right * Input.GetAxis("Horizontal") * speed; // 左右（カメラ基準）


        // 重力を設定しないと落下しない
        moveDirection.y -= gravity * Time.deltaTime;

        JumpGrow();

        // 移動のアニメーション
        anim.SetFloat("MoveSpeed", (moveZ + moveX).magnitude);

        // プレイヤーの向きを入力の向きに変更　
        transform.LookAt(transform.position + moveZ + moveX);

        // Move は指定したベクトルだけ移動させる命令
        con.Move(moveDirection * Time.deltaTime);
    }


    //成長ジャンプスクリプト
    void JumpGrow()
    {
        // isGrounded は地面にいるかどうかを判定します
        // 地面にいるとき、もしくは2段ジャンプフラグがオンの時はジャンプを可能に
        if (con.isGrounded)
        {
            moveDirection = moveZ + moveX;

            //ジャンプレベル10以降二段ジャンプ解禁)
            if (jumpGrowLevel > 10)
            doubleJumpFlg = true;

            if (Input.GetButtonDown("Jump"))
            {
                //ジャンプレベルを上げる
                jumpGrowLevel++;

                moveDirection.y = jump;
            }
            //ジャンプ中に関わる変数
            //地面についている時にリセット
            jumpEnd = false;
            jumpTime = 0;
            //空中回避フラグも戻す
            airDodgeFlg = true;
        }
        else
        {
            //ジャンプレベル5からは押す長さによって高さを変える
            //最大0.75f
            if (Input.GetButton("Jump") && jumpTime < 0.75f && !jumpEnd && jumpGrowLevel > 5)
            {
                jumpTime += Time.deltaTime;
                moveDirection.y = jump;
            }

            //ジャンプ中にジャンプボタンを離したことを記録
            if (Input.GetButtonUp("Jump") && !jumpEnd)
                jumpEnd = true;

            //二段ジャンプ可能なら二段ジャンプできる
            if (jumpEnd && doubleJumpFlg)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jump * 1.6f;
                    doubleJumpFlg = false;
                }
            }
        }
    }

    void DashGrow()
    {
        // 移動速度を取得       
        if (Input.GetKey(KeyCode.LeftShift) && con.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                dashGrowLevel++;

            speed = sprintSpeed;
        }
        else
        {
            speed = normalSpeed;
        }

        //ダッシュレベルが5以上かつ地面にいるなら回避
        if (Input.GetKeyDown(KeyCode.LeftControl) && dashGrowLevel > 5 && con.isGrounded)
        {
            dashGrowLevel++;
            //speed = sprintSpeed * 50;
            // Move は指定したベクトルだけ移動させる命令
            con.Move(moveDirection * 150 * Time.deltaTime);
        }
        //ダッシュレベル10以上かつ空中かつ空中回避フラグがtrueなら空中回避
        else if (Input.GetKeyDown(KeyCode.LeftControl) && dashGrowLevel > 10 && !con.isGrounded && airDodgeFlg)
        {
            dashGrowLevel++;
            //speed = sprintSpeed * 50;
            // Move は指定したベクトルだけ移動させる命令
            moveDirection.y = 0;
            con.Move(moveDirection * 150 * Time.deltaTime);
            airDodgeFlg = false;
        }
    }
}
