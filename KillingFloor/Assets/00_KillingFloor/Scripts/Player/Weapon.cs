using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{   public enum Type { Pistol, Rifle, Melee, Heal };


    public Transform targetObj;             // �÷��̾� ����
    public Transform leftHandObj = null;    // �޼� �׷�
    public Transform rightHandObj = null;   // ������ �׷�
    public Transform leftElbowObj = null;    // �޼� �׷�
    public Transform rightElbowObj = null;   // ������ �׷�

    [Header("Weapon")]
    public int weaponID;
    public Type weaponType;
    public float damage;       // ������
    public float fireRate;     // ����ӵ� : RPM���� ���
    public float reloadRate;   // ������ �ӵ� : �ʷ� ���
    public float ammo;         // ���� źâ
    public float remainingAmmo;// �ܿ� ź��
    public float magazineSize; // źâ �뷮
    public float maxAmmo;      //�ִ� �Ѿ�

    public AudioClip gunAudio;    // �� �Ҹ�
    public AudioClip reloadAudio; // ���� �Ҹ�
    public AudioClip emptyAudio;  // �� �� �Ҹ�

}
