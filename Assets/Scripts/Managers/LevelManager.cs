using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Action OnGameOver;
    public Action OnGameWon;


    //TODO : Level criterias, parameters, etc...



    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
    }

    private void GameWon()
    {
        OnGameWon?.Invoke();
    }
}
