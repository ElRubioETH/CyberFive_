using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class cyborg : MonoBehaviour
{
    [SerializeField] private GameObject mission1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            mission1.SetActive(true);
        }
        else mission1.SetActive(false);
    }
}
