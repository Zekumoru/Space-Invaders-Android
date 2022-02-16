using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    #region Constants

    const float DestroyAfterSeconds = 0.08f;

    #endregion

    #region Fields

    SpriteRenderer spriteRenderer;
    float elapsedSeconds = 0;

    #endregion

    #region MonoBehaviour methods

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        elapsedSeconds += Time.deltaTime;
        if (elapsedSeconds >= DestroyAfterSeconds)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Public methods

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    #endregion
}
