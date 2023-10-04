using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]   // �ν����Ϳ��� ���� ���� ����ȭ
public class Lang
{
    public string lang, langLocalize;
    public List<string> value = new List<string>(); //text ���� List
}

public class Singleton : MonoBehaviour
{
    const string langURL = "https://docs.google.com/spreadsheets/d/17OfoYRzHY7psQCTczfuZwVxX5xT-GevQhSDxfmqqlXE/export?format=tsv";
    public int curLangIndex;    // ������ ���� ��� �����ϱ� ����

    public List<Lang> Langs;    // ���ö���¡�� ���� Class ����
                                // ������ �� �����ؾ��ϴ� �ʱ�ȭ X

    public event System.Action LocalizeChanged = () => { };
    public event System.Action LocalizeSettingChanged = () => { };

    #region ���ö���¡ �̱���
    public static Singleton local_singleton;
    private void Awake()
    {
        if (local_singleton == null)
        {
            local_singleton = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
        InitLang();
    }
    #endregion

    void InitLang()
    {
        PlayerPrefs.DeleteAll();

        int langIndex = PlayerPrefs.GetInt("LangIndex", -1);    // LangIndex�� ����� ����
                                                                // �⺻���� -1�� ����
        int systemIndex = Langs.FindIndex(x => x.lang.ToLower() == 
            Application.systemLanguage.ToString().ToLower());
        if (systemIndex == -1) systemIndex = 0;     // ������ �� ã�� ���ϸ� ��� �⺻����
        int index = langIndex == -1 ? systemIndex : langIndex;

        SetLangIndex(index);
    }

    public void SetLangIndex(int index)
    {
        curLangIndex = index;
        PlayerPrefs.SetInt("LangIndex", curLangIndex);  // LangIndex�� ������ ��� ����
        LocalizeChanged();
        LocalizeSettingChanged();
    }

    // �÷��� ���� �ʾƵ� ��� ������ �� �ֵ���
    [ContextMenu("��� ��������")]
    void GetLang()
    {
        StartCoroutine(GetLangCo());
    }

    IEnumerator GetLangCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(langURL);
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);    // ����� ����� �����
        SetLangList(www.downloadHandler.text);  // ����� ���� �ҷ��ͼ� 2���� �迭�� ����
    }

    void SetLangList(string tsv)
    {
        string[] row = tsv.Split('\n'); // ���� ������ �� ����
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length; // �� ������ �� ����
        string[,] sentence = new string[rowSize, columnSize];

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');

            for (int j = 0; j < columnSize; j++) sentence[i, j] = column[j];
        }

        // Ŭ���� ����Ʈ - editor���� �̸� Ŭ���� ����
        Langs = new List<Lang>();
        for (int i = 0; i < columnSize; i++)
        {
            Lang lang = new Lang();
            lang.lang = sentence[0, i];
            lang.langLocalize = sentence[1, i];

            for (int j = 2; j < rowSize; j++) lang.value.Add(sentence[j, i]);
            Langs.Add(lang);
        }
    }
}