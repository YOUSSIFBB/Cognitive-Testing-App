using UnityEngine;

public class BlackSheepHighlight : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite originalSprite;
    public Sprite highlightSprite;

    private void Start()
    {
        originalSprite = GetComponent<SpriteRenderer>().sprite;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        // Change the sprite to the highlighted sprite when the mouse enters the collider
        spriteRenderer.sprite = highlightSprite;
    }

    private void OnMouseExit()
    {
        // Change the sprite back to the original sprite when the mouse exits the collider
        spriteRenderer.sprite = originalSprite;
    }
}