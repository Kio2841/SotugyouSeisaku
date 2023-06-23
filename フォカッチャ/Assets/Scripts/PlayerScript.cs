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

    float normalSpeed = 3f; // �ʏ펞�̈ړ����x
    float sprintSpeed = 5f; // �_�b�V�����̈ړ����x
    float gravity = 10f;    // �d�͂̑傫��

    //GrowJump�֘A�ϐ�
    bool jumpEnd;           //�W�����v�{�^������������
    [SerializeField] float jumpTime;//�W�����v���ԑ���
    float jump = 6f;        // �W�����v��
    public int jumpGrowLevel = 1; //�W�����v���x��
    bool doubleJumpFlg = false; //2�i�W�����v�t���O

    //GrowDash�֘A�ϐ�
    public int dashGrowLevel = 1;
    float speed;
    //��𖳓G�t���O
    bool invincibleFlg;
    //�󒆉���t���O
    bool airDodgeFlg;


    Vector3 moveDirection = Vector3.zero;

    Vector3 startPos;


    void Start()
    {
        con = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        grow = GetComponent<GrowScript>();

        // �}�E�X�J�[�\�����\���ɂ��A�ʒu���Œ�
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        startPos = transform.position;

        //�󒆉���\�t���O���I�t��
        airDodgeFlg = false;
        
    }

    void Update()
    {
        DashGrow();

        // �J�����̌�������ɂ������ʕ����̃x�N�g��
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �O�㍶�E�̓��́iWASD�L�[�j����A�ړ��̂��߂̃x�N�g�����v�Z
        // Input.GetAxis("Vertical") �͑O��iWS�L�[�j�̓��͒l
        // Input.GetAxis("Horizontal") �͍��E�iAD�L�[�j�̓��͒l
        moveZ = cameraForward * Input.GetAxis("Vertical") * speed;  //�@�O��i�J������j�@ 
        moveX = Camera.main.transform.right * Input.GetAxis("Horizontal") * speed; // ���E�i�J������j


        // �d�͂�ݒ肵�Ȃ��Ɨ������Ȃ�
        moveDirection.y -= gravity * Time.deltaTime;

        JumpGrow();

        // �ړ��̃A�j���[�V����
        anim.SetFloat("MoveSpeed", (moveZ + moveX).magnitude);

        // �v���C���[�̌�������͂̌����ɕύX�@
        transform.LookAt(transform.position + moveZ + moveX);

        // Move �͎w�肵���x�N�g�������ړ������閽��
        con.Move(moveDirection * Time.deltaTime);
    }


    //�����W�����v�X�N���v�g
    void JumpGrow()
    {
        // isGrounded �͒n�ʂɂ��邩�ǂ����𔻒肵�܂�
        // �n�ʂɂ���Ƃ��A��������2�i�W�����v�t���O���I���̎��̓W�����v���\��
        if (con.isGrounded)
        {
            moveDirection = moveZ + moveX;

            //�W�����v���x��10�ȍ~��i�W�����v����)
            if (jumpGrowLevel > 10)
            doubleJumpFlg = true;

            if (Input.GetButtonDown("Jump"))
            {
                //�W�����v���x�����グ��
                jumpGrowLevel++;

                moveDirection.y = jump;
            }
            //�W�����v���Ɋւ��ϐ�
            //�n�ʂɂ��Ă��鎞�Ƀ��Z�b�g
            jumpEnd = false;
            jumpTime = 0;
            //�󒆉���t���O���߂�
            airDodgeFlg = true;
        }
        else
        {
            //�W�����v���x��5����͉��������ɂ���č�����ς���
            //�ő�0.75f
            if (Input.GetButton("Jump") && jumpTime < 0.75f && !jumpEnd && jumpGrowLevel > 5)
            {
                jumpTime += Time.deltaTime;
                moveDirection.y = jump;
            }

            //�W�����v���ɃW�����v�{�^���𗣂������Ƃ��L�^
            if (Input.GetButtonUp("Jump") && !jumpEnd)
                jumpEnd = true;

            //��i�W�����v�\�Ȃ��i�W�����v�ł���
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
        // �ړ����x���擾       
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

        //�_�b�V�����x����5�ȏォ�n�ʂɂ���Ȃ���
        if (Input.GetKeyDown(KeyCode.LeftControl) && dashGrowLevel > 5 && con.isGrounded)
        {
            dashGrowLevel++;
            //speed = sprintSpeed * 50;
            // Move �͎w�肵���x�N�g�������ړ������閽��
            con.Move(moveDirection * 150 * Time.deltaTime);
        }
        //�_�b�V�����x��10�ȏォ�󒆂��󒆉���t���O��true�Ȃ�󒆉��
        else if (Input.GetKeyDown(KeyCode.LeftControl) && dashGrowLevel > 10 && !con.isGrounded && airDodgeFlg)
        {
            dashGrowLevel++;
            //speed = sprintSpeed * 50;
            // Move �͎w�肵���x�N�g�������ړ������閽��
            moveDirection.y = 0;
            con.Move(moveDirection * 150 * Time.deltaTime);
            airDodgeFlg = false;
        }
    }
}
