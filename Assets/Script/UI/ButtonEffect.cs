using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimationController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Hover", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        animator.SetTrigger("Click");
    }
}
