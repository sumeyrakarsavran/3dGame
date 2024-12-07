using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCollection : MonoBehaviour
{
    private AudioSource click;
    [SerializeField] private TMP_Text _text;
    private int count=0;

    private void Start()
    {
       click = GetComponent<AudioSource>(); 
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("coin"))
        {
            count++;
            _text.text = count.ToString();
            Destroy(other.gameObject);
            click.Play();
        }

    }
}
