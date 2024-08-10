using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class cyborg : MonoBehaviour
{
    [SerializeField] private GameObject mission1;
        

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            mission1.SetActive(true);
        }
        else mission1.SetActive(false);
    }
}
