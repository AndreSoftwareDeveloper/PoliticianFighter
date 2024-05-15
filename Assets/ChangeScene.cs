using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ButtonPressed(string buttonName)
    {
        SceneManager.LoadScene("level_1");
    }
}
