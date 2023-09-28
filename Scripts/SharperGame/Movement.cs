using UnityEngine;

public class Movement : MonoBehaviour
{
    private string behaviour;
    private bool needsNewBehaviour = true;
    private bool currentlyMoving = false;
    private bool currentlyIdling = false;

    // Floats used to process the sheep's move behaviour.
    private float moveLength;

    public float moveLengthMin;
    public float moveLengthMax;
    private float moveTime;
    public float moveDist;
    private Vector2 moveDirection;

    // Floats used to process the sheep's idle behaviour.
    public float idleMin;

    public float idleMax;
    private float idleTime;

    // Sprite Renderer
    public SpriteRenderer sheepSpriteRenderer;

    //sheeps rigidbody
    public Rigidbody2D rb;

    private GameManager gameManager;
    private float baseMoveSpeed;
    public float blackSheepSpeedMultiplier = 1.2f;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        // Set move speed for sheep
        baseMoveSpeed = 4f;
        // Set move speed for black sheep
        if (gameObject.CompareTag("BlackSheep"))
        {
            baseMoveSpeed *= blackSheepSpeedMultiplier;
        }
    }

    private void Update()
    {
        if (gameManager.pnlActive == false)
        {
            MoveSheep();
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    private void MoveSheep()
    {
        if (needsNewBehaviour)
        {
            behaviour = ChooseBehaviour();
            needsNewBehaviour = false;
        }
        switch (behaviour)
        {
            case "move":
                if (!currentlyMoving)
                {
                    moveTime = 0;
                    moveLength = Random.Range(moveLengthMin, moveLengthMax);
                    moveDirection = new Vector2(Random.Range(-moveDist, moveDist), Random.Range(-moveDist, moveDist));
                    if (moveDirection.x < 0)
                    {
                        sheepSpriteRenderer.flipX = true;
                    }
                    else if (moveDirection.x > 0)
                    {
                        sheepSpriteRenderer.flipX = false;
                    }
                    currentlyMoving = true;
                }
                if (moveTime < moveLength)
                {
                    rb.velocity = moveDirection * baseMoveSpeed;

                    moveTime += Time.fixedDeltaTime;
                }
                else
                {
                    currentlyMoving = false;
                    needsNewBehaviour = true;
                }
                break;

            case "idle":

                if (!currentlyIdling)
                {
                    idleTime = Random.Range(0.2f, 1.5f);
                    currentlyIdling = true;
                }
                if (idleTime > 0)
                {
                    rb.velocity = Vector2.zero;
                    idleTime -= Time.deltaTime;
                }
                else
                {
                    currentlyIdling = false;
                    needsNewBehaviour = true;
                }
                break;
        }
    }

    // Randomly returns one of two strings describing a possible sheep behaviour.
    private string ChooseBehaviour()
    {
        int behaviour = Random.Range(0, 2);
        switch (behaviour)
        {
            case 0:
                return "move";

            default:
                return "idle";
        }
    }
}