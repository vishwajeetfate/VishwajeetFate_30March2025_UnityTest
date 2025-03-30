using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    private bool isFalling = false;
    public float fallThreshold = 2f; // 2 seconds before game over

    void Start()
    {
        StartCoroutine(CheckFalling());
    }

    IEnumerator CheckFalling()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f); // Reduce frequency

            if (!Physics.Raycast(transform.position, Vector3.down, 1f))
            {
                isFalling = true;
                yield return new WaitForSeconds(fallThreshold);

                if (!Physics.Raycast(transform.position, Vector3.down, 1f))
                {
                    Debug.Log("Game Over! Player fell.");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
            else
            {
                isFalling = false;
            }
        }
    }
}
