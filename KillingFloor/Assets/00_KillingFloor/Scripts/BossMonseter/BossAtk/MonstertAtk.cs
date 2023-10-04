using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstertAtk : MonoBehaviour
{
    private bool atkChk = false;
    private float damage = 8f;
    private float atkTime = 0.5f;
    private void OnTriggerStay(Collider other)
    {

        if (gameObject.name.Equals("Effect_07_Sphere"))
        {
            damage = 1;
            atkTime = 0.1f;
        }
        else
        {
            damage = 8;
            atkTime = 0.5f;
        }

        if(atkChk == false)
        {
            // �������κ��� LivingEntity Ÿ���� �������� �õ�
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();
            if (attackTarget != null)
            {                // ������ �ǰ� ��ġ�� �ǰ� ������ �ٻ����� ���
                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position - other.transform.position;

                // ���� ����
                attackTarget.OnDamage(damage, hitPoint, hitNormal);
            }
            Invoke("DamTime", atkTime);
            atkChk = true;
        }
      
    }

    private void DamTime()
    {
        atkChk = false;
    }
}
