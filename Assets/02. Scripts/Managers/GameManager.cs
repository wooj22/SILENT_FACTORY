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

    // 싱글톤
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


    // 게임오버 연출
    IEnumerator GameOverCo()
    {
        // 사망 사운드

        // 피 이펙트
        playerBlood.Play();

        // 게임 화면애들 없애기
        for (int i=0; i< offGameObjects.Count; i++)
        {
            offGameObjects[i].SetActive(false);
        }

        // 페이드
        StartCoroutine(FadeUI(fadeImage, 3f));
        yield return new WaitForSeconds(2f);

        // 사망 텍스트
        adviceText.text = "사망하셨습니다";
        StartCoroutine(FadeUI(adviceText, 3f));
        yield return new WaitForSeconds(6f);

        for (int i = 3; i >0; i--)
        {
            adviceText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // 메인메뉴 복귀
        SceneSwitch.Instance.SceneSwithcing("MainMenu");
    }
    
    // 게임성공 연출
    IEnumerator GameSuccessCo()
    {
        SceneSwitch.Instance.SceneSwithcing("MainMenu");
        yield return null;
    }


    // 페이드아웃
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
