using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Instantiated in WeaponModifyerItem
public class PickupText : MonoBehaviour
{
    public float offsetY = 5f;
    //public float killTextTime; //Should not be changed unless animation is also changed

    private TextMeshPro pickupText;
    private Animator textAnimation;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.parent.position + Vector3.up * offsetY; //Set text over player
        transform.LookAt(transform.position + Camera.main.transform.forward);

        //pickupText = GetComponent<TextMeshPro>();
        //textAnimation = GetComponent<Animator>();
        //pickupText.enabled = false;
    }

    private void LateUpdate()
    {
        transform.position = transform.parent.position + Vector3.up * offsetY; //Set text over player
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void ActivateText(string pickupType)
    {
        pickupText = GetComponent<TextMeshPro>();
        textAnimation = GetComponent<Animator>();
        Debug.Log(pickupType);
        pickupText.text = pickupType;
        textAnimation.SetTrigger("Start");

        //StartCoroutine(KillText());
    }

    //private IEnumerator KillText()
    //{
    //    yield return new WaitForSeconds(killTextTime);
    //    Destroy(this.gameObject);
    //}

    public void TriggerKillText()
    {
        Destroy(gameObject);
    }

}
