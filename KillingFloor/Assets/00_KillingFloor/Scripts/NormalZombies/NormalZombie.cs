using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;

public class NormalZombie : NormalZombieData
{
    public List<GameObject> skills = new List<GameObject>();

    private Transform skillParent;
    private Transform ammoParent;

    private Coroutine blendTreeMove;
    private Coroutine animatorController;

    private NormalNavigation navigation;

    private Animator ani;

    public GameObject skillPrefab;
    public GameObject ammoPrefab;

    public VolumeController volume;

    private float timeElapsed;

    // ���� �ִϸ��̼� ��ǥ
    private float startTime;
    private float thisBlend;
    private float coolTime;
    private float skillTime;
    public int hitPos;

    /*
    0: Left
    1: front
    2: Right
    3: Head
    */

    public bool isCoroutine = false;
    private bool isDeath = false;
    private bool isHit = false;
    private bool isSkill = false;
    private bool isZedTime = false;
    private bool isZedTimeCheck = false;

    private void Awake()
    {
        volume = GameObject.Find("Global Volume").GetComponent<VolumeController>();
        navigation = GetComponent<NormalNavigation>();
        ani = GetComponent<Animator>();
        GetComponent<NavMeshAgent>().speed = 0.1f;
        if (gameObject.name == "ZombieWalk_01(Clone)" || gameObject.name == "ZombieWalk_02(Clone)" ||
            gameObject.name == "ZombieWalk_03(Clone)" || gameObject.name == "ZombieWalk_04(Clone)" ||
            gameObject.name == "ZombieHide_01(Clone)" || gameObject.name == "ZombieHide_02(Clone)")
        {
            isSkill = true;
        }
        else
        {
            skillParent = GameObject.Find("Skills").transform;
        }

        ammoParent = GameObject.Find("Ammos").transform;
    }

    private void Update()
    {
        if (isDeath == false)
        {
            //if (health <= 0)
            //{
            //    Death();
            //}
            if (health == health / 2 && isHit == false)
            {
                isHit = true;

                Hit(hitPos);
            }
            if (isCoroutine == false)
            {
                if (navigation.isLongContact == true && isSkill == false)
                {
                    SkillStart();
                }
                else if (navigation.isContact == true && PhotonNetwork.IsMasterClient)
                {
                    Attack();
                }
                else { /*No Event*/ }
            }
        }

        //SetTransform(transform.position);
    }

    public void SkillStart()
    {
        photonView.RPC("MasterSkill", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void MasterSkill()
    {
        photonView.RPC("SyncSkill", RpcTarget.All);
    }

    [PunRPC]
    public void SyncSkill()
    {
        StartCoroutine(Skill());
    }

    public void ZombieSetting()
    {
        //blendTreeMove = StartCoroutine(BlendTreeMove(0.0f, 1.0f, 2.0f));
        ani.SetFloat("move", 1.0f);
        if (gameObject.name == "ZombieWalk_01(Clone)" || gameObject.name == "ZombieWalk_02(Clone)" ||
            gameObject.name == "ZombieWalk_03(Clone)" || gameObject.name == "ZombieWalk_04(Clone)")
        {
            int number = UnityEngine.Random.Range(0, 2);

            switch (number)
            {
                case 0:
                    (health, damage, coin) = ZombieWalk();
                    break;
                case 1:
                    (health, damage, coin) = ZombieRun();
                    break;
            }
        }
        else if (gameObject.name == "ZombieHide_01(Clone)" || gameObject.name == "ZombieHide_02(Clone)")
        {
            (health, damage, coin) = ZombieHide();
        }
        else if (gameObject.name == "ZombieSpit_01(Clone)" || gameObject.name == "ZombieSpit_02(Clone)")
        {
            (health, damage, coin) = ZombieSpit();
        }
        else if (gameObject.name == "ZombieNoise(Clone)")
        {
            (health, damage, coin) = ZombieNoise();
        }
    }

    private IEnumerator BlendTreeMove(float startBlend, float endBlend, float duration)
    {
        isCoroutine = true;

        if (!(thisBlend == endBlend))
        {
            timeElapsed = 0.0f;

            thisBlend = ani.GetFloat("move");

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;

                float time = Mathf.Clamp01(timeElapsed / duration);

                ani.SetFloat("move", Mathf.Lerp(startBlend, endBlend, time));

                yield return null;
            }
        }

        isCoroutine = false;
    }

    private IEnumerator AnimatorController(string checkName)
    {
        isCoroutine = true;
        ani.SetBool(checkName, true);

        yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length);

        ani.SetBool(checkName, false);
        isCoroutine = false;
    }

