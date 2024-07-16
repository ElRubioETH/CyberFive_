using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private GameObject Open_chest;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        move();
        OpenChest();

    }
    public void move()
    {
        var move = Input.GetAxis("Horizontal");
        transform.localPosition += new Vector3(move, 0, 0) * movespeed * Time.deltaTime;
    }
    public void OpenChest()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Open_chest.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Open_chest.SetActive(false);
        }
    }
}
