using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemies : MonoBehaviour
{
    [SerializeField] private Slider hp;
    [SerializeField] private float moveSpeed = 5f;
    private float enemies_hp = 100f;
    [SerializeField]
    private float leftBomoving;
    [SerializeField]
    private float rightBomoving;
    private bool isMovingright=true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var currentPosition = transform.localPosition;
  
        if (currentPosition.x > rightBomoving)
        {
            isMovingright = true;
        }
        else if (currentPosition.x < leftBomoving)
        {
            isMovingright = false;
        }
        var direction = isMovingright ? Vector3.left : Vector3.right;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        var currentScale = transform.localScale;
        if ((isMovingright == true && currentScale.x < 0) || (isMovingright == false && currentScale.x > 0))
        {
            currentScale.x *= -1;
        }       
        transform.localScale = currentScale;
    }
}
