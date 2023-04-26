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
    }
}
