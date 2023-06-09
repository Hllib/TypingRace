using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCar _playerCar;
    [SerializeField]
    private GameObject _pauseMenu;
    public bool IsPaused { get; private set; }
    [SerializeField]
    private Keyboard _keyboard;

    private float CurrentWPM;
    private static float BestWPM;
    private static float TotalWPM;

    private static int TotalHits;
    private static int TotalCorrectHits;

    private int CurrentTotalHits;
    private int CurrentCorrectHits;

    private float CurrentAccuracy;
    private static float BestAccuracy;
    private static float TotalAccuracy;

    private float CurrentTime;
    private static float BestTime;
    private static float TotalTime;

    public bool IsGameOver { get; set; }

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is NULL!");
            }

            return _instance;
        }
    }

    public void LoadStats()
    {
        TotalHits = PlayerPrefs.GetInt("TotalHits", 0);
        TotalCorrectHits = PlayerPrefs.GetInt("TotalCorrectHits", 0);

        CurrentWPM = 0;
        BestWPM = PlayerPrefs.GetFloat("BestWPM", 0);
        TotalWPM = PlayerPrefs.GetFloat("TotalWPM", 0);

        CurrentAccuracy = 0;
        BestAccuracy = PlayerPrefs.GetFloat("BestAccuracy", 0);
        TotalAccuracy = PlayerPrefs.GetFloat("TotalAccuracy", 0);

        CurrentTime = 0;
        BestTime = PlayerPrefs.GetFloat("BestTime", 0);
        TotalTime = PlayerPrefs.GetFloat("TotalTime", 0);

        ValueTuple<double, double, double> time = (Math.Round(CurrentTime, 1), Math.Round(BestTime, 1), Math.Round(TotalTime, 1));
        ValueTuple<double, double, double> wpm = (Math.Round(CurrentWPM, 1), Math.Round(BestWPM, 1), Math.Round(TotalWPM, 1));
        ValueTuple<float, float, float> accuracy = (CurrentAccuracy, BestAccuracy, TotalAccuracy);

        var stats = (wpm, accuracy, time);

        UIManager.Instance.UpdateStats(stats);
    }

    public void UpdateStats(int correctHits, int totalHits, float currentTime)
    {
        float timeSurplus = currentTime - CurrentTime;
        int correctHitsSurplus = correctHits - CurrentCorrectHits;
        int totalHitsSurplus = totalHits - CurrentTotalHits;

        CurrentCorrectHits = correctHits;
        CurrentTotalHits = totalHits;

        CurrentWPM = ((totalHits / 5) / currentTime);
        CurrentAccuracy = correctHits * 100 / totalHits;
        CurrentTime = currentTime;

        BestWPM = CurrentWPM > BestWPM ? CurrentWPM : BestWPM;
        BestAccuracy = CurrentAccuracy > BestAccuracy ? CurrentAccuracy : BestAccuracy;
        BestTime = CurrentTime > BestTime ? CurrentTime : BestTime;

        TotalCorrectHits += correctHitsSurplus;
        TotalHits += totalHitsSurplus;
        TotalTime += timeSurplus;
        TotalWPM = ((TotalHits / 5) / TotalTime);
        TotalAccuracy = TotalCorrectHits * 100 / TotalHits;

        ValueTuple<double, double, double> time = (Math.Round(CurrentTime, 1), Math.Round(BestTime, 1), Math.Round(TotalTime, 1));
        ValueTuple<double, double, double> wpm = (Math.Round(CurrentWPM, 1), Math.Round(BestWPM, 1), Math.Round(TotalWPM, 1));
        ValueTuple<float, float, float> accuracy = (CurrentAccuracy, BestAccuracy, TotalAccuracy);

        var stats = (wpm, accuracy, time);

        UIManager.Instance.UpdateStats(stats);
        UIManager.Instance.UpdateWPM(Math.Round(CurrentWPM * CurrentAccuracy / 100, 1));
        _playerCar.SetCarSpeed(Convert.ToInt32(Math.Round(CurrentWPM * CurrentAccuracy / 100, 0)));
    }

    public void SaveStats()
    {
        PlayerPrefs.SetFloat("BestAccuracy", BestAccuracy);
        PlayerPrefs.SetFloat("TotalAccuracy", TotalAccuracy);

        PlayerPrefs.SetFloat("BestWPM", BestWPM);
        PlayerPrefs.SetFloat("TotalWPM", TotalWPM);

        PlayerPrefs.SetFloat("BestTime", BestTime);
        PlayerPrefs.SetFloat("TotalTime", TotalTime);

        PlayerPrefs.SetInt("TotalHits", TotalHits);
        PlayerPrefs.SetInt("TotalCorrectHits", TotalCorrectHits);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadGame()
    {
        SaveStats();
        Time.timeScale = 1f;
        IsGameOver = false;
        UIManager.Instance.ShowPlayBt(true);
        LoadScene("Main");
    }

    public void LeaveGame()
    {
        SaveStats();
        Time.timeScale = 1f;
        LoadScene("Menu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsGameOver)
        {
            CheckPauseState();
        }
    }

    public void CheckPauseState()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);

        if (_pauseMenu != null)
        {
            switch (_pauseMenu.activeSelf)
            {
                case true:
                    StartPause();
                    break;
                case false:
                    StopPause();
                    break;
            }
        }
    }

    private void StopPause()
    {
        UIManager.Instance.ShowBlur(false);
        AudioManager.Instance.InitCarSounds(FMODEvents.Instance.carSounds);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.windowOpen, Vector3.zero);
        Time.timeScale = 1f;
        Cursor.visible = false;
        IsPaused = false;
        _keyboard.enabled = true;
    }

    private void StartPause()
    {
        UIManager.Instance.ShowBlur(true);
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.windowOpen, Vector3.zero);
        AudioManager.Instance.StopCarSounds();
        _keyboard.enabled = false;
        Time.timeScale = 0f;
        Cursor.visible = true;
        IsPaused = true;
    }

    private void Awake()
    {
        _instance = this;
        Cursor.visible = false;
    }

    private void Start()
    {
        FloorMover.Instance.ActivatePoolObjects(1);
        FloorMover.Instance.SpawnNextFloorTile(5);
        LoadStats();
    }
}

