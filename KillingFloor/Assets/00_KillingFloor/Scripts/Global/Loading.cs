using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public void Run(float t, string name)
    {
        StartCoroutine(DelayForLoadScene(t, name));
    }

    public void RunForPause(float t, string name)
    {
        StartCoroutine(DelayForLoadSceneForPause(t, name));
    }

    // �񵿱� �ε� ��(�̸� �ε� �Ϸ��ϰ� ���ϴ� Ÿ�ֿ̹� �� ȣ��)
    public void asyncLoadScene(string name, float delay)
    {
        // �񵿱� �� �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        asyncLoad.allowSceneActivation = false; // �� �ε� �Ϸ� �� �ڵ� Ȱ��ȭ ����

        // �ε��� �Ϸ�� ������ ���
        StartCoroutine(WaitForSceneLoad(asyncLoad, delay));
    }

    public IEnumerator DelayForLoadScene(float t, string name)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(name);
    }

    public IEnumerator DelayForLoadSceneForPause(float t, string name)
    {
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - startTime < t)
        {
            yield return null;

            // �Ͻ������� ���¿��� �ð��� ����ϱ� ���� unscaledDeltaTime�� ���
            if (Time.timeScale == 0.0f)
            {
                startTime += Time.unscaledDeltaTime;
            }
        }

        SceneManager.LoadScene(name);
    }

    private IEnumerator WaitForSceneLoad(AsyncOperation asyncLoad, float delay)
    {
        while (!asyncLoad.isDone)
        {
            // �ε� ���� ��Ȳ�� ǥ���ϰų� �߰� �۾� ���� ����
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // 0.9f�� �ε��� 90%���� �Ϸ�Ǿ��ٴ� �ǹ�
            Debug.Log("Loading progress: " + progress * 100 + "%");

            // ������ �����ϸ� �� Ȱ��ȭ
            if (progress >= 1.0f)
            {
                yield return new WaitForSeconds(delay);
                asyncLoad.allowSceneActivation = true; // �� Ȱ��ȭ ���
            }

            // ���� �����ӱ��� ���
            yield return null;
        }

        // �ε� �Ϸ� �� �߰� �۾� ���� ����
        Debug.Log("Scene loading complete!");
    }
}
