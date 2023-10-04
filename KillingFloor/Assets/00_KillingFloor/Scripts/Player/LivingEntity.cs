using Photon.Pun;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEngine.UI.Image;

// ����ü�μ� ������ ���� ������Ʈ���� ���� ���븦 ����
// ü��, ������ �޾Ƶ��̱�, ��� ���, ��� �̺�Ʈ�� ����
public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public float startingHealth = 100f; // ���� ü��
    public float startingArmor = 0f;    // ���� �Ƹ�
    public float health { get; protected set; } // ���� ü��
    public float armor { get; protected set; }
    public int coin { get; protected set; }
    public int level { get; protected set; }
    public int exp { get; protected set; }
    public bool dead { get; protected set; } // ��� ����


    //// ȣ��Ʈ->��� Ŭ���̾�Ʈ �������� ü�°� ��� ���¸� ����ȭ �ϴ� �޼���
    [PunRPC]
    public void ApplyUpdatedHealth(float newHealth, float newArmor, int newCoin,int newLevel, int newExp, bool newDead)
    {
        health = newHealth;
        armor = newArmor;
        coin = newCoin;
        level = newLevel;
        exp = newExp;
        dead = newDead;
    }

    // ����ü�� Ȱ��ȭ�ɶ� ���¸� ����
    protected virtual void OnEnable()
    {
        // ������� ���� ���·� ����
        dead = false;
        // ü���� ���� ü������ �ʱ�ȭ
        health = startingHealth;
        armor = startingArmor;
    }
   
    // ������ ó��
    // ȣ��Ʈ���� ���� �ܵ� ����ǰ�, ȣ��Ʈ�� ���� �ٸ� Ŭ���̾�Ʈ�鿡�� �ϰ� �����
    [PunRPC]
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ��������ŭ ü�� ����
            Damage(damage);

            // ȣ��Ʈ���� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, armor, coin, level, exp, dead);

            // �ٸ� Ŭ���̾�Ʈ�鵵 OnDamage�� �����ϵ��� ��
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);
        }

        //if(photonView.IsMine)
        //{
        //    PlayerUIManager.instance.SetArmor(armor);
        //    PlayerUIManager.instance.SetHP(health);
        //    PlayerUIManager.instance.SetBloodScreen(health);
        //}

        if (health < 0)
        {
            health = 0;
        }
            // ü���� 0 ���� && ���� ���� �ʾҴٸ� ��� ó�� ����
            if (health <= 0 && !dead)
         Die();

    }
    private void Damage(float _damage)
    {
        // �ƸӰ� 75�̻��̸� 75% ������ ���
        if (armor >= 75)
        {
            int armorDamage = Mathf.RoundToInt(_damage * 0.75f);
            Debug.Log(armorDamage);

            armor -= Mathf.RoundToInt(_damage * 0.75f);
            health -= Mathf.RoundToInt(_damage * 0.25f);
            if (0 >= armor) armor = 0;
        }
        else if (75 >= armor && armor > 50)
        {
            armor -= Mathf.RoundToInt(_damage * 0.65f);
            health -= Mathf.RoundToInt(_damage * 0.35f);
            if (0 >= armor) armor = 0;
        }
        else if (50 >= armor && armor > 0)
        {
            armor -= Mathf.RoundToInt(_damage * 0.55f);
            health -= Mathf.RoundToInt(_damage * 0.45f);
            if (0 >= armor) armor = 0;
        }
        else
        {
            health -= _damage;
        }

        if (0 >= health)
        { 
            health = 0;
            if (0 < armor) // �ƸӰ� �����ִٸ� ü�� 1�� �ѹ� ����ֱ�
            { health = 1; }
        }

    }

    // ü���� ȸ���ϴ� ���
    [PunRPC]
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead)
        {
            // �̹� ����� ��� ü���� ȸ���� �� ����
            return;
        }

        //ȣ��Ʈ�� ü���� ���� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            //ü�� �߰�
            health += newHealth;

            // �������� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, armor, coin, level, exp, dead);

            // �ٸ� Ŭ���̾�Ʈ�鵵 RestoreHealth�� �����ϵ��� ��
            photonView.RPC("RestoreHealth", RpcTarget.Others, newHealth);
        }
    }

    // �ƸӸ� �����ϴ� ���
    [PunRPC]
    public virtual void RestoreArmor(float newArmor)
    {
        if(dead)
        {
            // �̹� ����� ��� �Ƹ� ȸ�� �Ұ��� : ������ �׾ ���ŵ� ��������...
            return;
        }

        //ȣ��Ʈ�� �ǵ带 ���� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            armor += newArmor;
            // �������� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, armor, coin, level, exp, dead);

            // �ٸ� Ŭ���̾�Ʈ�鵵 RestoreHealth�� �����ϵ��� ��
            photonView.RPC("RestoreArmor", RpcTarget.Others, newArmor);
        }

    }
    public virtual void ExpUp(int value)
    {
        if(dead)
        {
            return;
        }
        //ȣ��Ʈ�� ����ġ�� ���� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            exp += value;
            Debug.Log(exp + "+" + value);
            // ����ġ�� 1000�� �ѱ�� ������
            if(1000 <= exp)
            {
                level++;
                exp -= 1000;
                LevelUp();
                Debug.Log("������");
            }
            // �������� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, armor, coin, level, exp, dead);

            // �ٸ� Ŭ���̾�Ʈ�鵵 RestoreHealth�� �����ϵ��� ��
            photonView.RPC("ExpUp", RpcTarget.Others, value);
        }
    }
    public virtual void GetCoin(int newCoin)
    {
        if(dead)
        {
            return;
        }
        //ȣ��Ʈ�� �ǵ带 ���� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            coin += newCoin;
            // �������� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, armor, coin, level, exp, dead);

            // �ٸ� Ŭ���̾�Ʈ�鵵 RestoreHealth�� �����ϵ��� ��
            photonView.RPC("GetCoin", RpcTarget.Others, newCoin);
        }
    }
    public virtual void SpendCoin(int newCoin)
    {
        if(dead)
        {
            return;
        }
        //ȣ��Ʈ�� �ǵ带 ���� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            coin -= newCoin;
            // �������� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, armor, coin, level, exp, dead);

            photonView.RPC("SpendCoin", RpcTarget.Others, newCoin);
        }
    }
    public virtual void OnPoison()
    {
        if (dead)
        {
            return;
        }
        //ȣ��Ʈ�� �ǵ带 ���� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("OnPoison", RpcTarget.Others);
        }
    }
    public virtual void Die()
    {
        // ��� ���¸� ������ ����
        dead = true;
    }
    public virtual void LevelUp()
    {

    }
}