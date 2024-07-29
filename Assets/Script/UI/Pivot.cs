using UnityEngine;

public class AdjustPivot : MonoBehaviour
{
    public Vector2 newPivot;

    void Start()
    {
        AdjustSpritePivot(GetComponent<SpriteRenderer>(), newPivot);
    }

    void AdjustSpritePivot(SpriteRenderer spriteRenderer, Vector2 newPivot)
    {
        Sprite sprite = spriteRenderer.sprite;
        Rect rect = sprite.rect;
        Vector2 pivot = new Vector2(newPivot.x / rect.width, newPivot.y / rect.height);
        spriteRenderer.sprite = Sprite.Create(sprite.texture, rect, pivot, sprite.pixelsPerUnit);
    }
}
