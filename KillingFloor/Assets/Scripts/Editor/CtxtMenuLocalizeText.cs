using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[InitializeOnLoad]
public class CtxtMenuLocalizeText : MonoBehaviour
{
    //TODO : [������] �����հ����ͼ� ĳ���س��ٰ� �׳༮ �����ͼ� ~~~�Ұž�
#if UNITY_EDITOR
#endif

    [MenuItem("GameObject/UI/Custom LocalizeText", false)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // �������� Resources/Prefabs �������� ������
        GameObject prefab = Resources.Load<GameObject>("[MJ]Prefabs/Text_Localize");

        if (prefab == null)
        {
            Debug.LogError("Text_Localize �������� ã�� �� �����ϴ�. Resources ������ �ùٸ��� ��ġ�Ǿ����� Ȯ���ϼ���.");
            return;
        }

        // �������� �����Ͽ� ���� �߰��մϴ�.
        GameObject go = Instantiate(prefab);

        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

        Selection.activeObject = go;
    }
}
