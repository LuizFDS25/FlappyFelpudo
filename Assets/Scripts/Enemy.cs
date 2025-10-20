using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Vida do inimigo
     int health = 30;

    // Partículas de hit e destruição
    public GameObject Hitparticles;
    public GameObject Destroyparticle;

    // Barra de vida
    public Transform healthBar;
    public GameObject healthBarObject;
    private Vector3 healthBarScale;
    private float healthPercentage;
    private Vector3 initialHealthBarPosition;
    private float initialHealthBarScaleX;

    // Animator para animações
    public Animator animator;

    // SpriteRenderer para efeito de hit
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float hitFlashDuration = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        
        health = 30;

        
        healthBarScale = healthBar.localScale;
        initialHealthBarScaleX = healthBarScale.x;
        initialHealthBarPosition = healthBar.localPosition;
        healthPercentage = healthBarScale.x / health;

        
        animator = GetComponent<Animator>();


        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }


    }

    // Atualiza a barra de vida com base na vida atual
    void UpdatehealthBar()
    {
        healthBarScale.x = healthPercentage * health;
        healthBar.localScale = healthBarScale;
        float offset = (initialHealthBarScaleX - healthBarScale.x) / 2f;
        healthBar.localPosition = initialHealthBarPosition + new Vector3(-offset, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Instantiate(Destroyparticle, transform.position, Quaternion.identity);

            if (GameManager.instance != null)
                GameManager.instance.AddScore(100);

            Destroy(gameObject);
        }
    }

    // Coroutine para piscar vermelho ao ser atingido
    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(hitFlashDuration);
        spriteRenderer.color = originalColor;
    }

    // Detecta colisões com balas
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("bulletPlayer"))
        {
            health -= 10;
            if (health < 0) health = 0;
            UpdatehealthBar();
            Instantiate(Hitparticles, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            animator.SetTrigger("Hit");
            StartCoroutine(FlashRed());
        }
    }
}
