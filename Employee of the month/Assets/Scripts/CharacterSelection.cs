using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Sprite character1;
    public void SelectedCharacter(int selectedCharacter)
    {
        Debug.Log("selected: " + selectedCharacter);

        //TODO koppla selected character till rätt player osv 
        //sätta detta i singleton typ
        switch (selectedCharacter)
        {
            case 1:
                playerPrefab.GetComponent<SpriteRenderer>().sprite = character1;
                break;

            case 2:

                break;

            case 3:

                break;

            case 4:

                break;

        }

    }


}
