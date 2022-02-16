using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Invader : MonoBehaviour
{
    #region Constants

    const float GoDownByY = 0.03f;
    const float ChangeDirectionOnPosX = 0.6f;
    const float FramePerSecond = 1f;
    const float BulletSpeedPerSecond = 7.2f;

    #endregion

    #region Fields

    GameObject prefabInvaderBullet;
    GameObject prefabExplosion;

    Vector2 initialPosition;
    int direction;
    bool hasFired = false;
    bool canFire = true;
    bool isGamePaused = false;

    InvaderDestroyedEvent invaderDestroyed = new InvaderDestroyedEvent();
    InvaderInvisibleEvent invaderInvisible = new InvaderInvisibleEvent();

    // animation support
    [SerializeField] Sprite sprite0;
    [SerializeField] Sprite sprite1;
    SpriteRenderer spriteRenderer;
    int currentFrame = 1;
    float elapsedSeconds = 0;

    // cached for better performance
    Transform _transform;

    #endregion

    #region Properties

    public float SpeedPerSecond { get; set; }

    public float FireProbability { get; set; }

    #endregion

    #region MonoBehaviour methods

    void Awake()
    {
        _transform = transform;
        initialPosition = _transform.position;
        prefabInvaderBullet = Resources.Load<GameObject>(@"Prefabs/InvaderBullet");
        prefabExplosion = Resources.Load<GameObject>(@"Prefabs/Explosion");
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = 1;
    }

    void Start()
    {
        EventManager.AddEventHandler(EventName.InvaderDestroyed, invaderDestroyed);
        EventManager.AddEventHandler(EventName.InvaderInvisible, invaderInvisible);
        EventManager.AddEventListener(EventName.InvaderBulletHit, OnBulletHit);
        EventManager.AddEventListener(EventName.GamePause, OnGamePause);
        EventManager.AddEventListener(EventName.GameResume, OnGameResume);
    }

    void Update()
    {
        Animate();
        if (!isGamePaused)
        {
            Move();
            Fire();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShipBullet") || other.CompareTag("Ship"))
        {
            invaderDestroyed.Invoke(gameObject);
            Explosion explosion = Instantiate<GameObject>(prefabExplosion, transform.position, Quaternion.identity).GetComponent<Explosion>();
            explosion.SetColor(spriteRenderer.color);
            AudioManager.Play(AudioClipName.InvaderKilled);
            Destroy(gameObject);
        }
        else if (other.CompareTag("InvaderMapLimit"))
        {
            invaderInvisible.Invoke(gameObject);
        }
    }

    #endregion

    #region Public methods

    public void FlipDirection()
    {
        direction *= -1;
    }

    public void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void EnableFire()
    {
        canFire = true;
    }

    public void DisableFire()
    {
        canFire = false;
    }

    #endregion

    #region Private methods

    void Animate()
    {
        elapsedSeconds += Time.deltaTime;
        if (elapsedSeconds >= FramePerSecond)
        {
            elapsedSeconds = 0;
            if (currentFrame == 0) spriteRenderer.sprite = sprite0;
            else if (currentFrame == 1) spriteRenderer.sprite = sprite1;
            currentFrame++;
            if (currentFrame >= 2) currentFrame = 0;
        }
    }

    void Move()
    {
        Vector3 position = _transform.position;
        if (position.x > initialPosition.x + ChangeDirectionOnPosX ||
            position.x < initialPosition.x - ChangeDirectionOnPosX)
        {
            position.x = initialPosition.x + ChangeDirectionOnPosX * direction;
            position.y -= GoDownByY;
            direction *= -1;
        }
        position.x += direction * SpeedPerSecond * Time.deltaTime;
        _transform.position = position;
    }

    void Fire()
    {
        if (canFire && !hasFired && Random.value < FireProbability)
        {
            Bullet bullet = Instantiate<GameObject>(prefabInvaderBullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.SetBulletHitEvent(EventName.InvaderBulletHit, new InvaderBulletHitEvent());
            bullet.SetSpeedPerSecond(BulletSpeedPerSecond);
            bullet.SetColor(spriteRenderer.color);
            bullet.SetDirection(Vector3.down);
            AudioManager.Play(AudioClipName.Invader);
            hasFired = true;
        }
    }

    void OnGamePause(GameObject source)
    {
        isGamePaused = true;
    }

    void OnGameResume(GameObject source)
    {
        isGamePaused = false;
    }

    void OnBulletHit(GameObject source)
    {
        hasFired = false;
    }

    #endregion
}
