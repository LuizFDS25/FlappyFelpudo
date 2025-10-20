using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlaJogadorMouseEsquerdo : MonoBehaviour {

  bool comecou;
  bool acabou;
  Rigidbody2D corpoJogador;
  Vector2 forcaImpulso = new Vector2(0, 300f);
  
    // Atirador
  public GameObject bullet, spawnerBulletPos;

    // Sistema de Vida
  public int maxHealth = 9;              
  private int currentHealth;               
  public Image healthRenderer;    
  public Sprite[] healthSprites;

    // Invencibilidade após dano
    private float invincibleTime = 1f;
  private float lastDamageTime = -1f;

    // Animator para animações
    private Animator animator;

    // SpriteRenderer para efeito de hit
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public float hitFlashDuration = 0.1f;

    void Start ()
    {
        currentHealth = maxHealth;
        UpdateHealthSprite();
        corpoJogador = GetComponent<Rigidbody2D> ();

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
  
  void Update () 
    {

    if (Input.GetButtonDown ("Fire1")) { 
    
      if (!comecou) {
        comecou = true;
        corpoJogador.isKinematic = false;
      }

      corpoJogador.velocity = new Vector2 (0, 0);
      corpoJogador.AddForce(forcaImpulso);
            animator.SetTrigger("Jump");
        }

    if (Input.GetButtonDown("Jump"))
    {
        Instantiate(bullet, spawnerBulletPos.transform.position, this.gameObject.transform.rotation);
        }

    }

    void UpdateHealthSprite()
    {
        if (currentHealth > 0 && currentHealth <= healthSprites.Length)
        {
            int index = currentHealth - 1;
            healthRenderer.sprite = healthSprites[index]; // substitui healthRenderer.sprite
        }
        else
        {
            healthRenderer.sprite = null; // opcional: nenhum sprite quando morrer
        }
    }

    IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(hitFlashDuration);
        spriteRenderer.color = originalColor;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHealthSprite();

        if (currentHealth == 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (GameManager.instance != null)
            GameManager.instance.GameOver();
       
        Destroy(healthRenderer.gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.layer == 7)
        {
            if (Time.time - lastDamageTime > invincibleTime)
            {
                TakeDamage(2);
                StartCoroutine(FlashRed());
                lastDamageTime = Time.time;
            }
        }


        if (collision.gameObject.layer == 8)
        {
            if (Time.time - lastDamageTime > invincibleTime)
            {
                TakeDamage(1);
                StartCoroutine(FlashRed());
                lastDamageTime = Time.time;
            }
        }
    }
}
