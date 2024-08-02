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
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        hp.value = enemies_hp;
        animator = GetComponent<Animator>();
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemies_hp -= 20;
            hp.value = enemies_hp;
            if (enemies_hp == 0)
            {
                Destroy(gameObject);
            }
        }        
    }
}
