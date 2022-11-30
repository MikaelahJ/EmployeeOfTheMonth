using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinPlayers : MonoBehaviour
{
    public GameObject playerController;
    // Start is called before the first frame update

    void Start()
    {
        JoinConnectedDevices();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinConnectedDevices()
    {
        int playerNo = 0;
	    foreach(InputDevice device in InputSystem.devices)
        {
            //GameObject pController = Instantiate(playerController);
            PlayerInputManager.instance.JoinPlayer(playerNo++, playerNo++, null, device);
        }

        
    }
}
