using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGamaManager : MonoBehaviour
{

    // �����÷��̾� ���� +1 Test �޼���
    public void OnLvUpBtn()
    {
        // ��ư�� ������ localPlayerLv �� ���Ҵ�
        int lvUp = int.Parse(NetworkManager.net_instance.localPlayerLv) + 1;
        NetworkManager.net_instance.localPlayerLv = lvUp.ToString();

        Debug.Log("���ϱ� 1 ���� ��: " + NetworkManager.net_instance.localPlayerLv);

        // �����Ϳ� ����� ���� ������ �����ϱ� ���ؼ� �� �ʿ�
        NetworkManager.net_instance.SetData(NetworkManager.net_instance.localPlayerLv);
    }
    // �����÷��̾� ���� -1 Test �޼���
    public void OnLvDownBtn()
    {
        int lvDown = int.Parse(NetworkManager.net_instance.localPlayerLv) - 1;
        NetworkManager.net_instance.localPlayerLv = lvDown.ToString();

        Debug.Log("���� 1 ���� ��: " + NetworkManager.net_instance.localPlayerLv);

        NetworkManager.net_instance.SetData(NetworkManager.net_instance.localPlayerLv);
    }
}
