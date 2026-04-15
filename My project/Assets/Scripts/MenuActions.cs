using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuActions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public void IniciarJogo()
    {
        GameController.Init();
        SceneManager.LoadScene(1);
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene(0);
    }
}
