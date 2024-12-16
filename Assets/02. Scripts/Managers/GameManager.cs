using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> offGameObjects;
    [SerializeField] Image fadeImage;
    [SerializeField] Text adviceText;

    [SerializeField] ParticleSystem playerBlood;

    public static GameManager Instance { get; private set; }

    // �̱���
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCo());
    }

    public void GameSuccess()
    {
        StartCoroutine(GameSuccessCo());
    }


    // ���ӿ��� ����
    IEnumerator GameOverCo()
    {
        // ��� ����

        // �� ����Ʈ
        playerBlood.Play();

        // ���� ȭ��ֵ� ���ֱ�
        for (int i=0; i< offGameObjects.Count; i++)
        {
            offGameObjects[i].SetActive(false);
        }

        // ���̵�
        StartCoroutine(FadeUI(fadeImage, 3f));
        yield return new WaitForSeconds(2f);

        // ��� �ؽ�Ʈ
        adviceText.text = "����ϼ̽��ϴ�";
        StartCoroutine(FadeUI(adviceText, 3f));
        yield return new WaitForSeconds(6f);

        for (int i = 3; i >0; i--)
        {
            adviceText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // ���θ޴� ����
        SceneSwitch.Instance.SceneSwithcing("MainMenu");
    }
    
    // ���Ӽ��� ����
    IEnumerator GameSuccessCo()
    {
        SceneSwitch.Instance.SceneSwithcing("MainMenu");
        yield return null;
    }


    // ���̵�ƿ�
    IEnumerator FadeUI(Graphic graphic, float duration = 3f)
    {
        Color color = graphic.color;
        float startAlpha = color.a;
        graphic.gameObject.SetActive(true);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(startAlpha, 1f, t / duration);
            graphic.color = color;
            yield return null;
        }

        color.a = 1f;
        graphic.color = color;
    }
}
