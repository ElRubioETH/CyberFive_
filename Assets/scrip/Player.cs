using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movespeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
    public void move()
    {
        var move = Input.GetAxis("Horizontal");
        transform.localPosition += new Vector3(move, 0, 0) * movespeed * Time.deltaTime;
    }
}
