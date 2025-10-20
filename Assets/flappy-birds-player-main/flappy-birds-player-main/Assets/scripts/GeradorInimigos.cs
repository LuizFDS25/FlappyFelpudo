using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class GeradorInimigos : MonoBehaviour
{
    // Prefab do inimigo (arraste no Inspector)
    public GameObject inimigoPrefab;

    // Intervalo entre spawns (segundos)
    public float intervalo = 3f;

    // Limites de spawn no cenário
    public float limiteX = 8f;
    public float limiteY = 4f;

    // Velocidade de movimento dos inimigos
    public float velocidade = 3f;

    // Limite X para destruir o inimigo ao sair da tela
    public float limiteDestruicaoX = -12f;

    // Prefab do boss e ponto de spawn
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public float spawnDuration = 60f;
    private bool bossSpawned = false;

    public TMP_Text timerText;

    void Start()
    {
        // Começa a gerar inimigos repetidamente
        InvokeRepeating("GerarInimigo", 0f, intervalo);


    }

    void Update()
    {
        if (!bossSpawned)
        {
            // Atualiza timer
            spawnDuration -= Time.deltaTime;
            if (spawnDuration < 0f) spawnDuration = 0f;

            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(spawnDuration / 60f);
                int seconds = Mathf.FloorToInt(spawnDuration % 60f);
                timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
            }

            // Spawn do boss
            if (spawnDuration <= 0f)
            {
                SpawnBoss();
            }
        }
    }

    void SpawnBoss()
    {
        CancelInvoke("GerarInimigo");

        if (!bossSpawned && bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            bossSpawned = true;

            if (GameManager.instance != null)
                GameManager.instance.ShowBossUI();

            if (timerText != null)
                timerText.transform.parent.gameObject.SetActive(false);
        }
    }

    void GerarInimigo()
    {
        if (bossSpawned) return;
        // Define posição de spawn (à direita da tela)
        float x = limiteX;
        float y = Random.Range(-limiteY, limiteY);
        Vector2 posicaoAleatoria = new Vector2(x, y);

        // Instancia o inimigo
        GameObject inimigo = Instantiate(inimigoPrefab, posicaoAleatoria, Quaternion.identity);

        // Inicia o movimento automático (corrotina)
        StartCoroutine(MoverInimigo(inimigo));
    }

    IEnumerator MoverInimigo(GameObject inimigo)
    {
        while (inimigo != null)
        {
            // Move o inimigo da direita para a esquerda
            inimigo.transform.Translate(Vector2.left * velocidade * Time.deltaTime);

            // Se o inimigo sair do limite visível, destrói o objeto
            if (inimigo.transform.position.x < limiteDestruicaoX)
            {
                Destroy(inimigo);
                yield break; // Sai da corrotina
            }

            yield return null; // Espera o próximo frame
        }
    }

}

    
