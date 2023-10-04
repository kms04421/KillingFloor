using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviourPun
{

    public float damage = 20f; // ���ݷ�
    public float timeBetAttack = 0.5f; // ���� ����
    private float lastAttackTime = 0; // ������ ���� ����
    public int destroyCount;

    public bool isPoison;

    // ================== �������� �Դ� ���� ���� ================== //
    // 1. ��𼱰� ������ �޼��带 ȣ���Ѵ�.
    // �����Ϳ��� ������ ����� ��û�Ѵ�.
    public void OnDamage()
    {
        // �����Ϳ��� ������ ��� ��û
        //Debug.Log("������ ����û" + photonView.ViewID);

        photonView.RPC("MasterDamage", RpcTarget.MasterClient, destroyCount);
    }
    // 2.�����Ͱ� ������ ����� ��û�ް� ����� ���� ���ش�.
    // ����� ���� ���� ��ο��� �����ش�.
    [PunRPC]
    public void MasterDamage(int _destroyCount)
    {
        //Debug.Log("������ ��ο��� ������ ������Ʈ ��û" );

        // 5�� ������ ��û ������ �ı������ϱ����� ī��Ʈ 1 ����
        int newDestroyCount = _destroyCount + 1;
        // �����Ͱ� ����� �� ����
        photonView.RPC("SyncDamage", RpcTarget.All, newDestroyCount);

    }
    // 3. ��δ� (�����͸� ����) ���޹��� ���� ������Ʈ�� �Ѵ�.
    [PunRPC]
    public void SyncDamage(int _destroyCount)
    {
        // �����Ͱ� ����� ���� ��� �޾ƿͼ� ������Ʈ
        destroyCount = _destroyCount;

        if (5 < destroyCount)
        {
            //Debug.Log("���ӿ�����Ʈ ����");
            PhotonNetwork.Destroy(gameObject);
        }
    }
    // ================== �������� �Դ� ���� ���� ================== //

    private void OnTriggerStay(Collider other)
    {
        // �������κ��� LivingEntity Ÿ���� �������� �õ�
        LivingEntity attackTarget
            = other.GetComponent<LivingEntity>();

        // �����Ͱ� �ƴ϶�� ������ �Է� �Ұ�
        // �ֱ� ���� �������� timeBetAttack �̻� �ð��� �����ٸ� ���� ����
        if (Time.time >= lastAttackTime + timeBetAttack)
        {
            // ������ LivingEntity�� �ڽ��� ���� ����̶�� ���� ����
            if (attackTarget != null)
            {
                // ���� ���� ó���� �����Ϳ��Ը� ����
                if (PhotonNetwork.IsMasterClient)
                {// �ֱ� ���� �ð��� ����
                    lastAttackTime = Time.time;

                    // ������ �ǰ� ��ġ�� �ǰ� ������ �ٻ����� ���
                    Vector3 hitPoint = other.ClosestPoint(transform.position);
                    Vector3 hitNormal = transform.position - other.transform.position;

                    // ���� ����
                    attackTarget.OnDamage(damage, hitPoint, hitNormal);
                    if (isPoison)
                    {
                        attackTarget.OnPoison();
                    }
                }
               
            }
        }
    }
}
