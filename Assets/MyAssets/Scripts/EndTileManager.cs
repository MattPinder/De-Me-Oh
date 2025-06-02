using UnityEngine;

public class EndTileManager : MonoBehaviour
{
    private PlayerFigure playerFigure;
    private LayerMask layerMask;

    void Start()
    {
        playerFigure = GameObject.Find("PlayerFigure").GetComponent<PlayerFigure>();
        layerMask = LayerMask.GetMask("PlayerFigure");
    }

    // Has the player reached the end tile with the key?
    public bool VictoryCheck()
    {
        if (Physics.Raycast(transform.position, transform.up, 1, layerMask))
        {
            if (!playerFigure.gotKey)
            {
                Debug.Log("Key still required!");
                return false;
            }
            else
            {
                Debug.Log("Victory!");
                return true;
            }
        }
        else
        {
            return false;
        }
    }
}
