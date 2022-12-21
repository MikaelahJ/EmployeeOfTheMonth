using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupText : MonoBehaviour
{
    public float offsetY = 5f;

    private TextMeshPro pickupText;
    private Animator textAnimation;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.parent.position + Vector3.up * offsetY; //Make sure the bar is over player
        transform.LookAt(transform.position + Camera.main.transform.forward);

        pickupText = GetComponent<TextMeshPro>();
        textAnimation = GetComponent<Animator>();
        pickupText.enabled = false;
    }

    private void LateUpdate()
    {
        transform.position = transform.parent.position + Vector3.up * offsetY; //Make sure the bar is over player
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void ActivateText(string pickupType)
    {
        pickupText.enabled = true;
        pickupText.text = pickupType;
        textAnimation.SetTrigger("Start");

        StartCoroutine(ToogleText());
    }

    private IEnumerator ToogleText()
    {
        yield return new WaitForSeconds(2);
        pickupText.enabled = false;
        //Destroy(this.gameObject);
    }
}
