using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    public void SelectedCharacter(int selectedCharacter)
    {
        //koppla selected character till r�tt player osv 
        Debug.Log("selected: " + selectedCharacter);
    }

    public void OnStartPress(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
