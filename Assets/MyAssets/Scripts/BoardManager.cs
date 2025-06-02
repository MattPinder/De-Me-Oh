using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private List<Animator> allTiles;
    private List<Animator> raisedTiles;

    private LayerMask layerMask;

    // How many tiles to raise at a time?
    private int tilesToRaise = 20;

    private string endTileName = "TileJ5";
    private EndTileManager endTile;
    private AudioSource boardAudio;

    private int turnNumber = 0;

    public bool gameOver {  get; private set; }

    [Header("Key Settings")]
    [Tooltip("Key prefab")]
    public GameObject keyPrefab;
    [Tooltip("List of possible key spawn locations")]
    public List<Vector3> keySpawns;

    [Header("UI Settings")]
    [Tooltip("TextMeshPro object that displays the score")]
    public TextMeshProUGUI scoreText;

    [Header("Sound Settings")]
    [Tooltip("Sound effect for walls moving")]
    public AudioClip wallMoveSound;
    [Tooltip("Sound effect for winning")]
    public AudioClip victorySound;

    void Start()
    {
        gameOver = false;

        scoreText.text = "Turns used\n0";

        endTile = GameObject.Find(endTileName).GetComponent<EndTileManager>();
        boardAudio = GetComponent<AudioSource>();

        layerMask = LayerMask.GetMask("PlayerFigure", "Key");
        allTiles = GetComponentsInChildren<Animator>().ToList();
        raisedTiles = new List<Animator>();

        SpawnKey();

        RaiseWalls();
    }

    public void UpdateScore()
    {
        turnNumber++;
        scoreText.text = "Turns used\n" + turnNumber;
    }

    private void SpawnKey()
    {
        int randomKeySpawn = Random.Range(0, keySpawns.Count);
        Instantiate(keyPrefab, keySpawns[randomKeySpawn], keyPrefab.transform.rotation);
    }

    public IEnumerator LowerWalls()
    {
        yield return new WaitForSeconds(1.0f);

        if (endTile.VictoryCheck())
        {
            boardAudio.PlayOneShot(victorySound);
            gameOver = true;
            scoreText.text = "Victory in\n" + turnNumber + " turns!";
            Debug.Log("You win!");
        }
        else
        {
            boardAudio.PlayOneShot(wallMoveSound);

            foreach (Animator raisedTile in raisedTiles)
            {
                raisedTile.SetBool("Raised", false);

                allTiles.Add(raisedTile);
            }

            // Clear raisedTiles AFTER iterating or else it errors out!
            raisedTiles = new List<Animator>();

            RaiseWalls();
        }
    }

    private void RaiseWalls()
    {
        int raisedTileNumber = 0;

        boardAudio.PlayOneShot(wallMoveSound);

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
