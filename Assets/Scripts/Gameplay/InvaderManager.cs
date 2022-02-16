using System;
using System.Collections.Generic;
using UnityEngine;

public class InvaderManager : MonoBehaviour
{
    #region Constants

    [SerializeField] AnimationCurve invaderSpeedCurve;
    [SerializeField] AnimationCurve invaderFireProbabilityCurve;
    [SerializeField] const float InvaderSpacingY = 0.5f;

    #endregion

    #region Fields

    [SerializeField] int rows;
    [SerializeField] int columns;

    GameObject invaderPrefab;
    List<Invader> invaders; 

    int invadersTotalCount;
    AllInvadersDestroyedEvent allInvadersDestroyed = new AllInvadersDestroyedEvent();

    #endregion

    #region MonoBehaviour methods

    void Awake()
    {
        invadersTotalCount = rows * columns;
        invaders = new List<Invader>();
        invaderPrefab = Resources.Load<GameObject>(@"Prefabs/Invader");
    }

    void Start()
    {
        SetupEvents();
        SpawnInvaders();
    }

    #endregion

    #region Private methods

    void SetupEvents()
    {
        EventManager.AddEventHandler(EventName.AllInvadersDestroyed, allInvadersDestroyed);
        EventManager.AddEventListener(EventName.InvaderDestroyed, OnInvaderDestroyed);
        EventManager.AddEventListener(EventName.ShipDestroyed, OnGameOver);
    }

    void SpawnInvaders()
    {
        BoxCollider2D invaderCollider = invaderPrefab.GetComponent<BoxCollider2D>();
        Vector2 spawnPosition = transform.position;
        float startingPositionX = spawnPosition.x;

        // spacingX = (total width - 2 * padding left) / (number of columns - 1)
        float spacingX = (ScreenUtils.ScreenWidth - 2f * (startingPositionX - ScreenUtils.ScreenLeft)) / (float) (columns - 1);
        float spacingY = invaderCollider.size.y + InvaderSpacingY;

        Invader invader;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                invader = Instantiate<GameObject>(invaderPrefab, spawnPosition, Quaternion.identity).GetComponent<Invader>();
                invader.SpeedPerSecond = invaderSpeedCurve.Evaluate(0);
                ChangeInvaderColor(invader, row, col);
                ChangeInvaderDirection(invader, row, col);
                invaders.Add(invader);
                spawnPosition.x += spacingX;
            }
            spawnPosition.x = startingPositionX;
            spawnPosition.y -= spacingY;
        }
    }

    void ChangeInvaderColor(Invader invader, int row, int col)
    {
        if (row == 1 || row == 2)
        {
            invader.ChangeColor(new Color(Colors.InvaderBlueRValue, Colors.InvaderBlueGValue, Colors.InvaderBlueBValue));
        }
        if (row == 3 || row == 4)
        {
            invader.ChangeColor(new Color(Colors.InvaderRedRValue, Colors.InvaderRedGValue, Colors.InvaderRedBValue));
        }
    }

    void ChangeInvaderDirection(Invader invader, int row, int col)
    {
        if (row == 1 || row == 3)
        {
            invader.FlipDirection();
        }
    }

    void OnInvaderDestroyed(GameObject source)
    {
        invaders.Remove(source.GetComponent<Invader>());

        if (invaders.Count == 0)
        {
            allInvadersDestroyed.Invoke(gameObject);
        }

        foreach (Invader invader in invaders)
        {
            invader.SpeedPerSecond = invaderSpeedCurve.Evaluate(1f - (invaders.Count / (float) invadersTotalCount));
            invader.FireProbability = invaderFireProbabilityCurve.Evaluate(1f - (invaders.Count / (float)invadersTotalCount));
        }
    }

    void OnGameOver(GameObject source)
    {
        foreach (Invader invader in invaders)
        {
            invader.DisableFire();
        }
    }

    #endregion
}
