using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlayingCard : XRGrabInteractable
{
    private PlayerFigure playerFigure;
    private BoardManager boardManager;
    private Color cardColour;

    [Tooltip("Select the card's function")]
    public enum cardAction { North, South, East, West, Wait };
    [SerializeField] private cardAction _cardAction;

    [Tooltip("The card's starting socket")]
    public XRSocketInteractor startSocket;

    [Header("Card Slot")]
    [Tooltip("The socket the cards are played to")]
    public CardSlot cardSlot;

    // Describe the amount the card should rotate/move the player figure
    private Vector3 figureMovement;
    private Quaternion figureRotation;

    void Start()
    {
        playerFigure = GameObject.Find("PlayerFigure").GetComponent<PlayerFigure>();
        boardManager = GameObject.Find("Board").GetComponent<BoardManager>();
        cardColour = gameObject.GetComponent<MeshRenderer>().material.color;

        SetCardBehaviour();
    }


    public void ResetCard()
    {
        // Put the card back into its initial arm holster slot
        cardSlot.gameObject.SetActive(false);

        // Make the card disappear from the card slot, and return it to its initial arm socket
        Vector3 moveToPosition = startSocket.gameObject.transform.position;
        Quaternion rotateToPosition = startSocket.gameObject.transform.rotation;
        gameObject.transform.SetPositionAndRotation(moveToPosition, rotateToPosition);

        cardSlot.gameObject.SetActive(true);
    }

    public void PlayCard()
    {
        if (!boardManager.gameOver)
        {
            PlayerMove();
            ResetCard();
        }
    }

    public void PlayerMove()
    {
        playerFigure.Move(figureMovement, figureRotation);
    }

    private void SetCardBehaviour()
    {
        switch (_cardAction)
        {
            case cardAction.North:
                figureMovement = Vector3.forward;
                figureRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                break;
            case cardAction.South:
                figureMovement = Vector3.back;
                figureRotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                break;
            case cardAction.East:
                figureMovement = Vector3.right;
                figureRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                break;
            case cardAction.West:
                figureMovement = Vector3.left;
                figureRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                break;
            case cardAction.Wait:
                figureMovement = Vector3.zero;
                //figureRotation doesn't change
                break;
            default:
                break;
        }

    }
}