    private IEnumerator CoolTime(float time)
    {
        isSkill = true;

        coolTime = 0.0f;

        while (coolTime <= time)
        {
            coolTime += Time.deltaTime;

            yield return null;
        }

        isSkill = false;
    }

    private IEnumerator SkillTime(GameObject skill, float time)
    {
        skillTime = 0.0f;

        while (skillTime <= time)
        {
            skillTime += Time.deltaTime;

            yield return null;
        }

        skill.SetActive(false);
    }

    private void Attack()
    {
        int number = UnityEngine.Random.Range(0, 2);

        switch (number)
        {
            case 0:
                AniStart("atk01");
                //animatorController = StartCoroutine(AnimatorController("atk01"));
                break;
            case 1:
                AniStart("atk02");
                //animatorController = StartCoroutine(AnimatorController("atk02"));
                break;
        }
    }

    public void Hit(int number)
    {
        //if (blendTreeMove != null) { StopCoroutine(blendTreeMove); }
        if (animatorController != null) { StopCoroutine(animatorController); }

        switch (number)
        {
            case 0: // ����
                ani.SetFloat("hitPosX", 0.0f);
                ani.SetFloat("hitPosY", -1.0f);
                break;
            case 1: // ����
                ani.SetFloat("hitPosX", -1.0f);
                ani.SetFloat("hitPosY", 0.0f);
                break;
            case 2: // ������
                ani.SetFloat("hitPosX", 0.0f);
                ani.SetFloat("hitPosY", 1.0f);
                break;
            case 3: // �Ӹ�
                ani.SetFloat("hitPosX", 1.0f);
                ani.SetFloat("hitPosY", 0.0f);
                break;
        }

        animatorController = StartCoroutine(AnimatorController("isHit"));
    }

    public IEnumerator Skill()
    {
        StartCoroutine(CoolTime(10.0f));

        AniStart("atkScreaming");
        //animatorController = StartCoroutine(AnimatorController("atkScreaming"));

        while (true)
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Screaming") == true)
            {
                break;
            }

            yield return null;
        }

        if (skillPrefab.name == "NoiseEffect")
        {
            SkillSave("Noise", new Vector3(0.0f, 0.0f, 0.0f));
        }
        else if (skillPrefab.name == "SpitEffect")
        {
            SkillSave("Spit", new Vector3(0.0f, 1.0f, 0.0f));
        }

        //if (skillPrefab.name == "NoiseEffect")
        //{
        //    if (skillParent.childCount == 0)
        //    {
        //        GameObject newSkill = Instantiate(skillPrefab, skillParent);
        //        skills.Add(newSkill);
        //        newSkill.transform.position = gameObject.transform.position;
        //        newSkill.transform.rotation = gameObject.transform.rotation;
        //        StartCoroutine(SkillTime(newSkill, 5.0f));
        //    }
        //    else
        //    {
        //        for (int i = 0; i < skillParent.childCount; i++)
        //        {
        //            if (skillParent.GetChild(i).gameObject.activeSelf)
        //            {
        //                if (i == skillParent.childCount - 1)
        //                {
        //                    GameObject newSkill = Instantiate(skillPrefab, skillParent);
        //                    skills.Add(newSkill);
        //                    newSkill.transform.position = gameObject.transform.position;
        //                    newSkill.transform.rotation = gameObject.transform.rotation;
        //                    StartCoroutine(SkillTime(newSkill, 5.0f));

        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                skillParent.GetChild(i).gameObject.SetActive(true);
        //                skillParent.GetChild(i).gameObject.transform.position = gameObject.transform.position;
        //                skillParent.GetChild(i).gameObject.transform.rotation = gameObject.transform.rotation;
        //                StartCoroutine(SkillTime(skillParent.GetChild(i).gameObject, 5.0f));
        //            }
        //        }
        //    }
        //}
        //else if (skillPrefab.name == "SpitEffect")
        //{
        //    if (skillParent.childCount == 0)
        //    {
        //        GameObject newSkill = Instantiate(skillPrefab, skillParent);
        //        skills.Add(newSkill);
        //        newSkill.transform.position = gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        //        newSkill.transform.rotation = gameObject.transform.rotation;
        //    }
        //    else
        //    {
        //        for (int i = 0; i < skillParent.childCount; i++)
        //        {
        //            if (skillParent.GetChild(i).gameObject.activeSelf)
        //            {
        //                if (i == skillParent.childCount - 1)
        //                {
        //                    GameObject newSkill = Instantiate(skillPrefab, skillParent);
        //                    skills.Add(newSkill);
        //                    newSkill.transform.position = gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        //                    newSkill.transform.rotation = gameObject.transform.rotation;
        //                    StartCoroutine(SkillTime(newSkill, 5.0f));

        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                skillParent.GetChild(i).gameObject.SetActive(true);
        //                skillParent.GetChild(i).gameObject.transform.position = gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        //                skillParent.GetChild(i).gameObject.transform.rotation = gameObject.transform.rotation;
        //                StartCoroutine(SkillTime(skillParent.GetChild(i).gameObject, 5.0f));
        //            }
        //        }
        //    }
        //}

        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    if (transform.GetChild(i).name == "Skill")
        //    {
        //        if (transform.GetChild(i).childCount == 0)
        //        {
        //            GameObject newSkill = Instantiate(skillPrefab, transform.GetChild(i));

