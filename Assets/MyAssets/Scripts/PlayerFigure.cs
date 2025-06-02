using System.Collections;
using UnityEngine;

public class PlayerFigure : MonoBehaviour
{
    private ParticleSystem burstParticles;
    private BoardManager boardManager;
    private AudioSource boardAudio;

    // Distance to move 
    private float tileDistance = 0.141f;
    private Vector3 targetPos;
    private Quaternion targetRotation = Quaternion.identity;
    private float moveSpeed = 0.001f;
    private float rotationSpeed = 1.25f;

    private LayerMask layerMask;

    public bool gotKey { get; private set; }

    [Header("Card Slot")]
    [Tooltip("The socket the cards are played to")]
    public CardSlot cardSlot;

    [Header("Sound Settings")]
    [Tooltip("Sound effect for picking up a key")]
    public AudioClip keyPickupSound;

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
        if (!CheckIllegalMove(moveDirection))
        {
            targetPos = proposedTarget;
            targetRotation = rotateDirection;
            boardManager.UpdateScore();
            StartCoroutine(boardManager.LowerWalls());

            StartCoroutine(cardSlot.ResetSocket());
        }
        else
        {
            // Reject the move with a visual/sound effect
            Debug.Log("Illegal move!");
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
            Debug.Log("Got key!");
            Destroy(other.gameObject);
        }
    }

    public void ParticleBurst()
    {
        burstParticles.Play();
    }
}
