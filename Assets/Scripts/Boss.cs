using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // Vida do chefe
    int health = 600;
    private int currentHealth;

    // Movimento vertical
    public float moveSpeed = 2f;
    private int moveDirection = 1;

    // Partículas de hit e destruição
    public GameObject hitParticles;
    public GameObject deathParticles;

    // Prefab do projétil e ponto de disparo
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireInterval = 1.5f;



    // SpriteRenderer para efeito de hit
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float hitFlashDuration = 0.1f;

    // Estado de fúria
    private bool isEnraged = false;

    // Estado de morte
    private bool isDead = false;

    void Start()
    {
        

        currentHealth = health;
        if (GameManager.instance != null)
            GameManager.instance.ShowBossUI();


        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        StartCoroutine(Atacar());
    }

    void Update()
    {
        if (!isDead)
        {
            transform.Translate(Vector2.up * moveSpeed * moveDirection * Time.deltaTime);
        }
    }

   

    IEnumerator Atacar()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(fireInterval);
            if (bulletPrefab != null && firePoint != null)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(hitFlashDuration);
        spriteRenderer.color = originalColor;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            moveDirection *= -1;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(isDead) return;

        if (collision.gameObject.CompareTag("bulletPlayer"))
        {
            currentHealth -= 10;
            if (currentHealth < 0) currentHealth = 0;

            if (GameManager.instance != null)
                GameManager.instance.UpdateBossHealth(currentHealth, health);

            if (spriteRenderer != null)
                StartCoroutine(FlashRed());
            
            Destroy(collision.gameObject);
            Instantiate(hitParticles, transform.position, Quaternion.identity);

            if (currentHealth <= 300 && !isEnraged)
            {
                isEnraged = true;
                fireInterval /= 2f;
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        isDead = true;
        Instantiate(deathParticles, transform.position, Quaternion.identity);

        if(GameManager.instance != null)
        GameManager.instance.HideBossUI();

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        if (GameManager.instance != null)
            GameManager.instance.AddScore(1000);


        StartCoroutine(DelayGameOver());
    }

    IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(3f);

        if (GameManager.instance != null)
            GameManager.instance.GameWon();

        Destroy(gameObject); 
    }


}
