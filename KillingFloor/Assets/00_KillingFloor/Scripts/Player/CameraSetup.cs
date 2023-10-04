using Cinemachine; // �ó׸ӽ� ���� �ڵ�
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

// �ó׸ӽ� ī�޶� ���� �÷��̾ �����ϵ��� ����
public class CameraSetup : MonoBehaviourPun
{
    PlayerInputs input;
    PlayerMovement playerMovement;
    GameObject fpsCam;
    GameObject tpsCam;
    GameObject inspectorCam;
    public CinemachineVirtualCamera followCam; // ���� ī�޶�
    public GameObject tpsPlayerBody;    // 3��Ī �÷��̾� �ٵ�
    public GameObject fpsPlayerBody;    // 1��Ī �÷��̾� �ٵ�
    public GameObject playerSpine;
    public bool isFPS;
    public bool tpsTest;

    void Awake()
    {
        if (photonView.IsMine)
        {
            // ī�޶� �� �ϸ鼭 ȭ�� �� �ɾ��ֱ�
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            playerMovement = GetComponent<PlayerMovement>();
            input = GetComponent<PlayerInputs>();

            // ���� �ִ� �ó� �ӽ� ���� ī�޶� ã�� �÷��̾� ������ �ֱ�
            tpsCam = GameObject.FindWithTag("TPS CAM");
            tpsCam.transform.parent = this.transform;
            fpsCam = GameObject.FindWithTag("FPS CAM");
            fpsCam.transform.parent = this.transform;
            inspectorCam = GameObject.FindWithTag("Inspector CAM");
            playerSpine = this.transform.GetChild(1).gameObject;
            playerSpine.SetActive(false);               // 3��Ī �ѵ� ���ֱ�
            tpsCam.SetActive(false);                    // 3��Ī�� �̸� ���α� (Debug��)
            inspectorCam.SetActive(false);

            followCam = fpsCam.GetComponent<CinemachineVirtualCamera>();    // FPS ī�޶� �ȷο�ķ���� ����
            playerMovement.followCamera = followCam;
            CameraSet(followCam);   // ī�޶� ����
            if (tpsTest)
            {
                TPSTest();
                Destroy(fpsPlayerBody);
            }
        }
            fpsPlayerBody.SetActive(photonView.IsMine);
            tpsPlayerBody.SetActive(!photonView.IsMine);
        
    }

    void Update()
    {
        ChangeCamera();
    }

    public void ChangeCamera()
    {
        if (photonView.IsMine)
        {
            if (input.changeCamera) // ��ư�� ������ ����
            {
                SetCamera();
                input.changeCamera = false; // ī�޶� ����Ǹ� �ٽ� �Է� ����
            }
        }
    }

    public void SetCamera()
    {
        inspectorCam.SetActive(false);
        tpsCam.SetActive(isFPS);
        fpsCam.SetActive(!isFPS);
        tpsPlayerBody.SetActive(isFPS);
        fpsPlayerBody.SetActive(!isFPS);
        playerSpine.SetActive(isFPS);


        if (isFPS) // 1��Ī�� ��
        {
            followCam = tpsCam.GetComponent<CinemachineVirtualCamera>();
        }
        else if (!isFPS) // 3��Ī�� ��
        {
            followCam = fpsCam.GetComponent<CinemachineVirtualCamera>();
        }
        CameraSet(followCam);
    }

    public void InspectorCam(GameObject _player)
    {
        tpsCam.SetActive(false);
        fpsCam.SetActive(false);
        tpsPlayerBody.SetActive(true);
        fpsPlayerBody.SetActive(false);

        inspectorCam.SetActive(true);
        followCam = inspectorCam.GetComponent<CinemachineVirtualCamera>();
        followCam.Follow = _player.GetComponent<PlayerMovement>().cinemachineCameraTarget.transform;
        followCam.LookAt = _player.GetComponent<PlayerMovement>().cinemachineCameraTarget.transform;

    }
    public void TPSTest()
    {
        tpsCam.SetActive(isFPS);
        fpsCam.SetActive(!isFPS);
        tpsPlayerBody.SetActive(isFPS);
        fpsPlayerBody.SetActive(!isFPS);

        if (isFPS) // 1��Ī�� ��
        {
            followCam = tpsCam.GetComponent<CinemachineVirtualCamera>();
        }
        else if (!isFPS) // 3��Ī�� ��
        {
            followCam = fpsCam.GetComponent<CinemachineVirtualCamera>();
        }
        CameraSet(followCam);
        input.changeCamera = false; // ī�޶� ����Ǹ� �ٽ� �Է� ����
    }
    // ī�޶� ����
    public void CameraSet(CinemachineVirtualCamera _followCam)
    {
        isFPS = !isFPS; // ������ ����

        // ���� ī�޶��� ���� ����� �ڽ��� Ʈ���������� ����
        _followCam.Follow = playerMovement.cinemachineCameraTarget.transform;

        if (!isFPS)  // ���� FPS�� LookAt�� �߰�
        { _followCam.LookAt = playerMovement.cinemachineCameraTarget.transform; }
        else
        _followCam.LookAt = null;

    }
}