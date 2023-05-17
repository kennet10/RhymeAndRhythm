using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Dypsloom.RhythmTimeline.SceneManagement
{
    public class SceneLoaderAsync : MonoBehaviour
    {
        [SerializeField] protected int m_SceneBuildIndex;

        public Animator sceneTransitionAnimator;

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

        public void LoadSceneAsyncWithTransition()
        {
            StartCoroutine("LoadSceneWithTransition");
        }

        public void ReloadScene()
        {
            StartCoroutine("ReloadSceneWithTransition");
        }

        public void LoadNextScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private IEnumerator LoadSceneWithTransition()
        {
            sceneTransitionAnimator.SetTrigger("Start");

            yield return new WaitForSeconds(1f);

            LoadSceneAsync();
        }

        private IEnumerator ReloadSceneWithTransition()
        {
            sceneTransitionAnimator.SetTrigger("Reload");

            yield return new WaitForSeconds(1f);

            LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
