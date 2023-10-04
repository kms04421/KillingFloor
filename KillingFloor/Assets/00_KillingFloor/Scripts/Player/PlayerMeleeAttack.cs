using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviourPun
{
    int layerMask = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 11) | (1 << 14);    // ������ ���� ������ ���̾� ����ũ

    public PlayerHealth playerHealth;
    public PlayerShooter shooter;
    public float damage = 80;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider hitObj)
    {

        // Ư�� ���̾ Ȯ��
        if (((1 << hitObj.gameObject.layer) & layerMask) != 0)
        {
            if (photonView.IsMine)
            {
                Damage(hitObj.gameObject);
            }
            var hitTransform = hitObj.transform;
            if (hitTransform.gameObject.GetPhotonView() == null)
            {
                for (int i = 0; i < 20; i++)
                {
                    //Debug.Log("����䰡 ����. ������ ��ȯ : " + hitTransform);
                    hitTransform = hitTransform.parent;
                    if (hitTransform.gameObject.GetPhotonView() != null)
                    {
                        break;
                    }
                }
            }

            photonView.RPC("MeleeBlood", RpcTarget.All, this.transform.position, hitTransform.gameObject.GetPhotonView().ViewID);

        }



    }

    private void OnTriggerExit(Collider hitObj)
    {
    }

    // TODO : PunRPC�� ������ ������ ����
    void Damage(GameObject _hitObj)
    {

        Debug.Log(_hitObj.name);

        if (_hitObj.transform.GetComponent<HitPoint>() == null && _hitObj.transform.GetComponent<PlayerDamage>() != null)
        {
            playerHealth.GetCoin(100);  // Debug ����׿� ��ȭ ȹ��
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
    }
}
