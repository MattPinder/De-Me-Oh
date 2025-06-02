using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;

// CLASS FOR THE CARD SOCKET, ALLOWING IT TO EXECUTE THE FUNCTIONS OF CARDS PLACED ON IT
public class CardSlot : XRSocketInteractor
{
    private float cooldownTime = 2.5f;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Access the interacting card's methods
        IXRSelectInteractable selectedCard = args.interactableObject;
        PlayingCard cardFunction = selectedCard.transform.gameObject.GetComponent<PlayingCard>();

        // Play the card
        cardFunction.PlayCard();
    }

    public IEnumerator ResetSocket()
    {
        socketActive = false;
        yield return new WaitForSeconds(cooldownTime);
        socketActive = true;
    }
}
