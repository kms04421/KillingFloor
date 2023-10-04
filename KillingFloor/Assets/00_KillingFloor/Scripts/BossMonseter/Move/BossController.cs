using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class BossController : MonoBehaviourPun
{
    public AudioClip intro;
    public AudioClip walk;
    public AudioClip jump;
    public AudioClip attack1Audio;
    public AudioClip attack2Audio;
    public AudioClip shoutAudio;
    public AudioClip fireBreathsAudio;
    public float bossHp = 3500f;//���� hp;

    private AudioSource audioSource;
    private GameObject[] targetPlayer;
    private int randPlayerNum;//Ÿ���÷��̾�
    private Animator animator;
    private int randomFattern;//��������
    private float currentTime = 0f;
    private float setTime = 6f;
    private int saveFattern = 0;//��������
    private GameObject[] fireBreaths;//�극�� ������Ʈ �迭
    private ParticleSystem[] fireBreathsParticle;//�극�� ��ƼŬ �迭
    private GameObject[] fireBreathHoles;//���̷� ������Ʈ �迭
    private ParticleSystem[] fireBreathHoleParticles;//���̷� ��ƼŬ �迭
    private GameObject[] midSphereEffects;//���̷� ������Ʈ ������迭
    private ParticleSystem[] midSphereEffectParticles;//���̷� ��ƼŬ ������迭
    private GameObject[] fireRings;// ÷����� ������Ʈ �迭
    private ParticleSystem[] fireRingParticles;//������� ��ƼŬ ������迭
    private GameObject[] meteors;//���׿�
    private int mereorCount = 0;
    private float[] meteorFattern;
    private NavMeshAgent agent;
    private bool dieChk = false;
    private Image Hpimage;
    private GameObject meteor;
    private GameObject bossintro;
    private bool changeBool = false;
    private bool audioChk = false;
    private bool atkChk = false;

    void Start()
    {
       
        meteors = new GameObject[4];
        meteor = GameObject.Find("Meteor");
      

      
        for (int i = 0; i <= 1; i++) // ���׿� �迭�� �����ϴ°���
        {
            meteors[i] = meteor.transform.GetChild(i).gameObject;
            meteors[i].SetActive(false);
        }
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        fireBreathHoles = new GameObject[3];
        fireBreathHoleParticles = new ParticleSystem[3];
        fireBreaths = new GameObject[4];
        fireBreathsParticle = new ParticleSystem[4];
        midSphereEffects = new GameObject[4];
        midSphereEffectParticles = new ParticleSystem[4];
        fireRings = new GameObject[4];
        fireRingParticles = new ParticleSystem[4];
        bossintro = GameObject.Find("Bossintro");//���� �����
        Hpimage = GameObject.Find("HP_Main").GetComponent<Image>();
        GameObject midSphere = GameObject.Find("MidSphereEffect");
        GameObject fireBreath = GameObject.Find("FireBreath");
        GameObject fireBreathHole = GameObject.Find("FireBreathHole");
        GameObject fireRing = GameObject.Find("FireRing");


        for (int i = 0; i <= 2; i++) // ���� ��� �迭�� �����ϴ°���
        {
            fireRings[i] = fireRing.transform.GetChild(i).gameObject;
            fireRingParticles[i] = fireRings[i].GetComponent<ParticleSystem>();
            fireRings[i].SetActive(false);
        }
        for (int i = 0; i <= 1; i++) // ������ ��ƼŬ�迭�� �����ϴ°���
        {
            fireBreathHoles[i] = fireBreathHole.transform.GetChild(i).gameObject;
            fireBreathHoleParticles[i] = fireBreathHoles[i].GetComponent<ParticleSystem>();
            fireBreathHoles[i].SetActive(false);
        }
        for (int i = 0; i <= 1; i++) // ������ ������ ��ƼŬ�迭�� �����ϴ°���
        {
            midSphereEffects[i] = midSphere.transform.GetChild(i).gameObject;
            midSphereEffectParticles[i] = midSphereEffects[i].GetComponent<ParticleSystem>();
            midSphereEffects[i].SetActive(false);
        }

        for (int i = 0; i <= 3; i++) // �극�� ��ƼŬ�迭�� �����ϴ°���
        {
            fireBreaths[i] = fireBreath.transform.GetChild(i).gameObject;
            fireBreathsParticle[i] = fireBreaths[i].GetComponent<ParticleSystem>();
            fireBreaths[i].SetActive(false);
        }

        animator = GetComponent<Animator>();
        targetPlayer = GameObject.FindGameObjectsWithTag("Player");
        if (PhotonNetwork.IsMasterClient)
        {
            randPlayerNum = Random.Range(0, targetPlayer.Length);
        }
        bossHp = bossHp + (1000 * targetPlayer.Length);

        meteorFattern = new float[] { bossHp/2, 1000, 0, -10, -10, -10 };
    }

    void Update()
    {
        if(photonView.IsMine)
        {
            if (bossintro.activeSelf)
            {
                if (!audioChk)
                {
                  
                    StartCoroutine(MasterIntro());
                    audioChk = true;
                                   
                }
            }
        }

        if (!PhotonNetwork.IsMasterClient)
        { return; }

       

        if (bossHp <= 0)
        {
            if (dieChk == false)
            {
                animator.SetTrigger("Die");//����
                dieChk = true;
            }

            return;
        }

        //Debug.Log("���� ��ġ: " + transform.position + targetPlayer[randPlayerNum].transform.position + "�׺� ��� ��ġ " + agent.destination);
        currentTime += Time.deltaTime;
        if (currentTime >= setTime)
        {
          

            if (targetPlayer[randPlayerNum].GetComponent<PlayerHealth>().dead)
            {
                changeplayerlook();
            }

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(targetPlayer[randPlayerNum].transform.position, path);
            agent.SetPath(path);
            // agent.SetDestination(targetPlayer[randPlayerNum].transform.position);

          

            if (!changeBool)
            {
                Invoke("changeplayerlook", 30f);
                changeBool = true;
            }
            if (!atkChk)
            {
                atkChk = true;
                randomFattern = Random.Range(0, 6);
                if (bossHp < meteorFattern[mereorCount])
                {
                    mereorCount++;
                    randomFattern = 6;
                }
            }
            
            Debug.Log(randomFattern);
            // �÷��̾�� ���� ������ �Ÿ��� ����մϴ�.
            float distance = Vector3.Distance(targetPlayer[randPlayerNum].transform.position, transform.position);
      
            if (distance < distanceAtk(randomFattern))//�ٰŸ� �ִϸ��̼�
            {
                atkChk = false;
                agent.isStopped = true;
                agent.updatePosition = false;
                agent.updateRotation = false;
                agent.velocity = Vector3.zero;

                if (randomFattern != saveFattern)// �������� �ߺ�üũ, �ߺ����� ����
                {
                    saveFattern = randomFattern;
                }
                else if (randomFattern == saveFattern)
                {
                    if (randomFattern != 0)
                    {
                        randomFattern = 0;
                        saveFattern = 0;
                    }
                    else
                    {
                        randomFattern = 1;
                        saveFattern = 1;
                    }
                }// �������� �ߺ�üũ, �ߺ����� ���� End
              
                
                switch (randomFattern)//���� ��������
                {
                    case 0:
                        animator.SetTrigger("Shout");//¢��
                        photonView.RPC("PartticleOn", RpcTarget.All, 2); // �������� ȣ��

                        currentTime = 0;
                        setTime = 3.6f;

                        break;
                    case 1:

                        animator.SetTrigger("Attack2");//������
                        audioSource.PlayOneShot(attack2Audio);
                        currentTime = 0;

                        setTime = 3.7f;


                        break;
                    case 2:

                        StartCoroutine(TimeAudio(intro, 1));
                        AnimatorStart("Attack1");///���

                        audioSource.PlayOneShot(attack1Audio);
                        currentTime = 0;

                        setTime = 3.7f;
                    
                        break;
                    case 3:

                        animator.SetTrigger("Attack3");//�극��
                        photonView.RPC("PartticleOn", RpcTarget.All, 1); // �������� ȣ��
                        audioSource.PlayOneShot(fireBreathsAudio);
                        currentTime = 0;

                        setTime = 5.75f;

                        break;
                    case 4://��������
                        audioSource.PlayOneShot(jump);
                        animator.SetTrigger("Jump");
                        photonView.RPC("PartticleOn", RpcTarget.All, 3); // �������� ȣ��
                        currentTime = 0;
                        setTime = 3f;
                        break;
                    case 5://��극��
                        animator.SetTrigger("Breath");
                        audioSource.PlayOneShot(fireBreathsAudio);
                        photonView.RPC("PartticleOn", RpcTarget.All, 4); // �������� ȣ��
                        currentTime = 0;

                        setTime = 5.8f;
                        break;
                    case 6://���׿�
                        animator.SetTrigger("Meteor");

                        photonView.RPC("PartticleOn", RpcTarget.All, 5); // �������� ȣ��

                        currentTime = 0;

                        setTime = 11f;
                        break;

                }


            }//���� ��������End
            else//���Ÿ� �ִϸ��̼�
            {
               
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("idle"))// �ӵ� ��ȭ
                {
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(walk);
                    }
                  
                    agent.isStopped = false;
                    agent.updatePosition = true;
                    agent.updateRotation = true;
             

                }
                animator.SetFloat("Speed", agent.speed);



            }//���Ÿ� �ִϸ��̼� end

        }

    }
    // �������� ���� 
    private int distanceAtk(int num)
    {
        int returnNum = 0;
         switch(num)
        {
            case 0:
                returnNum = 3;
                break;
            case 1:
                returnNum = 4;
                break;
            case 2:
                returnNum = 5;
                break;
            case 3:
                returnNum = 7;
                break;
            case 4:
                returnNum = 6;
                break;
            case 5:
                returnNum = 8;
                break;
            case 6:
                returnNum = 20;
                break;
        }
        return returnNum;
    }

    [PunRPC]
    private void bossMove()// ���� ���� ���� 
    {
        agent.SetDestination(targetPlayer[randPlayerNum].transform.position);
    }
    [PunRPC]
    private void PartticleOn(int num) // ��ƼŬ on/off 
    {
        switch(num)
        {
            case 1:
                Invoke("fireBreathImpt", 0.5f);
                break;
            case 2:
                for (int i = 0; i <= 1; i++)
                {
                    fireBreathHoles[i].SetActive(true);
                    fireBreathHoleParticles[i].Play();
                    midSphereEffects[i].SetActive(true);
                    if (i == 1)
                    {
                        midSphereEffectParticles[i].Play();
                        StartCoroutine(StopParticle(midSphereEffectParticles[i], 3.6f));//��ƼŬ ���� �ڷ�ƾ
                        StartCoroutine(FalseObj(midSphereEffects[0], 3.6f));//������Ʈ ���� �ڷ�ƾ
                    }
                    StartCoroutine(StopParticle(fireBreathHoleParticles[i], 3.6f));//��ƼŬ ���� �ڷ�ƾ


                }
                break;
            case 3:
                Invoke("JumpImpt", 1.1f);

                break;
            case 4:
                Invoke("fireBreathImpt", 1f);
                break;
            case 5:
                // ���� ������Ʈ�� Transform ������Ʈ ��������
                Transform myTransform = transform;

                // ���� ������Ʈ�� ���� ���� ���� ���
                Vector3 forwardDirection = myTransform.forward;

                forwardDirection = new Vector3(forwardDirection.x, forwardDirection.y + 1f, forwardDirection.z);
                // ��ġ�� �Ÿ�
                float distanceToPlace = 4.0f;

                // ������Ʈ�� ��ġ ��� (���� ������Ʈ�� ��ġ���� ���� �������� ���� �Ÿ���ŭ �̵�)
                Vector3 newPosition = myTransform.position + forwardDirection * distanceToPlace;



                meteor.transform.position = newPosition;
                meteor.SetActive(true);

                // ������Ʈ�� ��ġ�� ���� ����� ��ġ�� ����
                //   meteors[i].transform.position = newPosition;
                // ������Ʈ Ȱ��ȭ
                meteors[0].SetActive(true);

                break;
        }

    }
    private void AnimatorStart(string name)
    {
        animator.SetTrigger(name);
    }
  /*  //����� �ٶ󺸴� ���� (������)
    private void LookRotate()
    {
        if (targetPlayer[randPlayerNum] == null) // ����������
        {
            targetPlayer = GameObject.FindGameObjectsWithTag("Player");
            randPlayerNum = Random.Range(0, targetPlayer.Length);
        }

        Vector3 lookDirection = targetPlayer[randPlayerNum].transform.position - transform.position;

        lookDirection = new Vector3(lookDirection.x, 0f, lookDirection.z);
        // ����� �ٶ󺸴� ȸ����(Quaternion) ���
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

        // ���� ������Ʈ�� ȸ���� ����� �ٶ󺸴� ȸ������ ����
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 2 * Time.deltaTime);
    }
    //���� ������*/

    
    //��ƼŬ ����
    private IEnumerator StopParticle(ParticleSystem particle, float num)
    {
        yield return new WaitForSeconds(num);
        audioSource.Stop();
        particle.Stop();

    }
    //������Ʈ ��Ȱ��ȭ
    private IEnumerator FalseObj(GameObject gameObj, float num)
    {
        yield return new WaitForSeconds(num);
        gameObj.SetActive(false);

    }
    
    //���� ���
    private void JumpImpt()
    {
        for (int i = 0; i <= 2; i++)
        {
            fireRings[i].SetActive(true);
            fireRingParticles[i].Play();
            StartCoroutine(StopParticle(fireRingParticles[i], 1f));
        }

    }

    private void fireBreathImpt()
    {
        for (int i = 0; i <= 2; i++)
        {
            fireBreaths[i].SetActive(true);
            fireBreathsParticle[i].Play();
            StartCoroutine(StopParticle(fireBreathsParticle[i], 4f));
        }
    }
    private IEnumerator MasterIntro()
    {
        yield return new WaitForSeconds(4);
        int id = bossintro.GetComponent<PhotonView>().ViewID;

        photonView.RPC("bossIntroTime", RpcTarget.MasterClient, id); // �������� ȣ��
      
       
    }

    [PunRPC]
    //���� ��Ʈ��
    private void bossIntroTime(int id)
    {

        photonView.RPC("IntroDel", RpcTarget.All, id); // �������� ȣ��
    }

    [PunRPC]
    private void IntroDel(int id )
    {
        // Photon View ID�� ����Ͽ� ������Ʈ ã��
        GameObject targetObject = PhotonView.Find(id)?.gameObject;

        targetObject.SetActive(false);
    }
    public void bossHit(float dam)
    {

        Hpimage.fillAmount = normalization();
        bossHp -= dam;
    }
    // ����ȭ
    public float normalization()
    {
        float normalizedHealth = (bossHp - 0) / (3500f + (1000f * targetPlayer.Length) - 0);
        return normalizedHealth;
    }


    // Ÿ�ٺ���
    public void changeplayerlook()
    {
       
        List<int> playerNumber = new List<int>();
        targetPlayer = GameObject.FindGameObjectsWithTag("Player");
        for(int i=0; i < targetPlayer.Length; i++)
        {            
            if(!targetPlayer[i].GetComponent<PlayerHealth>().dead)
            {
                playerNumber.Add(i);
            }
        }
        int targetNumber = Random.Range(0, playerNumber.Count);
        randPlayerNum = playerNumber[targetNumber];
        changeBool = false;
        photonView.RPC("bossMove", RpcTarget.All); // �������� ȣ��
    }

    private IEnumerator TimeAudio(AudioClip audio ,float time)
    {
        yield return new WaitForSeconds(time);
        audioSource.PlayOneShot(audio);
    }
    // ������ 
    public void OnDamage(float dam)
    {
        // �����Ϳ��� ������ ��� ��û
      

        photonView.RPC("MasterDamage", RpcTarget.MasterClient, dam);
    }
    // 2.�����Ͱ� ������ ����� ��û�ް� ����� ���� ���ش�.
    // ����� ���� ���� ��ο��� �����ش�.
    [PunRPC]
    public void MasterDamage(float _destroyCount)
    {
     

        bossHp -= _destroyCount;
      
        // �����Ͱ� ����� �� ����
        photonView.RPC("SyncDamage", RpcTarget.All, bossHp);

    }
    // 3. ��δ� (�����͸� ����) ���޹��� ���� ������Ʈ�� �Ѵ�.
    [PunRPC]
    public void SyncDamage(float _destroyCount)
    {  
        Hpimage.fillAmount = normalization();
        bossHp = _destroyCount;

    }
}
