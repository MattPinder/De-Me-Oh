using UnityEngine;

public class TileManager : MonoBehaviour
{
    private Animator tileAnimator;

    void Start()
    {
        tileAnimator = GetComponent<Animator>();
    }

    public void RaiseLowerTile(bool raiseState)
    {
        tileAnimator.SetBool("Raised", raiseState);
    }
}
