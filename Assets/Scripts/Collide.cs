using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{
    private int health=3;
    [SerializeField] GameObject[] _healthUI;
    [SerializeField] private GameObject _gameOver;

    private void OnTriggerEnter(Collider other)
    {
        /*if(other.gameObject.CompareTag("sphere"))
        {
            health--;
            _healthUI[health].gameObject.SetActive(false);
            if(health ==0)
            {
                _gameOver.SetActive(true);
                StartCoroutine(Fade());
            }
        }*/
        if(other.gameObject.CompareTag("UI"))
        {
            var a = other.gameObject.transform.GetChild(0).gameObject;
            a.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("UI"))
        {
            var a = other.gameObject.transform.GetChild(0).gameObject;
            a.SetActive(false);

        }
    }


    IEnumerator Fade()
    {
        yield return new WaitForSeconds(1.6f);
        Time.timeScale =0;
    }


}
