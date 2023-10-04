using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerShooter : MonoBehaviourPun
{
    public enum State
    {
        Ready, // �߻� �غ��
        Empty, // źâ�� ��
        Reloading // ������ ��
    }
    //ssm ��ź �߻� ���� //

    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private GameObject bulletPoint;
    [SerializeField]
    private float bulletSpeed = 600;
    [SerializeField]
    private List<GameObject> bulletlist;

    //ssm ��ź ��
    public State state { get; private set; }
    public enum Type { Pistol, Rifle, Melee, Heal };
    private PlayerInputs input;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private CameraSetup cameraSet;
    private BloodEffect bloodFX;
    protected Animator animator;
    int layerMask = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 11) | (1 << 14);    // ������ ���� ������ ���̾� ����ũ


    public Transform aimTarget; // �÷��̾ ���� ����
    public Transform targetObj;                // �÷��̾� ����
    public Transform weaponPosition = null;    // ���� ��ġ ������
    public Transform rightHandPosition; // ������ ��ġ

    // ����
    public enum WeaponClass { Commando, Demolitionist };    // ���� �߰��ɰ�� ���⿡ �߰�
    public WeaponClass weaponClass;                         // ���� ���� ����

    [Header("Weapon Info")]
    public Weapon equipedWeapon;
    public Weapon[] weaponList;
    [Range(1, 5)]
    public int weaponSlot = 1;
    public float damage;        // �ѱ� ������
    public float range = 100f;  // ��Ÿ�
    public float reloadRate;    // ������ �ӵ�
    public float fireRate;      // ��� �ӵ�
    public float lastFireTime;  // ������ ��ݽð�
    public float healCoolDown = 15f;  // �� ��ٿ�

    [Header("Effects")]

    public ParticleSystem muzzleFlashEffect; // �ѱ� ȭ��ȿ��
    public ParticleSystem shellEjectEffect;  // ź�� ���� ȿ��
    public AudioSource gunAudioPlayer;       // �� �Ҹ� �����

    public Transform fireTransform;          // �Ѿ��� �߻�� ��ġ
    public ParticleSystem bulletHole;            // �Ѿ��� �´� ���� �����Ǵ� ��ƼŬ
    public GameObject bloodParticle;            // �Ѿ��� �´� ���� �����Ǵ� ��ƼŬ
    public LineRenderer bulletLineRenderer;  // �Ѿ� ������ �׸��� ���� ������
    public ParticleSystem fireParticle;
    public bool isParticleTrigger;          // ��ƼŬ �������� Ʈ����
    public bool isBloodTrigger;             // ����ȿ�� ���� Ʈ����

    [Header("Grenade")]
    public int grenade;         // ����ź ����
    public bool isGrenade;      // ����ź ���� üũ (1��Ī �ִϸ��̼�)
    public GameObject grenadePrefab;  // ����ź ������ (Resources ����)
    public Transform throwPosition;   // ������ ������
    public float grenadePower;        // ����ź ������ ��


    [Header("TPS Weapon")]
    public Weapon tpsPistol;    // ������ ���� ���� ����
    public Weapon tpsRifle;     // ������ ������ ���� ����
    public Weapon tpsMelee;     // ������ ���� ���� ����
    public Weapon tpsHeal;     // ������ ���� ���� ����
    public Weapon tpsSMG;
    public Weapon tpsSCAR;

    [Header("FPS Weapon")]
    public Transform fpsPosition;
    public Transform fpsPistol;
    public Transform fpsSMG;
    public Transform fpsRifle;
    public Transform fpsSCAR;
    public Transform fpsMelee;
    public Transform fpsHeal;
    public Transform fpsGrenade;

    public bool isSMG;
    public bool isSCAR;

    [Header("Animator IK")]
    public Animator handAnimator;
    public Transform rightHandObj = null;   // ������
    public Transform leftHandObj = null;    // �޼�
    public Transform rightElbowObj = null;   // ������ �׷�
    public Transform leftElbowObj = null;    // �޼� �׷�
    [Range(0, 1)]
    public float handIKAmount = 1;
    [Range(0, 1)]
    public float elbowIKAmount = 1;
    [Range(0, 1)]
    public float animationIKAmount = 0.5f; // �ִϸ��̼� �� IK �⺻��
    public bool ikActive = false;

    private void Awake()
    {
        switch (GameManager.instance.playerClass)
        {
            case "Commando":
                weaponClass = WeaponClass.Commando;
                break;
            case "Demolitionist":
                weaponClass = WeaponClass.Demolitionist;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletlist = new List<GameObject>();


        for (int i = 0; i < 7; i++)
        {

            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, bulletPoint.transform.forward, transform.rotation);

            bulletlist.Add(bullet);
            bulletlist[i].GetComponent<GranadeGun>()?.setViewId(photonView.ViewID);
            bulletlist[i].SetActive(false);
        }







        input = GetComponent<PlayerInputs>();
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        cameraSet = GetComponent<CameraSetup>();
        animator = GetComponent<Animator>();
        bloodFX = GetComponent<BloodEffect>();

        // ========================= ���� �������� �κ� =========================//
        // TPS ���� ��������
        tpsPistol = weaponPosition.GetChild(0).GetChild(0).GetComponent<Weapon>();
        tpsMelee = weaponPosition.GetChild(2).GetComponent<Weapon>();
        tpsHeal = weaponPosition.GetChild(3).GetComponent<Weapon>();

        


        // FPS ���� ��������
        fpsPosition = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Transform>(); // FPS Body ���ӿ�����Ʈ
        fpsPistol = fpsPosition.transform.GetChild(0).GetChild(0).GetComponent<Transform>();   // Slot1 �� ���� ��������
        fpsMelee = fpsPosition.transform.GetChild(2).GetComponent<Transform>();
        fpsHeal = fpsPosition.transform.GetChild(3).GetComponent<Transform>();
        fpsGrenade = fpsPosition.transform.GetChild(4).GetComponent<Transform>();

        // ������ ���� ������ ���Ⱑ �޶������ϴ� ���
        switch (weaponClass)
        {

            // �ڸ����� Slot2�� ù��° ����
            case WeaponClass.Commando:
                fpsRifle = fpsPosition.transform.GetChild(1).GetChild(0).GetComponent<Transform>();
                tpsRifle = weaponPosition.GetChild(1).GetChild(0).GetComponent<Weapon>();
                fpsPosition.transform.GetChild(1).GetChild(1).GetComponent<Transform>().gameObject.SetActive(false);
                weaponPosition.GetChild(1).GetChild(1).GetComponent<Weapon>().gameObject.SetActive(false);

                break;
            // ��������Ʈ�� Slot2�� �ι�° ����
            case WeaponClass.Demolitionist:

                fpsRifle = fpsPosition.transform.GetChild(1).GetChild(1).GetComponent<Transform>();
                tpsRifle = weaponPosition.GetChild(1).GetChild(1).GetComponent<Weapon>();
                fpsPosition.transform.GetChild(1).GetChild(0).GetComponent<Transform>().gameObject.SetActive(false);
                weaponPosition.GetChild(1).GetChild(0).GetComponent<Weapon>().gameObject.SetActive(false);
                break;

        }


        tpsRifle.gameObject.SetActive(false);    // �̸� ���α�
        tpsMelee.gameObject.SetActive(false);    // �̸� ���α�
        tpsHeal.gameObject.SetActive(false);    // �̸� ���α�

        fpsRifle.gameObject.SetActive(false);
        fpsMelee.gameObject.SetActive(false);
        fpsHeal.gameObject.SetActive(false);
        fpsGrenade.gameObject.SetActive(false);

        // ���Ű����� ������� �����ͼ� �̸� ���α�
        fpsSMG = fpsPosition.transform.GetChild(0).GetChild(1).GetComponent<Transform>();   // Slot1 �� ���� ��������
        fpsSCAR = fpsPosition.transform.GetChild(1).GetChild(2).GetComponent<Transform>();
        fpsSMG.gameObject.SetActive(false);
        fpsSCAR.gameObject.SetActive(false);

        tpsSMG = weaponPosition.GetChild(0).GetChild(1).GetComponent<Weapon>();
        tpsSCAR = weaponPosition.GetChild(1).GetChild(2).GetComponent<Weapon>(); // ��ī ��������
        tpsSMG.gameObject.SetActive(false);
        tpsSCAR.gameObject.SetActive(false);

        SetWeapon(tpsPistol, fpsPistol); // ���� ����
        animator.SetBool("isWeaponPistol", true);
        animator.SetBool("isWeaponRifle", false);


        lastFireTime = 0;       // �ð� �ʱ�ȭ

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) { return; } // ���� �÷��̾ �ƴ� ��� �Է��� ���� �ʴ´�.

        // �Է� ���ɿ��� Ȯ��
        if (GameManager.instance != null && !GameManager.instance.inputLock)
        {
            HandSet();
            Aim();
            Fire();
            Reload();
            WeaponInput();
            Weapons();
            Melee();
            Heal();
        }
    }

    // �ֹ��� �������� ��� �Է�
    void Fire()
    {

        if (input.shoot && weaponSlot < 3 && weaponClass == WeaponClass.Commando ||
            input.shoot && weaponSlot == 1 && weaponClass == WeaponClass.Demolitionist)
        {
            // ���� ���°� �߻� ������ ����
            // && ������ �� �߻� �������� timeBetFire �̻��� �ð��� ����
            if (state == State.Ready && Time.time >= lastFireTime + fireRate && !input.dash && 0 < equipedWeapon.ammo)
            {
                // ������ �� �߻� ������ ����
                lastFireTime = Time.time;
                // ���� �߻� ó�� ����
                Shot();
             
            }
            // ���� �Ѿ��� ���� �� �߻��ϸ� ������ ����
            else if (state == State.Empty && 0 < equipedWeapon.remainingAmmo && !input.dash)
            {
                input.shoot = false;
                input.reload = true; // ������ ��ư �����ֱ�
            }
            // ���� �Ѿ˵� ���� ��
            else if (equipedWeapon.ammo == 0 && equipedWeapon.remainingAmmo == 0 && !input.dash)
            {
                // ToDo : ƽ ���� �÷��̵ǵ��� �ϱ� (�Ѿ� ����)
                gunAudioPlayer.clip = equipedWeapon.emptyAudio;
                gunAudioPlayer.PlayOneShot(gunAudioPlayer.clip); // �ѼҸ� ���

                input.shoot = false;
            }
        }
        if (input.shoot && weaponSlot < 3 && weaponClass == WeaponClass.Demolitionist)
        {
            if (state == State.Ready && Time.time >= lastFireTime + fireRate && 0 < equipedWeapon.ammo)
            {
                lastFireTime = Time.time;
                if (weaponSlot == 2)
                {
                    photonView.RPC("GranadeActMaster", RpcTarget.MasterClient);
                    equipedWeapon.ammo -= 1;
                    PlayerUIManager.instance.SetAmmo(equipedWeapon.ammo);           // ���� ź UI ����
                }
            }
            // ��������Ʈ �߻� �Է� �� �Ұ͵�
        }
        PlayerUIManager.instance.SetCoin(playerHealth.coin);

    }
    [PunRPC]
    void GranadeActMaster()
    {
        photonView.RPC("GranadeActAll", RpcTarget.All);
    }
    [PunRPC]
    void GranadeActAll()
    {
        for (int i = 0; i < 7; i++)
        {
            if (!bulletlist[i].activeSelf)
            {


                bulletlist[i].transform.position = bulletPoint.transform.position;
                bulletlist[i].transform.rotation = bulletPoint.transform.rotation;

                bulletlist[i].GetComponent<Rigidbody>().velocity = Vector3.zero; // ���� �ӵ� �ʱ�ȭ
                bulletlist[i].SetActive(true);
                bulletlist[i].GetComponent<Rigidbody>().AddForce(bulletPoint.transform.forward * bulletSpeed * 2f);
                handAnimator.SetTrigger("isFire");
                animator.SetTrigger("isFire");
               

                if (equipedWeapon.ammo <= 0)
                {
                    // źâ�� ���� ź���� ���ٸ�, ���� ���� ���¸� Empty���� ����
                    state = State.Empty;
                    input.shoot = false;
                }
                break;
            }
        }
    }
    void Shot()
    {

        Vector3 cameraForward = cameraSet.followCam.transform.forward;
        Vector3 cameraPosition = cameraSet.followCam.transform.position;
        // ī�޶�� ���� �����������Ƿ� ī�޶󰪵� �Բ� ����
        //Debug.Log(photonView.ViewID + "�����Ϳ��� ��� ��û");


        photonView.RPC("ShotProcessOnServer", RpcTarget.MasterClient, cameraForward, cameraPosition);



        // �ִϸ��̼� �۵� 
        handAnimator.SetTrigger("isFire");
        animator.SetTrigger("isFire");
        PlayerFireCameraShake.Invoke();
        equipedWeapon.ammo -= 1;
        PlayerUIManager.instance.SetAmmo(equipedWeapon.ammo);           // ���� ź UI ����

        if (equipedWeapon.ammo <= 0)
        {
            // źâ�� ���� ź���� ���ٸ�, ���� ���� ���¸� Empty���� ����
            state = State.Empty;
            input.shoot = false;
        }
        // ���� �� �� ���� ��¦�� �ö󰡰� �ϱ�
        input.LookInput(new Vector2(0, -0.7f));
        Invoke("ResetLookInput", 0.1f);
        //input.look = Vector2.Lerp(new Vector2(0, input.look.y - 5), Vector2.zero, 1f);

    }
    // �ѱ� �ݵ� �ʱ�ȭ
    public void ResetLookInput()
    {
        input.LookInput(new Vector2(0,0));

    }

    // �����Ͱ� �����ϴ� ���� �߻� ó��
    [PunRPC]
    private void ShotProcessOnServer(Vector3 _cameraForward, Vector3 _cameraPosition)
    {

        // ToDo : ��Ʈ�� �ۿ��� �����������. ��Ʈ����Ʈ�� �޾ƿ;��� ������ ȿ�� ���� ����
        // ����ĳ��Ʈ�� ���� �浹 ������ �����ϴ� ��������
        RaycastHit hit;
        Vector3 hitPoint = _cameraForward * range;
        GameObject hitObj = null;

        // ����ȿ���� ����� ����
        float angle = 0;
        // ����ȿ���� ���� ������Ʈ
        Transform hitTransformRoot = null;

        // ��� IK Ǯ���ֱ�
        handIKAmount = animationIKAmount;
        elbowIKAmount = animationIKAmount;
        StartCoroutine(ShootCoroutine());

        // ���� ������� ������ ������
        if (Physics.Raycast(_cameraPosition, _cameraForward, out hit, range, layerMask))
        {
            // ���� ȿ���� ���� ������Ʈ �����ֱ�
            hitTransformRoot = hit.transform.root;
            // ����ȿ���� ����� ����
            angle = Mathf.Atan2(hit.normal.x, hit.normal.z) * Mathf.Rad2Deg + 180;
            hitObj = hit.transform.gameObject;
            hitPoint = hit.point;
            isBloodTrigger = true;  // ����ȿ�� Ʈ����
        }

        // ���� ������ ������ �װ��� ��Ʈ����Ʈ��
        else if (Physics.Raycast(_cameraPosition, _cameraForward, out hit, range))
        {
            hitPoint = hit.point;
            isParticleTrigger = true;
        }

        // �߻�ó���� �����Ϳ��� ����
        if (hitObj != null)
        {
            // �ε�ģ ������Ʈ�� �ִٸ� ��ο��� ������ ����
            Damage(hitObj);
        }


        Vector3 hitNormal = hit.normal;
        int viewID = 999999;    // null ���� �˱����� ������ ����

        //Debug.Log("������� ��ҳ�? " + hitTransformRoot);

        if (hitTransformRoot != null)
        {
            var nearestBone = GetNearestObject(hitTransformRoot, hitPoint);
            if (nearestBone)
                //Debug.Log("���� �ֳ�? : " + nearestBone);

                if (nearestBone.gameObject.GetPhotonView() == null)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        //Debug.Log("����䰡 ����. ������ ��ȯ : " + nearestBone);
                        nearestBone = nearestBone.parent;
                        if (nearestBone.gameObject.GetPhotonView() != null)
                        {
                            break;
                        }
                    }
                }
            if (nearestBone != null && nearestBone.gameObject.GetPhotonView() != null)
            {

                viewID = nearestBone.gameObject.GetPhotonView().ViewID;
                //Debug.Log("ViewID �ο� : " + viewID);
            }
        }
        // ����Ʈ ��� �ڷ�ƾ�� ����
        photonView.RPC("ShotEffectProcessOnClients", RpcTarget.All, hitPoint, hitNormal, angle, viewID, isBloodTrigger, isParticleTrigger);

    }

    // ����ĳ��Ʈ�� ���� ���� ��Ʈ ����Ʈ���� ���� ����� ���� ã�� �Լ�
    Transform GetNearestObject(Transform hit, Vector3 hitPos)
    {
        var closestPos = 100f;
        Transform closestBone = null;
        var childs = hit.GetComponentsInChildren<Transform>();

        foreach (var child in childs)
        {
            var dist = Vector3.Distance(child.position, hitPos);
            if (dist < closestPos)
            {
                closestPos = dist;
                closestBone = child;
            }
        }

        var distRoot = Vector3.Distance(hit.position, hitPos);
        if (distRoot < closestPos)
        {
            closestPos = distRoot;
            closestBone = hit;
        }
        return closestBone;
    }

    // ����Ʈ ��� �ڷ�ƾ
    [PunRPC]
    private void ShotEffectProcessOnClients(Vector3 hitPosition, Vector3 _hitNormal, float _angle, int _viewID, bool _blood, bool _bullet)
    {
        StartCoroutine(ShotEffect(hitPosition, _hitNormal, _angle, _viewID, _blood, _bullet));
    }
    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� �Ѿ� ������ �׸���.
    private IEnumerator ShotEffect(Vector3 _hitPosition, Vector3 _hitNormal, float _angle, int _viewID, bool _blood, bool _bullet)
    {
        //Debug.Log("��� ����Ʈ�� �����Ѵ�.");

        if (_bullet)
        {
            //Debug.Log("�Ѿ��ڱ� ��ƼŬ ����");

            // �Ѿ� �ڱ� ��ƼŬ ����
            ParticleSystem _bulletHoleParticle = bulletHole;
            _bulletHoleParticle.transform.position = _hitPosition;
            bulletHole.Play();
            isParticleTrigger = false;
        }
        if (_blood)
        {
            //Debug.Log("���� ����");
            bloodFX.OnBloodEffect(_hitPosition, _angle, _hitNormal, _viewID);
            isBloodTrigger = false;
        }
        fireParticle.Play();    // ��ƼŬ ���
        gunAudioPlayer.clip = equipedWeapon.gunAudio;
        gunAudioPlayer.PlayOneShot(gunAudioPlayer.clip); // �ѼҸ� ���

        // ���� �������� �ѱ��� ��ġ
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        // ���� ������ �Է����� ���� �浹 ��ġ
        bulletLineRenderer.SetPosition(1, _hitPosition);
        // ���� �������� Ȱ��ȭ�Ͽ� �Ѿ� ������ �׸���
        bulletLineRenderer.enabled = true;

        yield return new WaitForSeconds(0.01f);
        // ���η����� ��Ȱ��ȭ
        bulletLineRenderer.enabled = false;
    }

    // �÷��̾� ����Ű �Է�
    void Aim()
    {
        // ������� ���� ���� �ִϸ��̼� False
        if (input.dash)
        {
            if (weaponSlot <= 2)
            {
                handAnimator.SetBool("isAim", false);
            }
            return;
        }

        // �ֹ���, �������� ������ ���̰ų� ������� �ƴ� �� ����
        if (weaponSlot <= 2 && state == State.Ready && !input.dash)
        {
            handAnimator.SetBool("isAim", input.aim);
        }

        // �и� ��������̸� ������ ����
        if (weaponSlot == 3 && state == State.Ready && input.aim)
        {
            state = State.Reloading;
            handAnimator.SetTrigger("isAim");
            gunAudioPlayer.clip = equipedWeapon.reloadAudio;
            gunAudioPlayer.PlayOneShot(gunAudioPlayer.clip); // �������� �Ҹ� ���
            StartCoroutine(WeaponDelay(reloadRate * 1.8f));
            input.aim = false;
        }
    }
    // ��� �����̸� �ֱ����� �ڷ�ƾ
    IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(fireRate); // fireRate �� RPM
        // �ܹ� ����
        if (weaponSlot == 1 && !isSMG)
        {
            input.shoot = false;
        }
        handIKAmount = 1f;
        elbowIKAmount = 1f;
    }

    // �и� �Ǵ� ���� �����̸� �ֱ����� �ڷ�ƾ
    IEnumerator WeaponDelay(float _reloadRate)
    {
        yield return new WaitForSeconds(_reloadRate);
        state = State.Ready;
    }

    void Melee()
    {
        // ��������
        if (input.shoot && weaponSlot == 3 && state == State.Ready && !input.dash)
        {
            state = State.Reloading;
            handAnimator.SetTrigger("isFire");
            gunAudioPlayer.clip = equipedWeapon.gunAudio;
            gunAudioPlayer.PlayOneShot(gunAudioPlayer.clip); // �������� �Ҹ� ���

            StartCoroutine(WeaponDelay(reloadRate));
            input.shoot = false;
        }
    }
    [PunRPC]
    public void MeleeBlood(Vector3 _hitPosition, int _viewID)
    {
        Debug.Log("�� ȿ�� ��û " + _viewID);
        bloodFX.OnBloodEffect(_hitPosition, 0, Vector3.zero, _viewID);
    }
    void Heal()
    {
        // �� Ŭ��
        if (input.shoot && weaponSlot == 4 && state == State.Ready && 15 <= healCoolDown && playerHealth.health != 100 && !input.dash)
        {
            state = State.Reloading;
            handAnimator.SetTrigger("isFire");
            healCoolDown = -0.1f;
            StartCoroutine(WeaponDelay(reloadRate));
            float heal = damage;
            if (heal + playerHealth.health >= 100)
            { heal -= ((heal + playerHealth.health) - 100); }
            photonView.RPC("HealProcessOnServer", RpcTarget.All, heal);
            //PlayerUIManager.instance.SetHP(playerHealth.health);
            input.shoot = false;
        }

        // ��ٿ� ������Ʈ
        if (healCoolDown <= 15)
        {
            healCoolDown += Time.deltaTime;
            PlayerUIManager.instance.SetHeal(healCoolDown);
        }
    }
    // �� ����ȭ
    [PunRPC]
    void HealProcessOnServer(float _heal)
    {
        playerHealth.RestoreHealth(_heal);
    }


    void Damage(GameObject _hitObj)
    {
        Debug.Log(_hitObj.name);

        if (_hitObj.transform.GetComponent<HitPoint>() == null && _hitObj.transform.GetComponent<PlayerDamage>() != null)
        {
            playerHealth.GetCoin(100);  // Debug ����׿� ��ȭ ȹ��
            playerHealth.ExpUp(8);
            _hitObj.transform.GetComponent<PlayerDamage>().OnDamage(); // RPC Ȯ�� ����׿�
            return;
        }


        if (!"Mesh_Alfa_2".Equals(_hitObj.transform.name) && !"DevilEye".Equals(_hitObj.transform.name))//���� �� �ƴҰ�� 
        {
            ////////////////////////////////////////////////����////////////////////


            if (_hitObj.transform.GetComponent<HitPoint>().parentObject.GetComponent<NormalZombie>().health > 0)
            {
                _hitObj.transform.GetComponent<HitPoint>().Hit(damage); // ���񿡰� ������

                // ���� ���� �״´ٸ�
                if (_hitObj.transform.GetComponent<HitPoint>().parentObject.GetComponent<NormalZombie>().health <= 0)
                {
                    // ���� ���̰�
                    playerHealth.GetCoin(_hitObj.transform.GetComponent<HitPoint>().parentObject.GetComponent<NormalZombie>().coin);
                    playerHealth.ExpUp(_hitObj.transform.GetComponent<HitPoint>().parentObject.GetComponent<NormalZombie>().coin);

                    // ���ΰ� �ʱ�ȭ
                    _hitObj.transform.GetComponent<HitPoint>().parentObject.GetComponent<NormalZombie>().coin = 0;
                    //coin += _hitObj.transform.GetComponent<HitPoint>().parentObject.GetComponent<NormalZombie>().coin;
                }
            }

            ////////////////////////////////////////////////////////////////////
        }

        if ("Mesh_Alfa_2".Equals(_hitObj.name)) // ���� �ϰ��
        {


            if (9 == _hitObj.transform.gameObject.layer)
            {

                _hitObj.gameObject.GetComponent<BossController>().OnDamage(damage);
            }
            else if (11 == _hitObj.transform.gameObject.layer)
            {

                _hitObj.gameObject.GetComponent<BossController>().OnDamage(damage * 0.5f);
            }
        }

        if ("DevilEye".Equals(_hitObj.name))
        {

            Debug.Log("���׿�");

            _hitObj.gameObject.GetComponent<Meteor>().OnDamage(damage);

        }
        // ������ ���
        if (_hitObj.transform.GetComponent<BossController>() != null)
        {
            // ���� ������ �־���ϴ� �κ�
            //_hitObj.transform.GetComponent<BossController>().bossHp -= damage;
        }

    }

    // ����
    public void Reload()
    {
        // �ֹ���, ���������� �� ���� ��ư�� �����ٸ�
        if (input.reload && weaponSlot < 3)
        {
            if (Reloading())
            {
                // �ִϸ��̼� �۵� �� ��� IK Ǯ���ֱ�
                handAnimator.SetTrigger("isReload");
                animator.SetTrigger("isReload");
                handIKAmount = animationIKAmount;
                elbowIKAmount = animationIKAmount;
                gunAudioPlayer.clip = equipedWeapon.reloadAudio;
                gunAudioPlayer.PlayOneShot(gunAudioPlayer.clip); // ������ �Ҹ� ���

                input.shoot = false;
                input.dash = false;
            }
            input.reload = false;
        }
    }
    private bool Reloading()
    {
        // �ܿ� ź�� 0���� ����, ź�� �������� �ʰ�, ���� ������ �� ����
        if (state == State.Reloading || equipedWeapon.remainingAmmo <= 0 || equipedWeapon.ammo == equipedWeapon.magazineSize)
        {
            return false;
        }
        StartCoroutine(ReloadCoroutine());
        return true;
    }
    // ���� �ڷ�ƾ
    IEnumerator ReloadCoroutine()
    {
        state = State.Reloading;

        yield return new WaitForSeconds(reloadRate);

        float currentAmmo = equipedWeapon.ammo;
        float remainingAmmo = equipedWeapon.remainingAmmo - (equipedWeapon.magazineSize - currentAmmo);

        equipedWeapon.ammo = Mathf.Min(equipedWeapon.magazineSize, equipedWeapon.ammo + equipedWeapon.remainingAmmo);   // ���� ź ����
        equipedWeapon.remainingAmmo = Mathf.Max(0, remainingAmmo);                                                      // ���� ź ����
        PlayerUIManager.instance.SetAmmo(equipedWeapon.ammo);           // ���� ź UI ����
        PlayerUIManager.instance.SetRemainingAmmo(equipedWeapon.remainingAmmo);           // ���� ź UI ����

        handIKAmount = 1f;
        elbowIKAmount = 1f;
        state = State.Ready;
    }

    // ȭ�鿡 �������� ���⸦ �����ϴ� �޼���
    public void Weapons()
    {
        // ������ ���� �ƴҶ��� ���� ����
        if (state != State.Reloading && !input.dash)
        {
            if (input.weaponSlot1)
            {
                if (weaponSlot != 1 && !isGrenade)
                {
                    weaponSlot = 1;
                    tpsRifle.gameObject.SetActive(false);
                    tpsPistol.gameObject.SetActive(true);
                    fpsRifle.gameObject.SetActive(false);
                    fpsPistol.gameObject.SetActive(true);
                    fpsMelee.gameObject.SetActive(false);
                    fpsHeal.gameObject.SetActive(false);
                    fpsGrenade.gameObject.SetActive(false);
                    SetWeapon(tpsPistol, fpsPistol); // ���� ����
                    photonView.RPC("ServerProcessWeapon", RpcTarget.Others, 0);

                    animator.SetBool("isWeaponPistol", true);
                    animator.SetBool("isWeaponRifle", false);
                }
                input.weaponSlot1 = false;
            }
            if (input.weaponSlot2)
            {
                if (weaponSlot != 2 && !isGrenade)
                {
                    weaponSlot = 2;
                    tpsPistol.gameObject.SetActive(false);
                    tpsRifle.gameObject.SetActive(true);
                    fpsPistol.gameObject.SetActive(false);
                    fpsRifle.gameObject.SetActive(true);
                    fpsMelee.gameObject.SetActive(false);
                    fpsHeal.gameObject.SetActive(false);
                    fpsGrenade.gameObject.SetActive(false);
                    SetWeapon(tpsRifle, fpsRifle); // ���� ����
                    photonView.RPC("ServerProcessWeapon", RpcTarget.Others, 1);

                    animator.SetBool("isWeaponPistol", false);
                    animator.SetBool("isWeaponRifle", true);
                }
                input.weaponSlot2 = false;
            }
            if (input.weaponSlot3)
            {
                if (weaponSlot != 3 && !isGrenade)
                {
                    weaponSlot = 3;
                    tpsPistol.gameObject.SetActive(false);
                    tpsRifle.gameObject.SetActive(false);
                    fpsPistol.gameObject.SetActive(false);
                    fpsRifle.gameObject.SetActive(false);
                    fpsMelee.gameObject.SetActive(true);
                    fpsHeal.gameObject.SetActive(false);
                    fpsGrenade.gameObject.SetActive(false);
                    SetWeapon(tpsMelee, fpsMelee); // ���� ����

                }
                input.weaponSlot3 = false;
            }
            if (input.weaponSlot4)
            {
                if (weaponSlot != 4 && !isGrenade)
                {
                    weaponSlot = 4;
                    tpsPistol.gameObject.SetActive(false);
                    tpsRifle.gameObject.SetActive(false);
                    fpsPistol.gameObject.SetActive(false);
                    fpsRifle.gameObject.SetActive(false);
                    fpsMelee.gameObject.SetActive(false);
                    fpsHeal.gameObject.SetActive(true);
                    fpsGrenade.gameObject.SetActive(false);
                    SetWeapon(tpsHeal, fpsHeal); // ���� ����

                }
                input.weaponSlot4 = false;
            }
            if (input.grenade && !isGrenade && 0 < grenade)
            {
                isGrenade = true;
                tpsPistol.gameObject.SetActive(false);
                tpsRifle.gameObject.SetActive(false);
                fpsPistol.gameObject.SetActive(false);
                fpsRifle.gameObject.SetActive(false);
                fpsMelee.gameObject.SetActive(false);
                fpsHeal.gameObject.SetActive(false);
                fpsGrenade.gameObject.SetActive(true);
                state = State.Reloading;
                StartCoroutine(Grenade());
                input.grenade = false;
            }
        }
    }

    [PunRPC]
    public void ServerProcessWeapon(int index)
    {
        Weapon _tpsWeapon = weaponList[index].GetComponent<Weapon>();
        equipedWeapon.gameObject.SetActive(false);
        _tpsWeapon.gameObject.SetActive(true);
        Debug.Log(index + "��° ���� ��ü ��û :" + _tpsWeapon);

        equipedWeapon = _tpsWeapon;
        rightHandObj = equipedWeapon.rightHandObj.transform;     // ������ ������ �׷�
        leftHandObj = equipedWeapon.leftHandObj.transform;       // ������ �޼� �׷�
        rightElbowObj = equipedWeapon.rightElbowObj.transform;   // ������ �����Ȳ�ġ
        leftElbowObj = equipedWeapon.leftElbowObj.transform;     // ������ ���Ȳ�ġ

        // ���� ���� �� ���� ���� ����
        switch ((Type)equipedWeapon.weaponType)
        {
            case Type.Pistol:
                animator.SetBool("isWeaponPistol", true);
                animator.SetBool("isWeaponRifle", false);
                equipedWeapon.weaponType = Weapon.Type.Pistol;
                break;
            case Type.Rifle:
                animator.SetBool("isWeaponRifle", true);
                animator.SetBool("isWeaponPistol", false);
                equipedWeapon.weaponType = Weapon.Type.Rifle;
                break;
        }
        animator.SetFloat("ReloadSpeed", reloadRate);
    }

    // ���� ���� �Էºκ�
    public void WeaponInput()
    {
        // ��ũ�ѷ� �޴� �Էºκ� �Է��� ������ ����
        if (input.scroll != 0 && !isGrenade)
        {
            int newSlot = weaponSlot;

            if (input.scroll > 0)
            {
                newSlot += 1;
                if (4 < newSlot) newSlot = 1;
            }
            else if (input.scroll < 0)
            {
                newSlot -= 1;
                if (0 >= newSlot) newSlot = 4;
            }

            switch (newSlot)
            {
                case 1:
                    input.weaponSlot1 = true;
                    break;
                case 2:
                    input.weaponSlot2 = true;
                    break;
                case 3:
                    input.weaponSlot3 = true;
                    break;
                case 4:
                    input.weaponSlot4 = true;
                    break;
            }

        }
    }
    IEnumerator Grenade()
    {
        yield return new WaitForSeconds(1.3f);

        GameObject newGrenade = PhotonNetwork.Instantiate(grenadePrefab.name, throwPosition.transform.position, Quaternion.identity);
        newGrenade.GetComponent<Rigidbody>().AddForce(calculateForce(), ForceMode.Impulse);
        newGrenade.GetComponent<Grenade>().setViewId(photonView.ViewID);

        yield return new WaitForSeconds(1f);

        grenade -= 1;   // ����ź ���� -1
        PlayerUIManager.instance.SetGrenade(grenade); // ����ź ���� �߰�
        isGrenade = false;
        state = State.Ready;

        // ����ź ������ ���� �������� �����ֱ�
        int slotIs = weaponSlot;
        weaponSlot = 0;

        switch (slotIs)
        {
            case 1:
                input.weaponSlot1 = true;
                break;
            case 2:
                input.weaponSlot2 = true;
                break;
            case 3:
                input.weaponSlot3 = true;
                break;
            case 4:
                input.weaponSlot4 = true;
                break;
        }
    }
    // ������ �� ���
    public Vector3 calculateForce()
    {
        return cameraSet.followCam.transform.forward * grenadePower;
    }
    public void SetWeapon(Weapon _tpsWeapon, Transform _fpsWeapon)
    {

        // ���� ���� �� TPS IK ����
        equipedWeapon = _tpsWeapon;
        rightHandObj = equipedWeapon.rightHandObj.transform;     // ������ ������ �׷�
        leftHandObj = equipedWeapon.leftHandObj.transform;       // ������ �޼� �׷�
        rightElbowObj = equipedWeapon.rightElbowObj.transform;   // ������ �����Ȳ�ġ
        leftElbowObj = equipedWeapon.leftElbowObj.transform;     // ������ ���Ȳ�ġ

        // ���� ���� �� ���� ���� ����
        switch ((Type)equipedWeapon.weaponType)
        {
            case Type.Pistol:
                animator.SetBool("isWeaponPistol", true);
                animator.SetBool("isWeaponRifle", false);
                equipedWeapon.weaponType = Weapon.Type.Pistol;
                break;
            case Type.Rifle:
                animator.SetBool("isWeaponRifle", true);
                animator.SetBool("isWeaponPistol", false);
                equipedWeapon.weaponType = Weapon.Type.Rifle;
                break;
        }
        damage = equipedWeapon.damage;
        reloadRate = equipedWeapon.reloadRate;
        fireRate = equipedWeapon.fireRate;
        animator.SetFloat("ReloadSpeed", reloadRate);

        PlayerUIManager.instance.SetAmmo(_tpsWeapon.ammo);           // ���� ź UI ����
        PlayerUIManager.instance.SetRemainingAmmo(_tpsWeapon.remainingAmmo); // ���� ���� ź UI ����
        PlayerUIManager.instance.SetGrenade(grenade);


        // FPS �ִϸ��̼ǵ� ����
        handAnimator = _fpsWeapon.GetComponent<Animator>();
        handAnimator.SetFloat("ReloadSpeed", reloadRate);
        playerMovement.fpsAnimator = handAnimator;

        // FPS ���̾� ������ �������ֱ�
        if (weaponSlot == 1 || weaponClass == WeaponClass.Commando && weaponSlot == 2)
        {
            fireTransform = _fpsWeapon.GetComponent<FireTransform>().fireTransform;
            bulletLineRenderer = _fpsWeapon.GetComponent<LineRenderer>();
            fireParticle = fireTransform.GetComponent<ParticleSystem>();

            // ����� ���� �ΰ��� ����
            bulletLineRenderer.positionCount = 2;
            // ���� �������� ��Ȱ��ȭ
        }

        else
        {
            fireTransform = null;
            bulletLineRenderer = null;
            fireParticle = null;
        }
        // ������ ���µ� üũ
        if (0 < _tpsWeapon.ammo)
        { state = State.Ready; }
        else if (0 >= _tpsWeapon.ammo)
        { state = State.Empty; }
    }

    [PunRPC]
    public void GetAmmo(int value)
    {
        equipedWeapon.remainingAmmo += value;
        PlayerUIManager.instance.SetRemainingAmmo(equipedWeapon.remainingAmmo);
    }
    void HandSet()
    {
        weaponPosition.position = targetObj.position;
        weaponPosition.rotation = targetObj.rotation;
    }

    public void BuySMG()
    {
        fpsPistol.gameObject.SetActive(false);

        fpsPistol = fpsSMG; // �Ѻ���
        tpsPistol = tpsSMG;

        isSMG = true;
        if(weaponSlot == 1)
        {
            fpsPistol.gameObject.SetActive(true);
            SetWeapon(tpsPistol, fpsPistol); // ���� ����
            photonView.RPC("ServerProcessWeapon", RpcTarget.Others, 0);
            animator.SetBool("isWeaponPistol", true);
            animator.SetBool("isWeaponRifle", false);
        }
    }
    public void BuySCAR()
    {
        fpsRifle.gameObject.SetActive(false);

        fpsRifle = fpsSCAR; // �Ѻ���
        tpsRifle = tpsSCAR;

        isSCAR = true;
        weaponClass = WeaponClass.Commando; // Ŭ������ �������ֱ�

        if (weaponSlot == 2)
        {
            fpsRifle.gameObject.SetActive(true);
            SetWeapon(tpsRifle, fpsRifle); // ���� ����
            photonView.RPC("ServerProcessWeapon", RpcTarget.Others, 1);

            animator.SetBool("isWeaponPistol", false);
            animator.SetBool("isWeaponRifle", true);
        }
    }

    // ���� IK �ִϸ��̼� ó��
    void OnAnimatorIK()
    {
        if (animator)
        {
            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {
                // �÷��̾� lookat
                if (targetObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(targetObj.position);
                }
                // ������ �׷�
                if (rightHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handIKAmount);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handIKAmount);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }
                // �޼� �׷�
                if (leftHandObj != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handIKAmount);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, handIKAmount);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
                }

                // ���� �Ȳ�ġ
                if (leftElbowObj != null)
                {
                    animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowObj.position);
                    animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, elbowIKAmount);
                }
                // ������ �Ȳ�ġ
                if (rightElbowObj != null)
                {
                    animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowObj.position);
                    animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, elbowIKAmount);
                }
            }
            // �׷��� �ƹ��͵� ���ٸ� 0
            else
            {
                animator.SetLookAtWeight(0);

                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);

                animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 0);
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0);
            }
        }
    }
    //ssm �θ� ã�� 
    private Transform FindTopmostParent(Transform currentTransform)
    {
        if (currentTransform.parent == null)
        {
            // ���� Transform�� ��Ʈ�̸� �ֻ��� �θ��̹Ƿ� ��ȯ�մϴ�.
            return currentTransform;
        }
        else
        {
            // �θ� ������ �θ��� �θ� ��������� ã���ϴ�.
            return FindTopmostParent(currentTransform.parent);
        }
    }


}