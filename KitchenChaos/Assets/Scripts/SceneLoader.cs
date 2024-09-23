using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static Scene targetScene;

    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }

    public static void LoadScene(Scene targetScene)
    {
        SceneLoader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }
    public static void SceneLoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
