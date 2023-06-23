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
    int jumpGrowLevel = 1; //�W�����v���x��
    bool doubleJumpFlg = false; //2�i�W�����v�t���O

    //GrowDash�֘A�ϐ�
    int dashGrowLevel = 1;
    float speed;
    [SerializeField] float dashTime = 5.0f;//�_�b�V����t����
    //��𖳓G�t���O
    bool invincibleFlg;


    Vector3 moveDirection = Vector3.zero;

    Vector3 startPos;

    //SE�p
    public AudioClip sound3;
    AudioSource SE;


    void Start()
    {
        con = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        grow = GetComponent<GrowScript>();

        // �}�E�X�J�[�\�����\���ɂ��A�ʒu���Œ�
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        startPos = transform.position;

        SE = GetComponent<AudioSource>(); //SE�ϐ�


    }

    void Update()
    {
        // �ړ����x���擾       
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

    // �w��n�_�ɋ����ړ�����
    public void MoveStartPos()
    {
        con.enabled = false;

        moveDirection = Vector3.zero;
        transform.position = startPos + Vector3.up * 10f;
        transform.rotation = Quaternion.identity;

        con.enabled = true;
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
                //�W�����vSE
                SE.PlayOneShot(sound3);

                //�W�����v���x�����グ��
                jumpGrowLevel++;

                moveDirection.y = jump;
            }
            //�W�����v���Ɋւ��ϐ�
            //�n�ʂɂ��Ă��鎞�Ƀ��Z�b�g
            jumpEnd = false;
            jumpTime = 0;
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
}
