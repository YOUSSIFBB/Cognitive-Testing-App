using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public static MouseCursor instance;
    public Texture2D handCursor;

    public Transform mCursorVisual;
    public Vector3 mDisplacement;

    public Sprite originalSprite;
    public Sprite highlightSprite;

    public bool isOverBlackSheep = false;
    private SpriteRenderer blackSheepRenderer;

    //score going up too fast slowing it down
    private float scoreUpdateInterval = 1f;

    private float scoreUpdateTime = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Hide the system mouse cursor
        Cursor.visible = false;
        // Set and Show the custom cursor
        ActivateCursor();
        mCursorVisual.gameObject.SetActive(true);
        // Find the SpriteRenderer component on the BlackSheep object
        blackSheepRenderer = GameObject.FindGameObjectWithTag("BlackSheep").GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Get the position of the mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Move the mouse cursor to the mouse position
        transform.position = mousePosition;

        // Cast a ray from the mouse position and see if it hits a black sheep
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("BlackSheep"))
        {
            if (!isOverBlackSheep)
            {
                // Set the sprite of the black sheep to the highlighted sprite
                blackSheepRenderer.sprite = highlightSprite;
                isOverBlackSheep = true;
            }

            // Move the cursor visual to the mouse position with an offset
            mCursorVisual.position = Input.mousePosition + mDisplacement;

            // Update the score
            scoreUpdateTime += Time.deltaTime;
            if (scoreUpdateTime >= scoreUpdateInterval)
            {
                Score scoreScript = FindObjectOfType<Score>();
                scoreScript.AddScore(1);
                scoreUpdateTime = 0f;
            }
        }
        else
        {
            if (isOverBlackSheep)
            {
                // Restore the original sprite of the black sheep
                blackSheepRenderer.sprite = originalSprite;
                isOverBlackSheep = false;
            }
        }

        // Show the system mouse cursor when the Space bar key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ActivateCursor()
    {
        Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
    }
}