        //            if (skillPrefab.name == "NoiseEffect")
        //            {
        //                newSkill.transform.position = gameObject.transform.position;
        //            }
        //            else if (skillPrefab.name == "SpitEffect")
        //            {
        //                newSkill.transform.position = gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        //            }
        //            StartCoroutine(SkillTime(newSkill, 5.0f));
        //        }
        //        else if (transform.GetChild(i).GetChild(0).gameObject.activeSelf == false)
        //        {
        //            transform.GetChild(i).GetChild(0).gameObject.SetActive(true);

        //            if (skillPrefab.name == "NoiseEffect")
        //            {
        //                transform.GetChild(i).GetChild(0).position = gameObject.transform.position;
        //            }
        //            else if (skillPrefab.name == "SpitEffect")
        //            {
        //                transform.GetChild(i).GetChild(0).position = gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f);
        //            }

        //            StartCoroutine(SkillTime(transform.GetChild(i).GetChild(0).gameObject, 5.0f));
        //        }
        //    }
        //}
    }

    private void AniStart(string ani)
    {
        photonView.RPC("SyncAni", RpcTarget.All, ani);
    }

    [PunRPC]
    private void SyncAni(string ani)
    {
        // �޾ƿ��� ��Ʈ���� ���� �ڷ�ƾ�� �ٸ��� ��������ֱ� 
        animatorController = StartCoroutine(AnimatorController(ani));
        //switch (ani)
        //{
        //    case "atk01":
        //        animatorController = StartCoroutine(AnimatorController("atk01"));
        //    break;
        //    case "atk02":
        //        animatorController = StartCoroutine(AnimatorController("atk02"));
        //        break;
        //    case "atkScreaming":
        //        animatorController = StartCoroutine(AnimatorController("atkScreaming"));
        //        break;
        //}
    }

    private void SkillSave(string _name, Vector3 _addVector)
    {
        for (int i = 0; i < skillParent.childCount; i++)
        {
            if (skillParent.GetChild(i).name == _name)
            {
                if (skillParent.GetChild(i).childCount == 0)
                {
                    GameObject newSkill = Instantiate(skillPrefab, skillParent.GetChild(i));
                    newSkill.transform.position = gameObject.transform.position + _addVector;
                    newSkill.transform.rotation = gameObject.transform.rotation;
                    StartCoroutine(SkillTime(newSkill, 5.0f));
                }
                else
                {
                    for (int j = 0; j < skillParent.GetChild(i).childCount; j++)
                    {
                        if (skillParent.GetChild(i).GetChild(j).gameObject.activeSelf)
                        {
                            if (j == skillParent.GetChild(i).childCount - 1)
                            {
                                GameObject newSkill = Instantiate(skillPrefab, skillParent.GetChild(i));
                                newSkill.transform.position = gameObject.transform.position + _addVector;
                                newSkill.transform.rotation = gameObject.transform.rotation;
                                StartCoroutine(SkillTime(newSkill, 5.0f));

                                break;
                            }
                        }
                        else
                        {
                            skillParent.GetChild(i).GetChild(j).gameObject.SetActive(true);
                            skillParent.GetChild(i).GetChild(j).gameObject.transform.position = gameObject.transform.position + _addVector;
                            skillParent.GetChild(i).GetChild(j).gameObject.transform.rotation = gameObject.transform.rotation;
                            StartCoroutine(SkillTime(skillParent.GetChild(i).GetChild(j).gameObject, 5.0f));

                            break;
                        }
                    }
                }
            }
        }
    }

    public void Death()
    {
        ani.SetTrigger("isDie");

        isDeath = true;

        StartCoroutine(DeathEnd());
    }

    private IEnumerator DeathEnd()
    {
        int num_Zed = UnityEngine.Random.Range(0, 100);
        if (0 <= num_Zed && num_Zed < 5)
        {
            GameManager.instance.isZedTime = true;

            if (PhotonNetwork.IsMasterClient)
            {
                volume.isZedTime = true;
            }
        }   // if: ����Ÿ�� �߻�

        int num_Ammo = UnityEngine.Random.Range(0, 100);
        if (0 <= num_Ammo && num_Ammo < 5)
        {
            GameObject newAmmo = Instantiate(ammoPrefab, ammoParent);
            newAmmo.transform.position = gameObject.transform.position;
        }   // if: �Ѿ� ����

        while (ani.GetNextAnimatorStateInfo(0).IsName("Dead") == false)
        {
            yield return null;
        }


        yield return new WaitForSeconds(3);

        Destroy(gameObject);

        //gameObject.SetActive(false);
    }

    protected virtual (float, float, int) ZombieWalk()
    {
        return base.ZombieWalk(health, damage, coin);
    }

    protected virtual (float, float, int) ZombieRun()
    {
        return base.ZombieRun(health, damage, coin);
    }

    protected virtual (float, float, int) ZombieSpit()
    {
        return base.ZombieSpit(health, damage, coin);
    }

    protected virtual (float, float, int) ZombieHide()
    {
        return base.ZombieHide(health, damage, coin);
    }

    protected virtual (float, float, int) ZombieNoise()
    {
        return base.ZombieNoise(health, damage, coin);
    }

    // ================== �������� �Դ� ���� ���� ================== //
    // 1. ��𼱰� ������ �޼��带 ȣ���Ѵ�.
    // �����Ϳ��� ������ ����� ��û�Ѵ�.
    public void Hit(float _damage)
    {
        // �����Ϳ��� ������ ��� ��û
        photonView.RPC("MasterDamage", RpcTarget.MasterClient, _damage);
    }
    // 2.�����Ͱ� ������ ����� ��û�ް� ����� ���� ���ش�.
    // ����� ���� ���� ��ο��� �����ش�.
    [PunRPC]
    public void MasterDamage(float _destroyCount)
    {
        // 5�� ������ ��û ������ �ı������ϱ����� ī��Ʈ 1 ����
        //int newDestroyCount = _destroyCount + 1;

        float newHealth = health - _destroyCount;

        // �����Ͱ� ����� �� ����
        photonView.RPC("SyncDamage", RpcTarget.All, newHealth);

    }
    // 3. ��δ� (�����͸� ����) ���޹��� ���� ������Ʈ�� �Ѵ�.
    [PunRPC]
    public void SyncDamage(float _destroyCount)
    {
        // �����Ͱ� ����� ���� ��� �޾ƿͼ� ������Ʈ
        health = _destroyCount;

        if (health <= 0)
        {
            Death();
        }
    }
    // ================== �������� �Դ� ���� ���� ================== //

    // ================== ���� ��ġ�� ���� ================== //
    // �����Ϳ��� ���� ��ġ�� ������ ��û�Ѵ�.
    //public void SetTransform(Vector3 _position)
    //{
    //    // �����Ϳ��� ������ ��� ��û
    //    photonView.RPC("MasterTransform", RpcTarget.MasterClient, _position);
    //}
    //// 2.�����Ͱ� ������ ����� ��û�ް� ����� ���� ���ش�.
    //// ����� ���� ���� ��ο��� �����ش�.
    //[PunRPC]
    //public void MasterTransform(Vector3 _position)
    //{
    //    transform.position = _position;

    //    // �����Ͱ� ����� �� ����
    //    photonView.RPC("SyncTransform", RpcTarget.All, _position);
    //}
    //// 3. ��δ� (�����͸� ����) ���޹��� ���� ������Ʈ�� �Ѵ�.
    //[PunRPC]
    //public void SyncTransform(Vector3 _position)
    //{
    //    // �����Ͱ� ����� ���� ��� �޾ƿͼ� ������Ʈ
    //    transform.position = _position;
    //}
    // ================== ���� ��ġ�� ���� ================== //

    //Legacy:
    //private IEnumerator AnimationCoroutine(float startBlend, float endBlend, float duration, bool isBlend, string checkName)
    //{
    //    isCoroutine = true;

    //    if (isBlend)
    //    {
    //        if (!(thisBlend == endBlend))
    //        {
    //            timeElapsed = 0.0f;

    //            thisBlend = ani.GetFloat("move");

    //            while (timeElapsed < duration)
    //            {
    //                timeElapsed += Time.deltaTime;

    //                float time = Mathf.Clamp01(timeElapsed / duration);

    //                ani.SetFloat("move", Mathf.Lerp(startBlend, endBlend, time));

    //                yield return null;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        ani.SetBool(checkName, true);

    //        float clipTime = ani.GetCurrentAnimatorClipInfo(0).Length;

    //        yield return new WaitForSeconds(clipTime);

    //        ani.SetBool(checkName, false);
    //    }

    //    isCoroutine = false;
    //}
}
