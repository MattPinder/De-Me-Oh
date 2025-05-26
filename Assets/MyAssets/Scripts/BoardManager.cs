using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    private List<Animator> allTiles;
    private List<Animator> raisedTiles;

    private LayerMask layerMask;

    // How many tiles to raise at a time?
    private int tilesToRaise = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layerMask = LayerMask.GetMask("PlayerFigure");
        allTiles = GetComponentsInChildren<Animator>().ToList();
        raisedTiles = new List<Animator>();

        RaiseWalls();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LowerWalls()
    {
        foreach (Animator raisedTile in raisedTiles)
        {
            raisedTile.SetBool("Raised", false);

            allTiles.Add(raisedTile);
        }

        // Clear raisedTiles AFTER iterating or else it errors out!
        raisedTiles = new List<Animator>();

        RaiseWalls();
    }

    private void RaiseWalls()
    {
        int raisedTileNumber = 0;

        while (raisedTileNumber < tilesToRaise)
        {
            int randomIndex = Random.Range(0, allTiles.Count);  // Generate a random number
            Animator randomTile = allTiles[randomIndex];        // Access the relevant random tile's Animator

            // Only raise the tile if it doesn't have something on it
            if (!Physics.Raycast(allTiles[randomIndex].transform.position, transform.up, 1, layerMask))
            {
                randomTile.SetBool("Raised", true);

                raisedTiles.Add(randomTile);                        // Add the random tile to raisedTiles
                allTiles.Remove(randomTile);                        // Remove the random tile from allTiles

                raisedTileNumber++;
            }
        }
    }
}
