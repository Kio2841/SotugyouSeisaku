using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowScript : MonoBehaviour
{
    CharacterController con;
    float jump = 8f;        // ジャンプ力
    int jumpGrowLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        con = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ジャンプ成長
    public float JumpGrow()
    {
        jumpGrowLevel++;
        //ノーマルジャンプ
        // isGrounded は地面にいるかどうかを判定します
        // 地面にいるときはジャンプを可能に
        if (jumpGrowLevel <= 5)
            return jump;

        //ハイジャンプ
        //ジャンプ15回でハイジャンプに
        //スペースの押す長さでジャンプの高さを変える(最大0.75秒)
        else if (jumpGrowLevel <= 15)
            return (jump * 1.5f);

        //二段ジャンプ
        //ハイジャンプ15で二段ジャンプ解禁
        //空中でスペースを押すともう一回ジャンプ
        else
            return jump;
    }
    //攻撃成長
    void AttackGrow()
    {
        //攻撃

        //大振り攻撃
            //攻撃15回で大振り攻撃に
            //攻撃範囲を広げる(1.3倍程度)

        //2段切り
            //大振り攻撃15回でに段切り解禁
            //一度の近接攻撃で2回攻撃判定を出す

    }
    //ダッシュ(回避)成長
    void MoveGrow()
    {
        //ダッシュ

        //回避
            //ダッシュ5回で回避解禁
            //一定距離を無敵状態で移動

        //瞬歩
            //回避15回+アイテムで解禁
            //一定距離を無敵状態で移動し、崖などを渡れるようにする(短距離瞬間移動)
            
    }
}
