using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int score = 0;
    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    public TMP_Text finalScoreTextBad;
    public GameObject gameWonPanel;
    public TMP_Text finalScoreTextGood;

    // Boss health bar prefabs
    public GameObject bossHealthPanel; 
    public Image bossHealthFill;      


    private Vector3 originalScale;

    void Start()
    {
        // Singleton — garante que só exista um GameManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        bossHealthPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameWonPanel.SetActive(false);
        AtualizarScoreUI();

        if (scoreText != null)
            originalScale = scoreText.transform.localScale;
    }

    public void AddScore(int value)
    {
        score += value;
        AtualizarScoreUI();

        if (scoreText != null)
            StartCoroutine(PopScoreEffect());
    }

    void AtualizarScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Pontuação: " + score;
    }

    public void ShowBossUI()
    {
        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(true);

        if (bossHealthFill != null)
            bossHealthFill.fillAmount = 1f;
    }

    public void UpdateBossHealth(float currentHealth, float maxHealth)
    {
        if (bossHealthFill != null)
            bossHealthFill.fillAmount = currentHealth / maxHealth;
    }
    public void HideBossUI()
    {
        if (bossHealthPanel != null)
            bossHealthPanel.SetActive(false);
    }


    IEnumerator PopScoreEffect()
    {
        
        float t = 0f;
        float popScale = 1.3f;
        float speed = 6f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            float scale = Mathf.Lerp(1f, popScale, Mathf.Sin(t * Mathf.PI));
            scoreText.transform.localScale = originalScale * scale;
            yield return null;
        }

        scoreText.transform.localScale = originalScale;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        Destroy(scoreText.gameObject);
        HideBossUI();
        gameOverPanel.SetActive(true);
        finalScoreTextBad.text = "Pontuação Final: " + score;
    }

    public void GameWon()
    {
        Time.timeScale = 0;
        Destroy(scoreText.gameObject);
        HideBossUI();
        gameWonPanel.SetActive(true);
        finalScoreTextGood.text = "Pontuação Final: " + score;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
