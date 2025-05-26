using System.Collections;
using UnityEngine;

public class PlayingCard : MonoBehaviour
{
    private string cardName;
    private PlayerFigure playerFigure;
    private BoardManager boardManager;
    private Color cardColour;

    // Check whether the enumeration types can be changed to set the values directly
    public enum cardAction { turnLeft, turnRight, moveForward };
    [SerializeField] private cardAction _cardAction;

    // Describe the amount the card should rotate/move the player figure
    private float figureRotation;
    private int figureMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardName = gameObject.name;
        playerFigure = GameObject.Find("PlayerFigure").GetComponent<PlayerFigure>();
        boardManager = GameObject.Find("Board").GetComponent<BoardManager>();
        cardColour = gameObject.GetComponent<MeshRenderer>().material.color;

        SetCardBehaviour();
    }

    public void ShowSelectedCard()
    {
        playerFigure.ChangeParticleColour(cardColour);
        playerFigure.ParticleBurst();
        PlayerMove();

        StartCoroutine(RaiseWallsWithDelay());
    }

    IEnumerator RaiseWallsWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        boardManager.LowerWalls();
    }

    public void PlayCardAnimation()
    {
        // Play animations and sounds when the card is placed in the socket
    }

    public void PlayerMove()
    {
        playerFigure.Move(figureMovement, figureRotation);
    }

    private void SetCardBehaviour()
    {
        switch (_cardAction)
        {
            case cardAction.turnLeft:
                figureMovement = 1;
                figureRotation = -90.0f;
                break;
            case cardAction.turnRight:
                figureMovement = 1;
                figureRotation = 90.0f;
                break;
            case cardAction.moveForward:
                figureMovement = 1;
                figureRotation = 0;
                break;
            default:
                break;
        }

    }
}