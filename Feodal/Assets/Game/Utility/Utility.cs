using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Utility
{
    public class Utility
    {
        internal static async Task<AsyncOperation> LoadSceneAsync(string scene, CancellationToken token, ApplicationContext context)
        {
            var staticLoadTime = 0.5f;
            var totalLoadTime = 0.0f;
            string sceneName = scene;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            asyncOperation.allowSceneActivation = false;
            while (asyncOperation.progress < 0.9f && !token.IsCancellationRequested)
            {
                await Task.Yield();
                context.OnLoadingGame?.Invoke(Mathf.Clamp(totalLoadTime / staticLoadTime, 0.0f, 0.5f));
                totalLoadTime += Time.deltaTime;
            }
            while (totalLoadTime < staticLoadTime && !token.IsCancellationRequested)
            {
                context.OnLoadingGame?.Invoke(Mathf.Clamp(totalLoadTime / staticLoadTime, 0.0f, 0.95f));
                await Task.Yield();
                totalLoadTime += Time.deltaTime;
            }
            if (!token.IsCancellationRequested)
            {
                await Task.Yield();
                context.OnLoadingGame?.Invoke(Mathf.Clamp(0.0f, 1f, totalLoadTime / staticLoadTime));
                asyncOperation.allowSceneActivation = true;
            }
            return asyncOperation;
        }
    }
}