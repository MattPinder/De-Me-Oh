using System.Collections;
using UnityEngine;

public class PlayerFigure : MonoBehaviour
{
    private ParticleSystem burstParticles;
    private BoardManager boardManager;
    private AudioSource boardAudio;

    // Distance to move 
    private float tileDistance = 1.604f;
    private Vector3 targetPos;
    private Quaternion targetRotation = Quaternion.identity;
    private float moveSpeed = 0.02f;
    private float rotationSpeed = 2.5f;

    private LayerMask layerMask;

    public bool gotKey { get; private set; }

    [Header("Card Slot")]
    [Tooltip("The socket the cards are played to")]
    public CardSlot cardSlot;

    [Header("Sound Settings")]
    [Tooltip("Sound effect for the player figure moving")]
    public AudioClip playerMove;
    [Tooltip("Sound effect for picking up a key")]
    public AudioClip keyPickupSound;
    [Tooltip("Sound effect for attempting an illegal move")]
    public AudioClip illegalMoveAudio;

    void Start()
    {
        gotKey = false;

        burstParticles = transform.Find("BurstParticles").GetComponent<ParticleSystem>();
        boardManager = GameObject.Find("Board").GetComponent<BoardManager>();
        boardAudio = GameObject.Find("Board").GetComponent<AudioSource>();

        layerMask = LayerMask.GetMask("BoardEdge", "Wall");

        targetPos = transform.position;
    }

    // Move and rotate the figure in the relevant direction
    void Update()
    {
        transform.SetPositionAndRotation(Vector3.MoveTowards(transform.position, targetPos, moveSpeed),
            Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed));
    }

    // Update the movement and rotation direction, if the proposed move is legal
    public void Move(Vector3 moveDirection, Quaternion rotateDirection)
    {
        Vector3 proposedTarget = transform.position + (moveDirection * tileDistance);

        if (moveDirection == Vector3.zero)
        {
            // If Wait card is played, trigger LowerWalls() immediately
            boardManager.UpdateScore();
            StartCoroutine(cardSlot.ResetSocket());
            StartCoroutine(boardManager.LowerWalls());
        }
        else if (!CheckIllegalMove(moveDirection))
        {
            targetPos = proposedTarget;
            targetRotation = rotateDirection;
            boardAudio.PlayOneShot(playerMove);
            boardManager.UpdateScore();
            StartCoroutine(cardSlot.ResetSocket());
        }
        else
        {
            // Reject the move with a visual/sound effect
            boardAudio.PlayOneShot(illegalMoveAudio);
        }
    }

    // Use raycasting to check whether there is a collider in the way of the proposed move
    private bool CheckIllegalMove(Vector3 moveDir)
    {
        return Physics.Raycast(transform.position, moveDir, tileDistance, layerMask);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            boardAudio.PlayOneShot(keyPickupSound);
            burstParticles.Play();
            gotKey = true;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Tile"))
        {
            StartCoroutine(boardManager.LowerWalls());
        }
    }

    public void ParticleBurst()
    {
        burstParticles.Play();
    }
}
