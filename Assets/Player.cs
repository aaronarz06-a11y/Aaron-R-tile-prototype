using UnityEngine;

public class Player : MonoBehaviour
{
    public float tileSize = 2f;
    public int maxHealth = 3;
    public float fallHeight = -2f;
    public float moveCooldown = 1f;
    private int currentHealth;
    private bool canMove = true;
    private float moveTimer = 0f;
    private int moveCount = 0;
    private bool gridChangeReady = false;

    void Start()
    {
        //sets the player's starting health
        currentHealth = maxHealth;

        SnapToGrid();
    }

    void Update()
    {
        //checks if the player fell off the map
        if (transform.position.y <= fallHeight)
        {
            currentHealth = 0;

            GameManager gameManager = FindAnyObjectByType<GameManager>();

            if (gameManager != null)
            {
                gameManager.LoseGame();
            }

            canMove = false;
            return;
        }

        //movement timer countdown
        if (!canMove)
        {
            moveTimer -= Time.deltaTime;

            if (moveTimer <= 0f)
            {
                canMove = true;
            }
        }

        //WASD movement
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Move(Vector3.forward);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Move(Vector3.back);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(Vector3.left);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                Move(Vector3.right);
            }
        }
    }

    //moves the player one tile
    void Move(Vector3 direction)
    {
        //changes the grid if the previous move landed on a conveyor
        if (gridChangeReady)
        {
            ChangeGrid();

            gridChangeReady = false;
            moveCount = 0;
        }

        Vector3 newPosition = transform.position + direction * tileSize;

        transform.position = newPosition;

        SnapToGrid();

        //starts movement cooldown
        canMove = false;
        moveTimer = moveCooldown;

        moveCount++;

        //checks the tile the player moved onto
        Tiles.TileType? tileType = CheckTile();

        //checks if the player has moved twice
        if (moveCount >= 2)
        {
            //if the player lands on a conveyor wait until their next manual move to change the grid
            if (tileType == Tiles.TileType.Conveyor)
            {
                gridChangeReady = true;
            }
            else
            {
                ChangeGrid();
                moveCount = 0;
            }
        }
    }

    void ChangeGrid()
    {
        GridMap gridMap = FindAnyObjectByType<GridMap>();

        if (gridMap != null)
        {
            gridMap.ChangeTiles();
        }
    }

    public void SnapToGrid()
    {
        float snappedX = Mathf.Round(transform.position.x / tileSize) * tileSize;
        float snappedZ = Mathf.Round(transform.position.z / tileSize) * tileSize;
        transform.position = new Vector3(snappedX, transform.position.y, snappedZ);
    }

    //check what type of tile the player is standing on
    public Tiles.TileType? CheckTile()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(0.4f, 1f, 0.4f));

        foreach (Collider collider in colliders)
        {
            Tiles tile = collider.GetComponent<Tiles>();

            if (tile != null)
            {
                if (tile.GetTileType() == Tiles.TileType.Hazard)
                {
                    TakeDamage(1);
                }

                if (tile.GetTileType() == Tiles.TileType.Exit)
                {
                    GameManager gameManager = FindAnyObjectByType<GameManager>();

                    if (gameManager != null)
                    {
                        gameManager.WinGame();
                    }
                }

                return tile.GetTileType();
            }
        }

        return null;
    }

    //damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Player Health: " + currentHealth);

        //ends game when health reaches zero
        if (currentHealth <= 0)
        {
            GameManager gameManager = FindAnyObjectByType<GameManager>();

            if (gameManager != null)
            {
                gameManager.LoseGame();
            }
        }
    }
}