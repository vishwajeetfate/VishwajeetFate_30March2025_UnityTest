using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Settings")]
    [SerializeField] private int totalCubes;
    [SerializeField] private Text cubeCounterText;

    private int collectedCubes = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        totalCubes = GameObject.FindGameObjectsWithTag("Collectible").Length;
        UpdateCubeUI();
    }

    public void CollectCube()
    {
        collectedCubes++;
        UpdateCubeUI();

        if (collectedCubes >= totalCubes)
        {
            WinGame();
        }
    }

    void UpdateCubeUI()
    {
        cubeCounterText.text = "Cubes: " + collectedCubes + "/" + totalCubes;
    }

    void WinGame()
    {
        Debug.Log("All Cubes Collected! You Win!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart game
    }
}
