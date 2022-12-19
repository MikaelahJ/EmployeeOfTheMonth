using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPauseRound : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.StartRoundPause();
    }

}
