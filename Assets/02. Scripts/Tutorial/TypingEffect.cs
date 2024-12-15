using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingEffect : MonoBehaviour
{
    [SerializeField] Text infoText;
    [SerializeField] Image fadeImage;
    [SerializeField] string[] gameInfoString;

    [SerializeField] private float typingSpeed = 0.05f;

    private Coroutine typing;

    public void Start()
    {
        typing = StartCoroutine(Typing());
    }

    // 스킵 버튼
    public void Skip()
    {
        StopCoroutine(typing);
        StartCoroutine(FadeOutEnd());
    }

    // 타이핑 연출
    private IEnumerator Typing()
    {
        infoText.text = "";

        foreach (string paragraph in gameInfoString)
        {
            // typing
            foreach (char c in paragraph)
            {
                infoText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
            yield return new WaitForSeconds(1f);

            // Backspace
            for (int i = infoText.text.Length - 1; i >= 0; i--)
            {
                infoText.text = infoText.text.Substring(0, i);
                yield return new WaitForSeconds(typingSpeed / 2);
            }

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeOutEnd());
    }

    // 페이드아웃 후 메인게임 시작
    private IEnumerator FadeOutEnd()
    {
        fadeImage.gameObject.SetActive(true);

        Color fadeColor = fadeImage.color;
        float fadeDuration = 2f;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            fadeColor.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 1;
        fadeImage.color = fadeColor;

        yield return new WaitForSeconds(3f);
        SceneSwitch.Instance.SceneSwithcing("MainGame");
    }
}
