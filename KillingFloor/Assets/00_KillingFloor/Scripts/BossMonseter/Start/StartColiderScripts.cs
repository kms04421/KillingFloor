using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartColiderScripts : MonoBehaviourPun
{
    private BoxCollider boxCollider; 
    private GameObject[] targetPlayer; //�÷��̾� ����Ʈ
    private List<GameObject> playersInTrigger; // �÷��̾� �ݶ��̴� ���˼� ����Ʈ�����
    private GameObject Boss; // ����������Ʈ
    private List<GameObject> bossList;
    private GameObject entrance;
    private GameObject introCanvas;
    public GameObject bossPf;
    public GameObject bossPos;
    // Start is called before the first frame update

  
    void Start()
    {

        playersInTrigger = new List<GameObject>(); // �÷��̾� �ݶ��̴� ���˼� ����Ʈ�����
        targetPlayer = GameObject.FindGameObjectsWithTag("Player");
        entrance = GameObject.Find("Entrance");
        entrance.SetActive(false);
        boxCollider = GetComponent<BoxCollider>();
    
       
   
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
   
        if (playersInTrigger.Count >= targetPlayer.Length) //�÷��̾ ��� ����� ���� Active
        {
            
                PhotonNetwork.Instantiate(bossPf.name, bossPos.transform.position, bossPos.transform.rotation);
                entrance.SetActive(true);
               
                gameObject.SetActive(false);
            
        }
      
     
    }
   


    //�������� �÷��̾�
    private void OnTriggerStay(Collider other)
    {
      
        GameObject otherGameObject = other.gameObject;

        targetPlayer = GameObject.FindGameObjectsWithTag("Player");
        // Ʈ���ſ� ������ ������ �÷��̾����� ���θ� �Ǵ�
        if (otherGameObject.CompareTag("Player"))
        {
            // �̹� ����Ʈ�� �߰����� �ʾ����� �߰�
            if (!playersInTrigger.Contains(otherGameObject))
            {

                playersInTrigger.Add(otherGameObject);
                // �÷��̾� Ʈ���� ���� �̺�Ʈ�� ó���ϰų� �ʿ��� �۾� ����
            }
        }
    }


    // ������ ���� �÷��̾�
    private void OnTriggerExit(Collider other)
    {
        
        GameObject otherGameObject = other.gameObject;

        // Ʈ���ſ��� ���� ������ �÷��̾����� ���θ� �Ǵ�
        if (otherGameObject.CompareTag("Player"))
        {
            // ����Ʈ���� ����
            if (playersInTrigger.Contains(otherGameObject))
            {
                playersInTrigger.Remove(otherGameObject);
                // �÷��̾� Ʈ���� ���� ���� �̺�Ʈ�� ó���ϰų� �ʿ��� �۾� ����
            }
        }
    }

}
