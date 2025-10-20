using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
      // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * 10 * Time.deltaTime);
        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
