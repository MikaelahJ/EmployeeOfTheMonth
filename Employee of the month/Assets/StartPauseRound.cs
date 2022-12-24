using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPauseRound : MonoBehaviour
{
    void Start()
    {
        if (GameManager.Instance.tiebreaker)
            GameManager.Instance.StartTiebreaker();
   
        else
            GameManager.Instance.StartRoundPause();

    }
}
