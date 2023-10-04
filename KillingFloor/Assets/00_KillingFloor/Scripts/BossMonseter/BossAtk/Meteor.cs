using Photon.Pun;
using UnityEngine;

public class Meteor : MonoBehaviourPun
{
    private GameObject[] targetPlayer;
    private Transform orgPos;
    private float MeteorHP = 250;
    private GameObject meteorsField;
    private bool fildchk = false;
  
    private ParticleSystem[] meteorsParticle;//�극�� ��ƼŬ �迭
                                             // Start is called before the first frame update

    void Start()
    {
       
        targetPlayer = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(MeteorHP + " ," + targetPlayer);
        MeteorHP = MeteorHP + (250 * targetPlayer.Length);
     
        meteorsField = GameObject.Find("FireBreathField");
      
        meteorsParticle = new ParticleSystem[5];

        

        for (int i = 0; i < 4; i++) // ���׿���ƼŬ �迭�� �����ϴ°���
        {
            meteorsParticle[i] = meteorsField.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();

        }

       
        meteorsField.SetActive(false);

        orgPos = meteorsField.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if(MeteorHP < 0)
        {
            //���� ��ġ�� �̵�
            transform.position = new Vector3(orgPos.transform.position.x , orgPos.transform.position.y + 17.9f, orgPos.transform.position.z);
            gameObject.SetActive(false);
            return;
        }

      
        if (transform.position.y < orgPos.transform.position.y)
        {

            fildchk = true;
            
       
            transform.position = new Vector3(orgPos.transform.position.x, orgPos.transform.position.y + 17.9f, orgPos.transform.position.z);
            gameObject.SetActive(false);
            for (int i = 0; i < 4; i++)
            {
                meteorsField.SetActive(true);

            }
            for (int i = 0; i < 4; i++)
            {
                meteorsParticle[i].Play();
            }
        }
        else
        {
            transform.Translate(Vector3.down * 2 * Time.deltaTime);
            if(!fildchk)
            {
               
                meteorsField.SetActive(false);
                fildchk = true;
                
            }
          
        }

        

     



    }
    // ������ 
    public void OnDamage(float dam)
    {
        
        // �����Ϳ��� ������ ��� ��û
        Debug.Log("���׿� ������ ����û" + MeteorHP);

        photonView.RPC("MasterDamage", RpcTarget.MasterClient, dam);
    }
    // 2.�����Ͱ� ������ ����� ��û�ް� ����� ���� ���ش�.
    // ����� ���� ���� ��ο��� �����ش�.
    [PunRPC]
    public void MasterDamage(float _destroyCount)
    {
        Debug.Log("������ ��ο��� ������ ������Ʈ ��û");
        MeteorHP -= _destroyCount;
     

        // �����Ͱ� ����� �� ����
        photonView.RPC("SyncDamage", RpcTarget.All, MeteorHP);

    }
    // 3. ��δ� (�����͸� ����) ���޹��� ���� ������Ʈ�� �Ѵ�.
    [PunRPC]
    public void SyncDamage(float _destroyCount)
    {

        MeteorHP = _destroyCount;

    }
    
}
