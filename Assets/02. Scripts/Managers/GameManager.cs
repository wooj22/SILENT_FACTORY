using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> offGameObjects;
    [SerializeField] Image fadeImage;
    [SerializeField] Text adviceText;
    [SerializeField] Transform machinePos;
    [SerializeField] ParticleSystem playerBlood;

    [SerializeField] AudioSource audios;

    public int essenceCount = 0;
    public bool isGetMaxEssence;

    private Transform playerPos;
    private LineRenderer playerLine;

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

    // ���� ���� üũ
    public void GetEssence()
    {
        essenceCount++;
        if(essenceCount >= 10)
        {
            isGetMaxEssence = true;
            EndPathDrawing();
        }
    }

    // playerPos���� machinePos���� ���η����� ��� ǥ��
    private void EndPathDrawing()
    {  
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerLine = GameObject.FindGameObjectWithTag("Player").GetComponent<LineRenderer>();

        // LineRenderer
        playerLine.positionCount = 2;
        playerLine.startWidth = 0.2f;
        playerLine.endWidth = 0.2f;
        playerLine.material = new Material(Shader.Find("Sprites/Default"));
        playerLine.startColor = Color.blue;
        playerLine.endColor = Color.red;

        InvokeRepeating(nameof(UpdateLine), 0, 0.5f);
    }

    private void UpdateLine()
    {
        playerLine.SetPosition(0, playerPos.position);
        playerLine.SetPosition(1, machinePos.position);
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
        audios.Play();
        SoundManager.Instance.PlaySFX("SFX_Die");
        SoundManager.Instance.FadeOutBGM();
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
        SoundManager.Instance.PlaySFX("SFX_Machine");

        // ���� ȭ��ֵ� ���ֱ�
        for (int i = 0; i < offGameObjects.Count; i++)
        {
            offGameObjects[i].SetActive(false);
        }

        // ���̵�
        StartCoroutine(FadeUI(fadeImage, 3f));
        yield return new WaitForSeconds(2f);

        // ��� �ؽ�Ʈ
        adviceText.text = "���� ����";
        StartCoroutine(FadeUI(adviceText, 3f));
        yield return new WaitForSeconds(6f);

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
