using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BloodEffect : MonoBehaviour
{
    public bool InfiniteDecal;      // ���� ȿ���� ��� ǥ������ ����
    public Light DirLight;          // �ֺ� ���� ����
    public GameObject BloodAttach;  // ���� ȿ���� ���� ���� ������Ʈ
    public GameObject[] BloodFX;    // ���� ����Ʈ ������

    public Vector3 direction;
    int effectIdx;
    int activeBloods;

    // ����ĳ��Ʈ�� ���� ���� ��Ʈ ����Ʈ���� ���� ����� ���� ã�� �Լ�
    Transform GetNearestObject(Transform hit, Vector3 hitPos)
    {
        var closestPos = 100f;
        Transform closestBone = null;
        var childs = hit.GetComponentsInChildren<Transform>();

        foreach (var child in childs)
        {
            var dist = Vector3.Distance(child.position, hitPos);
            if (dist < closestPos)
            {
                closestPos = dist;
                closestBone = child;
            }
        }

        var distRoot = Vector3.Distance(hit.position, hitPos);
        if (distRoot < closestPos)
        {
            closestPos = distRoot;
            closestBone = hit;
        }
        return closestBone;
    }

    public void Start()
    {
        DirLight = GameObject.Find("Directional Light").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnBloodEffect(Vector3 hitPoint, float angle, Vector3 hitNormal, int _viewID)
    {

        if (effectIdx == BloodFX.Length) effectIdx = 0;

        // ���� ����Ʈ�� �����ϰ� ����
        var instance = Instantiate(BloodFX[effectIdx], hitPoint, Quaternion.Euler(0, angle + 90, 0));
        effectIdx++;
        activeBloods++;
        var settings = instance.GetComponent<BFX_BloodSettings>();
        settings.LightIntensityMultiplier = DirLight.intensity;
        Destroy(instance, 10f);

        PhotonView hitObj = PhotonView.Find(_viewID); // viewID�� ã������ PhotonView�� ID�Դϴ�.
        if (hitObj != null)
        {
            Debug.Log(hitObj + "�Ǹ� �׸���");
            GameObject targetObject = hitObj.gameObject;
            // ���� targetObject�� �ش� PhotonView�� ���� ���� ������Ʈ�Դϴ�.


            //// �ǰ� ������ ���� ����� ���� ã�� ���� ����Ʈ�� ����
            //var nearestBone = GetNearestObject(hit.transform.root, hitPoint);

            var attachBloodInstance = Instantiate(BloodAttach);
            var bloodT = attachBloodInstance.transform;
            bloodT.position = hitPoint;
            bloodT.localRotation = Quaternion.identity;
            bloodT.localScale = Vector3.one * Random.Range(0.75f, 1.2f);
            bloodT.LookAt(hitPoint + hitNormal, direction);
            bloodT.Rotate(90, 0, 0);
            bloodT.transform.parent = targetObject.transform;
            Destroy(attachBloodInstance,10f);
        }
    }

    // �� ���� ���� ������ ����ϴ� �Լ�
    public float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }

}
