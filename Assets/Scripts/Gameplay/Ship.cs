using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    #region Constants

    const float SpeedPerSecond = 9.6f;

    #endregion

    #region Fields

    bool isDragging = false;
    bool isGamePaused = false;
    bool hasFiredBullet = false;
    Vector3 initialShipPositionOnDrag;
    Vector3 initialDraggingPosition;
    GameObject prefabBullet;
    GameObject prefabExplosion;

    ShipDestroyedEvent shipDestroyed = new ShipDestroyedEvent();

    // cached for better performance
    Transform _transform;
    float halfColliderWidth;

    #endregion

    #region MonoBehaviour methods

    void Awake()
    {
        _transform = transform;
        halfColliderWidth = GetComponent<BoxCollider2D>().size.x / 2;
        prefabBullet = Resources.Load<GameObject>(@"Prefabs/ShipBullet");
        prefabExplosion = Resources.Load<GameObject>(@"Prefabs/Explosion");
    }

    void Start()
    {
        EventManager.AddEventHandler(EventName.ShipDestroyed, shipDestroyed);
        EventManager.AddEventListener(EventName.ShipBulletHit, OnBulletHit);
        EventManager.AddEventListener(EventName.GamePause, OnGamePause);
        EventManager.AddEventListener(EventName.GameResume, OnGameResume);
        EventManager.AddEventListener(EventName.ShipDraggingInput, OnShipDragInput);
        EventManager.AddEventListener(EventName.ShipReleasingInput, OnShipReleaseDragInput);
    }

    void Update()
    {
        if (!isGamePaused)
        {
            HandleMoveInput();
            HandleFiring();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("InvaderBullet") || other.CompareTag("Invader"))
        {
            shipDestroyed.Invoke(gameObject);
            Explosion explosion = Instantiate<GameObject>(prefabExplosion, transform.position, Quaternion.identity).GetComponent<Explosion>();
            explosion.SetColor(GetComponent<SpriteRenderer>().color);
            AudioManager.Play(AudioClipName.Explosion);
            Destroy(gameObject);
        }
    }

    #endregion

    #region Private methods

    void HandleMoveInput()
    {
        if (isDragging)
        {
            Vector2 position = _transform.position;
            Vector2 dragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - initialDraggingPosition;
            position.x = dragPosition.x + initialShipPositionOnDrag.x;
            _transform.position = ClampInScreen(position);
        }
    }

    Vector3 ClampInScreen(Vector3 position)
    {
        if (position.x - halfColliderWidth < ScreenUtils.ScreenLeft)
        {
            position.x = ScreenUtils.ScreenLeft + halfColliderWidth;
        }
        else if (position.x + halfColliderWidth > ScreenUtils.ScreenRight)
        {
            position.x = ScreenUtils.ScreenRight - halfColliderWidth;
        }
        return position;
    }

    void HandleFiring()
    {
        if (!hasFiredBullet)
        {
            Bullet bullet = Instantiate<GameObject>(prefabBullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
            bullet.SetBulletHitEvent(EventName.ShipBulletHit, new ShipBulletHitEvent());
            bullet.SetDirection(Vector3.up);
            AudioManager.Play(AudioClipName.Shoot);
            hasFiredBullet = true;
        }
    }

    void OnShipDragInput(GameObject source)
    {
        isDragging = true;
        initialShipPositionOnDrag = _transform.position;
        initialDraggingPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnShipReleaseDragInput(GameObject source)
    {
        isDragging = false;
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
        hasFiredBullet = false;
    }

    #endregion
}
