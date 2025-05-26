using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// CLASS FOR THE CARD SOCKET, ALLOWING IT TO EXECUTE THE FUNCTIONS OF CARDS PLACED ON IT
public class CardSlot : XRSocketInteractor
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Access the interacting card's methods
        IXRSelectInteractable selectedCard = args.interactableObject;
        PlayingCard cardFunction = selectedCard.transform.gameObject.GetComponent<PlayingCard>();

        // Change this to select what to trigger in the played card
        cardFunction.ShowSelectedCard();
    }
}
