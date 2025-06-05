using UnityEngine;

public class EndTileManager : MonoBehaviour
{
    private PlayerFigure playerFigure;
    private BoardManager boardManager;
    private LayerMask layerMask;

    void Start()
    {
        playerFigure = GameObject.Find("PlayerFigure").GetComponent<PlayerFigure>();
        boardManager = GameObject.Find("Board").GetComponent<BoardManager>();
        layerMask = LayerMask.GetMask("PlayerFigure");
    }

    // Has the player reached the end tile with the key?
    void Update()
    {
        // Only trigger if not in the game over state, otherwise the sound effect loops!
        if (!boardManager.gameOver)
        {
            if (Physics.Raycast(transform.position, transform.up, 10, layerMask))
            {
                if (!playerFigure.gotKey)
                {
                    Debug.Log("Key still required!");
                }
                else
                {
                    boardManager.GameOver();
                }
            }
        }
    }
}
