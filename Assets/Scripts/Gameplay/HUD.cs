using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    #region Constants

    const string ScoreTextPrefix = "Score: ";

    #endregion

    #region Fields

    [SerializeField] Text scoreText;
    [SerializeField] Text descriptionText;
    [SerializeField] Button restartButton;
    int score = 0;

    #endregion

    #region MonoBehaviour methods

    void Start()
    {
        scoreText.text = ScoreTextPrefix + score;
        EventManager.AddEventListener(EventName.ShipDestroyed, OnShipDestroyed);
        EventManager.AddEventListener(EventName.InvaderDestroyed, OnInvaderDestroyed);
        EventManager.AddEventListener(EventName.AllInvadersDestroyed, OnAllInvadersDestroyed);
        EventManager.AddEventListener(EventName.InvaderInvisible, OnInvaderInvisible);
    }

    void GameOver()
    {
        descriptionText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    void OnShipDestroyed(GameObject source)
    {
        GameOver();
        scoreText.text = "Game Over! You Lost! Your total score is: " + score;
        descriptionText.text = "Uh oh! Don't let the ship gets destroyed next time! Tap the screen to play again.";
    }

    void OnInvaderDestroyed(GameObject source)
    {
        score += 10;
        scoreText.text = ScoreTextPrefix + score;
    }

    void OnAllInvadersDestroyed(GameObject source)
    {
        GameOver();
        scoreText.text = "Congratulations! You won! Your total score is: " + score;
        descriptionText.text = "You destroyed all invaders! Tap the screen to play again.";
    }

    void OnInvaderInvisible(GameObject source)
    {
        GameOver();
        scoreText.text = "Game Over! You lost! Your total score is: " + score;
        descriptionText.text = "You  did not kill all invaders! Tap the screen to play again.";
    }

    #endregion
}
