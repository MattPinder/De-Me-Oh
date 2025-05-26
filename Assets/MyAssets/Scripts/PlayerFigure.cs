using System.Collections;
using UnityEngine;

public class PlayerFigure : MonoBehaviour
{
    private ParticleSystem burstParticles;
    private Animator playerAnimator;

    // Distance to move 
    private float tileDistance = 0.141f;
    private Vector3 targetPos;
    private Quaternion targetRotation = Quaternion.identity;

//    private int moveNumber = 1;

    private LayerMask layerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        burstParticles = transform.Find("BurstParticles").GetComponent<ParticleSystem>();
        playerAnimator = GetComponent<Animator>();
        layerMask = LayerMask.GetMask("BoardEdge", "Wall");

        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.001f);

        // Currently, this only rotates relative to the original rotation, not current rotation
        // FIX THIS!
//        transform.rotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, 1.0f);
    }
    public void Move(int distanceToMove, float degreesToRotate)
    {
        if (degreesToRotate != 0)
        {
            //targetRotation = Quaternion.Euler(0, transform.localRotation.y + degreesToRotate, 0);

            // Change this to RotateTowards (or whichever process determines non-instant movement)
            gameObject.transform.Rotate(0.0f, degreesToRotate, 0.0f);
        }
        if (distanceToMove != 0)
        {
            Vector3 proposedTarget = transform.position + (transform.forward * tileDistance * distanceToMove);

            // Only move if the target is a legal space
            if (!CheckIllegalMove(proposedTarget))
            {
                targetPos = proposedTarget;
            }
            else
            {
                // Reject the move with a visual/sound effect
                Debug.Log("Illegal move!");
            }
        }
    }

    // Determine whether the proposed move is legal (e.g. does it land within the board?)
    private bool CheckIllegalMove(Vector3 proposedPos)
    {
        // Use raycasting to check whether there is a collider in the way of the proposed move
        // Add additional checks when enemies are added
        return Physics.Raycast(transform.position, transform.forward, tileDistance, layerMask);
    }

    public void ChangeParticleColour(Color newColour)
    {
        ParticleSystem.MainModule particleMain = burstParticles.main;
        particleMain.startColor = newColour;
    }

    public void ParticleBurst()
    {
        burstParticles.Play();
        // Triggered by PlayingCard > PlayerMove()
        // Have the player figure make some other action (attack?)
    }
}
