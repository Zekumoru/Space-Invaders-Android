using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Constants

    const float DefaultSpeedPerSecond = 17f;

    #endregion

    #region Fields

    bool isGamePaused;
    float speedPerSecond;

    Vector3 direction;
    BulletHitEvent bulletHit;

    // cached for performance
    SpriteRenderer spriteRenderer;
    Transform _transform;

    #endregion

    #region MonoBehaviour methods

    void Awake()
    {
        _transform = transform;
        speedPerSecond = DefaultSpeedPerSecond;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        EventManager.AddEventListener(EventName.GamePause, OnGamePause);
        EventManager.AddEventListener(EventName.GameResume, OnGameResume);
    }

    void Update()
    {
        if (!isGamePaused)
        {
            _transform.position += direction * speedPerSecond * Time.deltaTime;
        }
    }

    void OnBecameInvisible()
    {
        DestroyBullet();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DestroyBullet();
    }

    #endregion

    #region Private methods

    void OnGamePause(GameObject source)
    {
        isGamePaused = true;
    }

    void OnGameResume(GameObject source)
    {
        isGamePaused = false;
    }

    void DestroyBullet()
    {
        bulletHit.Invoke(gameObject);
        Destroy(gameObject);
    }

    #endregion

    #region Public methods

    public void SetBulletHitEvent(EventName eventName, BulletHitEvent bulletHitEvent)
    { 
        bulletHit = bulletHitEvent;
        EventManager.AddEventHandler(eventName, bulletHit);
    }

    public void SetSpeedPerSecond(float newSpeedPerSecond)
    { 
        speedPerSecond = newSpeedPerSecond;
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    #endregion
}
