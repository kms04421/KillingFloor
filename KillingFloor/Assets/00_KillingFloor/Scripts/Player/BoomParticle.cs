using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BoomParticle : MonoBehaviourPun
{
    private GameObject user;
    private PlayerHealth playerHealth;
    private void OnParticleCollision(GameObject other)
    {
        
            GameObject IdOBj = FindTopmostParent(gameObject.transform).gameObject;

            int viewID = IdOBj.GetComponent<Grenade>().viewId;
        if (viewID != null)
        {
            user = PhotonView.Find(viewID).gameObject;
            }
            playerHealth = user.GetComponent<PlayerHealth>();

         

        if (FindTopmostParent(other.transform).name.Contains("Zombie"))
        {
            if (other.transform.GetComponent<HitPoint>().parentObject.GetComponent<NormalZombie>().health > 0)
            {
                other.transform.GetComponent<HitPoint>().Hit(60); // ���񿡰� ������


                // ���� ���� �״´ٸ�
                if (other.transform.GetComponent<HitPoint>().parentObject.GetComponent<NormalZombie>().health <= 0)
                {
                    // ���� ���̰�
                    playerHealth.GetCoin(other.transform.GetComponent<HitPoint>().parentObject.GetComponent<NormalZombie>().coin);

                    // ���ΰ� �ʱ�ȭ
                    other.transform.GetComponent<HitPoint>().parentObject.GetComponent<NormalZombie>().coin = 0;
                    //coin += _hitObj.transform.GetComponent<HitPoint>().parentObject.GetComponent<NormalZombie>().coin;
                }
            }
          
        }
        if ("Mesh_Alfa_2".Equals(other.name)) // ���� �ϰ��
        {

            Debug.Log(other.name);

            other.gameObject.GetComponent<BossController>().OnDamage(20);

        }

        if ("DevilEye".Equals(other.name))
        {

            Debug.Log("���׿�");

            other.gameObject.GetComponent<Meteor>().OnDamage(20);

        }
        // ������ ���
        if (other.transform.GetComponent<BossController>() != null)
        {
            // ���� ������ �־���ϴ� �κ�
            //_hitObj.transform.GetComponent<BossController>().bossHp -= damage;
        }
    }
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
