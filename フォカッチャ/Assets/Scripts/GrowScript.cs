using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowScript : MonoBehaviour
{
    CharacterController con;
    float jump = 8f;        // �W�����v��
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

    //�W�����v����
    public float JumpGrow()
    {
        jumpGrowLevel++;
        //�m�[�}���W�����v
        // isGrounded �͒n�ʂɂ��邩�ǂ����𔻒肵�܂�
        // �n�ʂɂ���Ƃ��̓W�����v���\��
        if (jumpGrowLevel <= 5)
            return jump;

        //�n�C�W�����v
        //�W�����v15��Ńn�C�W�����v��
        //�X�y�[�X�̉��������ŃW�����v�̍�����ς���(�ő�0.75�b)
        else if (jumpGrowLevel <= 15)
            return (jump * 1.5f);

        //��i�W�����v
        //�n�C�W�����v15�œ�i�W�����v����
        //�󒆂ŃX�y�[�X�������Ƃ������W�����v
        else
            return jump;
    }
    //�U������
    void AttackGrow()
    {
        //�U��

        //��U��U��
            //�U��15��ő�U��U����
            //�U���͈͂��L����(1.3�{���x)

        //2�i�؂�
            //��U��U��15��łɒi�؂����
            //��x�̋ߐڍU����2��U��������o��

    }
    //�_�b�V��(���)����
    void MoveGrow()
    {
        //�_�b�V��

        //���
            //�_�b�V��5��ŉ������
            //��苗���𖳓G��Ԃňړ�

        //�u��
            //���15��+�A�C�e���ŉ���
            //��苗���𖳓G��Ԃňړ����A�R�Ȃǂ�n���悤�ɂ���(�Z�����u�Ԉړ�)
            
    }
}
