using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    private Vector2 movement;

    internal int coinsCount = 0;
    internal GridManager gridManager;

    [SerializeField] float walkSpeed;
    [SerializeField] TMPro.TextMeshProUGUI coinText;
    [SerializeField] LevelLoader levelLoader;

    internal int currentLevel;
    internal Tile currentTile;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        transform.localScale = new Vector3(4f, 4f, 1f);
    }

    void FixedUpdate()
    {
        checkMovement();
    }

    void checkMovement()
    {
        Vector2 futurePosition = body.position + movement * walkSpeed * Time.fixedDeltaTime;
        if (gridManager.checkPlayerMovementLimit(futurePosition))
        {

            body.MovePosition(futurePosition);

            if (movement.x < -0.01f)
            {
                this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                this.gameObject.transform.localScale = new Vector3(1, 1, 1);
            }

            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("Speed", movement.sqrMagnitude);

            if(currentTile != null)
            {
                if(movement.sqrMagnitude > 0)
                {
                    if (!currentTile.GetComponent<AudioSource>().isPlaying)
                    {
                        currentTile.GetComponent<AudioSource>().Play();
                    }
                }
                else
                {
                    currentTile.GetComponent<AudioSource>().Stop();
                }

            }
        }
    }

    internal void setCoinsAmount(int amount)
    {
        this.coinsCount = amount;
        coinText.text = amount + "/" + gridManager.getCoinTotal();

        if (amount >= gridManager.getCoinTotal())
        {
            levelLoader.loadNextLevel(currentLevel + 1);
        }
    }

}