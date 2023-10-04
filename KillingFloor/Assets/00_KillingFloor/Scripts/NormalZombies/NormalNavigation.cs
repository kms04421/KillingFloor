using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NormalNavigation : MonoBehaviour
{
    private List<GameObject> targets = new List<GameObject>();  // Player�� ����Ǵ� List

    private NormalZombie normalZombie;
    private Transform Players;                                   // Player�� ����Ǵ� �θ� ������Ʈ

    private NavMeshAgent nav;                                   // �׺���̼�

    private float length = 2.0f;                                // ���� ���� �����Ÿ�
    private float longLength = 8.0f;
    private float minDistance;                                  // ���� ����� ������Ʈ�� �Ÿ�
    private int minDistanceTarget;                              // ���� ����� ������Ʈ List number

    private bool isCoroutine;                                   // �ڷ�ƾ�� �������� üũ
    public bool isContact;                                      // ��ü�� �ε������� üũ
    public bool isLongContact;

    private void Awake()
    {
        normalZombie = GetComponent<NormalZombie>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        Players = GameObject.Find("Players").transform;
    }

    private void Start()
    {
        Players = GameObject.Find("Players").transform;

        // ���� ������ Players�� �ڽ� ������Ʈ ������ŭ List�� �߰�
        if (Players.childCount != 0)
        {
            for (int i = 0; i < Players.childCount; i++)
            {
                targets.Add(Players.GetChild(i).gameObject);
            }
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (0 < normalZombie.health)
            {
                if (isCoroutine == false)
                {
                    nav.enabled = true;
                    StartCoroutine(Target());
                }
            }
            else
            {
                nav.enabled = false;
            }
        }
    }

    private IEnumerator Target()
    {
        isCoroutine = true;
        minDistance = 0.0f;     // minDistance �ʱ�ȭ
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].activeSelf == true)      // if: �ɸ��Ͱ� �׾����� Ȯ��
            {
                float distance = Vector3.Distance(GetComponent<Transform>().position, targets[i].transform.position);

                if (minDistance == 0.0f)
                {
                    minDistance = distance;
                }   // if: minDistance �� 0.0f �� ���� ����ó��
                if (minDistance != 0.0f)
                {
                    minDistance = minDistance < Mathf.Abs(distance) ? minDistance : Mathf.Abs(distance);
                    minDistanceTarget = minDistance < Mathf.Abs(distance) ? minDistanceTarget : i;
                }
            }

            yield return null;
        }
        if (Vector3.Distance(transform.position, targets[minDistanceTarget].transform.position) <= longLength)
        {
            isLongContact = true;
        }
        else
        {
            isLongContact = false;
        }
        if (Vector3.Distance(transform.position, targets[minDistanceTarget].transform.position) <= length)
        {
            isContact = true;
        }
        else
        {
            isContact = false;

            if (targets[minDistanceTarget].transform.position == null)
            {
                nav.enabled = false;
            }
            else if (targets[minDistanceTarget].transform.position != null)
            {
                if (nav.enabled == true)
                {
                    nav.SetDestination(targets[minDistanceTarget].transform.position);
                }
            }
        }

        isCoroutine = false;
    }
}
