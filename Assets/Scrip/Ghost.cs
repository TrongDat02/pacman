using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Move))]
public class Ghost : MonoBehaviour
{
    public Move move { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehavior initialBehavior;
    public Transform target;
    public int points = 200;

    public AudioSource eatGhost;
    public AudioSource pacmanDie;

    private void Awake()
    {
        move = GetComponent<Move>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.move.ResetState();

        //this.frightened.Disable();
        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior)
        {
            home.Disable();
        }

        if (initialBehavior != null)
        {
            initialBehavior.Enable();
        }
    }
    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled)
            {
                eatGhost.Play();
                FindAnyObjectByType<GameManager>().GhostEaten(this);
            }
            else
            {
                pacmanDie.Play();
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }
}