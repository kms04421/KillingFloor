using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviourPun
{
    private PlayerInputs input;
    private CharacterController controller;
    public Animator tpsAnimator;    // �÷��̾� TPS �� �ִϸ�����
    public Animator fpsAnimator;    // �÷��̾� FPS �� �ִϸ�����

    [Header("Player")]
    [Tooltip("�÷��̾� �̵��ӵ� m/s")]
    public float moveSpeed;
    [Tooltip("�÷��̾� ��üӵ� m/s")]
    public float dashSpeed;
    [Tooltip("�÷��̾� ȸ���ӵ�")]
    public float rotationSpeed;
    [Tooltip("�̵� ���ӵ�")]
    public float speedChangeRate;

    [Space(10)]
    [Tooltip("���� ����")]
    public float jumpHeight;
    [Tooltip("�÷��̾� �߷� ��. �߷� �⺻�� : -9.81f")]
    public float gravity;

    [Space(10)]
    [Tooltip("���� ���¸� Ȯ���ϴ� ��. ���� 0�̸� �ٷ� ���� ����")]
    public float jumpTimeout;
    [Tooltip("�߶� ���¸� Ȯ���ϴ� ��")]
    public float fallTimeout;

    [Header("Player Grounded")]
    [Tooltip("�ٴڿ� �ִ��� ������ üũ")]
    public bool isGrounded = true;
    [Tooltip("�ٴ��� ����")]
    public float groundedOffset;
    [Tooltip("�ٴ� üũ ����")]
    public float groundedRadius;


    [Header("Cinemachine")]
    [Tooltip("�ó׹��� ����� ī�޶� ���� Ÿ��. FPS �÷��̾��� �Ӹ� ��ġ")]
    public GameObject cinemachineCameraTarget;
    public CinemachineVirtualCamera followCamera;
    [Tooltip("ī�޶� �ִ� ����")]
    public float topClamp;
    [Tooltip("ī�޶� �ּ� ����")]
    public float bottomClamp;

    public Transform weaponTarget;



    // cinemachine
    private float _cinemachineTargetPitch;

    // player
    public float _speed;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private const float _threshold = 0.01f;
    private bool IsCurrentDeviceMouse;

    private float animMoveSpeed;
    private AudioSource m_AudioSource;
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
    private float m_StepCycle;
    private float m_NextStep;

    //SSM 230925
    public GameObject shopNav;
    private List<GameObject> shopNavList;
    private GameObject shopNavPrant;
    private bool NavAct = false;

    private List<string> cashTem;
   
    //SSM End

    void Start()
    {
        
        cashTem = new List<string>();
        //ssm
        for (int i = 0; i < GameManager.instance.cashItem.Count; i++)
        {
            if (GameManager.instance.cashItem[i] == "GoldSkin")
            {
                
                photonView.RPC("changeMaster", RpcTarget.MasterClient,photonView.ViewID);   
             
            }
        }

        shopNavList = new List<GameObject>();
        shopNavPrant = GameObject.Find("ShopNavigation");
        if (shopNavPrant != null)
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject saveObj = Instantiate(shopNav, shopNavPrant.transform);
                shopNavList.Add(saveObj);
                shopNavList[i].SetActive(false);
            }
        }

        //ssm end
        input = GetComponent<PlayerInputs>();
        controller = GetComponent<CharacterController>();
        m_AudioSource = GetComponent<AudioSource>();
        _jumpTimeoutDelta = jumpTimeout;
        _fallTimeoutDelta = fallTimeout;
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
    }
    public void OnEnable()
    {
        _jumpTimeoutDelta = jumpTimeout;
        _fallTimeoutDelta = fallTimeout;
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
    }
    void Update()
    {
        // �Է� ���ɿ��� Ȯ��
        if (!photonView.IsMine) { return; } // ���� �÷��̾ �ƴ� ��� �Է��� ���� �ʴ´�.
        MouseSensitiveUpdate();
        GroundedCheck();    // �ٴ� üũ
        JumpAndGravity();   // ������ �߷� ���� �޼���
        Move();             // �̵� ���� �޼���
        ActiveAnimation();  // �ִϸ��̼� ����
        ShopUIUpdate();     // �̵��� ���� ���� UI ������Ʈ

        // �Է� ���� ���� Ȯ��
        if (GameManager.instance.inputLock)
        {
            GetComponent<PlayerInput>().enabled = false;
        }
        else if (!GameManager.instance.inputLock)
        {
            GetComponent<PlayerInput>().enabled = true;
        }
        if (GameManager.instance.isShop == true)
        {
            photonView.RPC("shopNavSp", RpcTarget.All);
       
        }
    }

    private void LateUpdate()
    {
        if (!photonView.IsMine) { return; } // ���� �÷��̾ �ƴ� ��� �Է��� ���� �ʴ´�.
        CameraRotation();
    }

    public void MouseSensitiveUpdate()
    {
        rotationSpeed = PlayerUIManager.instance.mouseSensitive.value;
    }
    // �ٴ� üũ
    private void GroundedCheck()
    {
        isGrounded = controller.isGrounded;
    }
    private void JumpAndGravity()
    {
        if (GameManager.instance != null && GameManager.instance.inputLock)
            return;

        if (isGrounded)
        {
            // �߶� Ÿ�Ӿƿ� �ʱ�ȭ
            _fallTimeoutDelta = fallTimeout;

            tpsAnimator.SetBool("isJump", false);
            tpsAnimator.SetBool("isAir", false);

            // ���� ���� 0���� �������� ��� -2�� ���ѽ��� �ӵ��� �����ϰ� �������� �� ����
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // ����
            if (input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                // H * -2 * G �� ������ = ���ϴ� ���̿� �����ϴµ� �ʿ��� �ӵ�
                _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                tpsAnimator.SetBool("isJump", true);

            }

            // ���� Ÿ�Ӿƿ�
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // �ٴڿ� ����ִ� ���¶�� ���� Ÿ�Ӿƿ� �ʱ�ȭ
            _jumpTimeoutDelta = jumpTimeout;

            // ���� ���� Ÿ�Ӿƿ��� �ʱ�ȭ
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }

            else
                tpsAnimator.SetBool("isAir", true);

            // �ٴ��� �ƴϸ� ������ ���ϰ� ó��
            input.jump = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }
    }
    private void CameraRotation()
    {
        if (GameManager.instance != null && GameManager.instance.inputLock)
            return;
        // ���콺 �Է��� ������
        if (input.look.sqrMagnitude >= _threshold)
        {
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetPitch += input.look.y * rotationSpeed * deltaTimeMultiplier;
            _rotationVelocity = input.look.x * rotationSpeed * deltaTimeMultiplier;

            // ȸ���� ����
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

            // �ó׸ӽ� ī�޶� Ÿ�� ������Ʈ
            cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
            weaponTarget.localRotation = Quaternion.Euler(_cinemachineTargetPitch * -1, 0.0f, 0.0f);    // �𵨸��� �ݴ�� ���� �޾Ƽ� -1�� ����
            followCamera.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);   // �ó׸ӽ� ���� ���Ʒ� ȸ�� �����ϰ� ����. (���ϸ� �����Ǿ�����)

            // �÷��̾� �¿�� ȸ����Ű��
            transform.Rotate(Vector3.up * _rotationVelocity);
        }
    }
    private void Move()
    {
        // Ÿ�� �ӵ��� ��� ���¿� ���� �޶����� ����
        //float targetSpeed = input.dash ? dashSpeed : moveSpeed;
        float targetSpeed = moveSpeed;
        if (input.dash & 0.7 <= input.move.y)
        { targetSpeed = dashSpeed; }

        if (GameManager.instance != null && GameManager.instance.inputLock)
        { input.move = Vector2.zero; }

        // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

        // Vector2�� ==�����ڴ� �ٻ�ġ�� ����ϹǷ�, �ε��Ҽ��� ������ �߻����� �ʴ´�.
        // �Է��� ������ �ӵ��� 0���� ����
        if (input.move == Vector2.zero) targetSpeed = 0.0f;

        // �÷��̾��� ���� �ӵ�
        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = input.move.magnitude;

        // ��ǥ �ӵ����� ���� �Ǵ� ������ �Ѵ�
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // ���� �������� �ӵ� ��ȭ�� ���� ������ �ƴ� � ����� ����. ����?
            // Lerp�� T�� �����Ǿ������Ƿ� �ӵ��� ���� ������ �ʿ䰡 ���ٰ��Ѵ�. T�� Time.deltaTime * speedChangeRate
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

            // �ӵ��� �Ҽ��� ���� 3�ڸ��� �ݿø�
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }
        // �÷��̾� �̵� �ִϸ��̼� ���� �ӵ�
        animMoveSpeed = Mathf.Lerp(animMoveSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
        if (animMoveSpeed < 0.01f) animMoveSpeed = 0f;


        // �Է� ���� ����ȭ
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        // move �Է��� �ִ� ��� �÷��̾ ������ �� �÷��̾� ȸ��
        if (input.move != Vector2.zero)
        {
            // �̵�
            inputDirection = transform.right * input.move.x + transform.forward * input.move.y;
        }


        // �÷��̾� ��ġ �̵�
        controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        ProgressStepCycle(moveSpeed);

    }
    // ���ڱ� �Ҹ�
    private void ProgressStepCycle(float speed)
    {
        if (controller.velocity.sqrMagnitude > 0 && (input.move.x != 0 || input.move.y != 0))
        {
            m_StepCycle += (controller.velocity.magnitude + (speed * (input.dash ? 0.3f : 0))) *
                         Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + 5;

        PlayFootStepAudio();
    }
    // ���ڱ� �Ҹ� ���
    private void PlayFootStepAudio()
    {
        if (!controller.isGrounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }
    // ī�޶� ���� ����
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    // �ٴڰ� ��Ҵ��� Ȯ���� �� �ִ� �����
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // ����� �׸���
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
    }

    //// �ִϸ��̼�
    public void ActiveAnimation()
    {
        if (fpsAnimator != null)
        {
            //// �ȱ� �ִϸ��̼� ����
            if (input.move.x != 0 || input.move.y != 0)
            {
                fpsAnimator.SetBool("isWalk", true);
            }
            else
            {
                fpsAnimator.SetBool("isWalk", false);
            }
            fpsAnimator.SetBool("isRun", input.dash);
            if (isGrounded)
            {
                fpsAnimator.SetBool("isGrounded", isGrounded);
            }
            else if (!isGrounded)
            {
                fpsAnimator.SetBool("isGrounded", isGrounded);
            }
        }
        tpsAnimator.SetBool("isJump", input.jump);

        if (isGrounded)
        {
            tpsAnimator.SetBool("isGrounded", isGrounded);
        }
        else if (!isGrounded)
        {
            tpsAnimator.SetBool("isGrounded", isGrounded);
        }

        tpsAnimator.SetFloat("Speed", animMoveSpeed);
    }
    // ���� UI ������Ʈ
    public void ShopUIUpdate()
    {

        // �������� �Ÿ� ����ϱ�
        float shopDistance = Mathf.FloorToInt(Vector3.Distance(controller.transform.position, GameManager.instance.shopPosition.position));
        PlayerUIManager.instance.SetShopDistance(shopDistance); // ���� �Ÿ� ������Ʈ
        // �÷��̾��� ������⿡�� ���������� ���Ͱ� ���� ����ϱ�
        Vector3 playerForward = controller.transform.rotation * Vector3.forward;
        float shopAngle = Vector3.SignedAngle(playerForward, GameManager.instance.shopPosition.position - controller.transform.position, Vector3.up);

        if (GameManager.instance.shopPosition.position.y + 1f > controller.transform.position.y)
        { PlayerUIManager.instance.SetShopRotation(shopAngle, true); }
        else
            PlayerUIManager.instance.SetShopRotation(shopAngle, false);


    }
    [PunRPC]
    public void shopNavSp()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        //SSM 20230925 �׺� ��ȯ
        if (NavAct== false)
        {
            for (int i = 0; i < shopNavList.Count; i++)
            {
                if (!shopNavList[i].activeSelf)
                {
                    shopNavList[i].transform.position = transform.position;
                    shopNavList[i].SetActive(true);
                    NavAct = true;
                    StartCoroutine(shopNavSpTime());
                    break;
                }
            }
        }

        //SSM End
    }
    private IEnumerator shopNavSpTime()
    {
        yield return new WaitForSeconds(3);
        NavAct = false;


    }
    [PunRPC]
    private void changeMaster(int viewid)
    {

        photonView.RPC("changeRender", RpcTarget.All, viewid);
    
    }
    [PunRPC]
    private void changeRender(int viewid)
    {
        GameObject user = PhotonView.Find(viewid).gameObject;
        Transform childObject = user.transform.Find("genAssault_LOD0");
        Renderer renderer = childObject.GetComponent<Renderer>();
        Material newMaterial = new Material(Shader.Find("Standard"));
        newMaterial.color = Color.blue;
        renderer.material = newMaterial;
    }
}
