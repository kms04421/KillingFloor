using Photon.Pun;
using UnityEngine;
public class GranadeGunParticle : MonoBehaviourPun
{
    private GameObject user;
    private PlayerHealth playerHealth;

    private float damage = 5f;
    private float coolTime = 0.05f;
    private void OnParticleCollision(GameObject other)
    {

        GameObject IdOBj = FindTopmostParent(gameObject.transform).gameObject;

        int viewID = IdOBj.GetComponent<GranadeGun>().viewId;
        if (viewID != null)
        {
            user = PhotonView.Find(viewID).gameObject;
        }


        playerHealth = user.GetComponent<PlayerHealth>();


        Debug.Log(FindTopmostParent(other.transform));
        if (FindTopmostParent(other.transform).name.Contains("Zombie"))
        {
            if (FindTopmostParent(other.transform).gameObject.GetComponent<NormalZombie>().health > 0)
            {

                Debug.Log("���Դٱ�" + other.name);
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

        
      
       
          
        
        // �������κ��� LivingEntity Ÿ���� �������� �õ�
        LivingEntity attackTarget = other.GetComponent<LivingEntity>();
        if (attackTarget != null)
        {                // ������ �ǰ� ��ġ�� �ǰ� ������ �ٻ����� ���
            Vector3 hitPoint = other.transform.position;
            Vector3 hitNormal = transform.position - other.transform.position;

            // ���� ����
            attackTarget.OnDamage(1, hitPoint, hitNormal);
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
            if(currentTransform.name.Contains("(Clone"))
            {
                return currentTransform;
            }
            else
            {
                // �θ� ������ �θ��� �θ� ��������� ã���ϴ�.
                return FindTopmostParent(currentTransform.parent);
            }
          
        }
    }
}
