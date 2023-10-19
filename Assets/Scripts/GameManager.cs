using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Image fadeImage;
    private Blade blade;
    private Spawner spawner;
    private int score;

    private void Awake()
    {
        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
    }

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        Time.timeScale = 1f; //установка шкалы времени для удобства его регулирования при появлении белого экрана

        ClearScene();

        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString();
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void Explode() //взрыв
    {
        blade.enabled = false;
        spawner.enabled = false;

        StartCoroutine(ExplodeSequence());
    }

    private IEnumerator ExplodeSequence() //появление белой картинки
    {
        float elapsed = 0f; //для отслеживания, сколько прошло времени
        float duration = 0.5f; //для продолжительности анимации

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration); //вычислить процент завершенной анимации
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t); //когда т=0, цвет прозрачный, когда тбольше, цвет белеет

            Time.timeScale = 1f - t; //замедлить время
            elapsed += Time.unscaledDeltaTime; //немасштабированное время

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        NewGame();

        elapsed = 0f; //сбросить истекшее время до нуля

        while (elapsed < duration) //в обратном порядке, исчезание белого
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
    }

}
