using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dypsloom.RhythmTimeline.SceneManagement
{
    public class SceneLoaderAsync : MonoBehaviour
    {
        [SerializeField] protected int m_SceneBuildIndex;

        [ContextMenu("LoadScene")]
        public void LoadSceneAsync()
        {
            LoadSceneAsync(m_SceneBuildIndex);
        }

        private void LoadSceneAsync(int buildIndex)
        {
            SceneManager.LoadSceneAsync(buildIndex);
        }

        public void SetBuildIndex(int buildIndex)
        {
            m_SceneBuildIndex = buildIndex;
        }

        public void ReloadScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadNextScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
