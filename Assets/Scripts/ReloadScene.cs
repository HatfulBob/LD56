using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    [SerializeField] private GameObject reloadPanel;
    
    public void ShowReloadMenu()
    {
        Time.timeScale = 0;
        reloadPanel.gameObject.SetActive(true);
    }
    
    public void Reload()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void DontReload()
    {
        reloadPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
