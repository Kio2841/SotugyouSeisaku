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
    int jumpGrowLevel = 1; //ジャンプレベル
    bool doubleJumpFlg = false; //2段ジャンプフラグ

    //GrowDash関連変数
    int dashGrowLevel = 1;
    float speed;
    [SerializeField] float dashTime = 5.0f;//ダッシュ受付時間
    //回避無敵フラグ
    bool invincibleFlg;


    Vector3 moveDirection = Vector3.zero;

    Vector3 startPos;

    //SE用
    public AudioClip sound3;
    AudioSource SE;


    void Start()
    {
        con = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        grow = GetComponent<GrowScript>();

        // マウスカーソルを非表示にし、位置を固定
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        startPos = transform.position;

        SE = GetComponent<AudioSource>(); //SE変数


    }

    void Update()
    {
        // 移動速度を取得       
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
            dashGrowLevel++;
        }
        else
        {
            speed = normalSpeed;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftControl)&&dashGrowLevel > 5)
        {
            dashGrowLevel++;
            speed = sprintSpeed * 50;
        }

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

    // 指定地点に強制移動する
    public void MoveStartPos()
    {
        con.enabled = false;

        moveDirection = Vector3.zero;
        transform.position = startPos + Vector3.up * 10f;
        transform.rotation = Quaternion.identity;

        con.enabled = true;
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
                //ジャンプSE
                SE.PlayOneShot(sound3);

                //ジャンプレベルを上げる
                jumpGrowLevel++;

                moveDirection.y = jump;
            }
            //ジャンプ中に関わる変数
            //地面についている時にリセット
            jumpEnd = false;
            jumpTime = 0;
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
}
