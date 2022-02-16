using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GamePauseEvent gamePauseEvent = new GamePauseEvent();
    GameResumeEvent gameResumeEvent = new GameResumeEvent();

    void Start()
    {
        EventManager.AddEventHandler(EventName.GamePause, gamePauseEvent);
        EventManager.AddEventHandler(EventName.GameResume, gameResumeEvent);
        EventManager.AddEventListener(EventName.ShipDestroyed, OnShipDestroyed);
        EventManager.AddEventListener(EventName.AllInvadersDestroyed, OnAllInvadersDestroyed);
        EventManager.AddEventListener(EventName.InvaderInvisible, OnInvaderInvisible);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void PauseGame()
    {
        gamePauseEvent.Invoke(gameObject);
    }

    public void ResumeGame()
    {
        gameResumeEvent.Invoke(gameObject);
    }

    void OnShipDestroyed(GameObject source)
    {
        PauseGame();
    }

    void OnAllInvadersDestroyed(GameObject source)
    {
        AudioManager.Play(AudioClipName.Success);
        PauseGame();
    }

    void OnInvaderInvisible(GameObject source)
    {
        PauseGame();
    }
}
