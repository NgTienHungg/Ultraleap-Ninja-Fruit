using BaseSource;
using UnityEngine.SceneManagement;

namespace _GameDemo.Scripts.UI
{
    public class Button_ReloadScene : UIButtonBase
    {
        protected override void OnClick()
        {
            SceneManager.LoadScene(0);
        }
    }
}