using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupText : MonoBehaviour
{
    public float offsetY = 5f;
    public float killTextTime = 2f;

    private TextMeshPro pickupText;
    private Animator textAnimation;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.parent.position + Vector3.up * offsetY; //Make sure the bar is over player
        transform.LookAt(transform.position + Camera.main.transform.forward);

        //pickupText = GetComponent<TextMeshPro>();
        //textAnimation = GetComponent<Animator>();
        //pickupText.enabled = false;
    }

    private void LateUpdate()
    {
        transform.position = transform.parent.position + Vector3.up * offsetY; //Make sure the bar is over player
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void ActivateText(string pickupType)
    {
        pickupText = GetComponent<TextMeshPro>();
        textAnimation = GetComponent<Animator>();
        Debug.Log(pickupType);
        pickupText.text = pickupType;
        textAnimation.SetTrigger("Start");

        StartCoroutine(KillText());
    }

    private IEnumerator KillText()
    {
        yield return new WaitForSeconds(killTextTime);
        Destroy(this.gameObject);
    }
}
