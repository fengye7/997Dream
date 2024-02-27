using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEditor;


public static class Utils
{
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    /// <summary>
    /// 工具类缩放协程
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="minScale"></param>
    /// <param name="scaleDuration"></param>
    /// <returns></returns>
    public static IEnumerator ScaleEffect(Transform transform, float minScale, float scaleDuration)
    {
        float originalScale = transform.localScale.x;
        float targetScale = Mathf.Max(originalScale * minScale, 0.001f); //目标缩放值，稍微小于原始值

        // 使用 DoTween 缩放到目标值，并设置完成后的回调
        transform.DOScale(targetScale, scaleDuration).OnComplete(() =>
        {
            // 缩放动画完成后，恢复原始大小
            transform.localScale = new Vector3(originalScale, originalScale, originalScale);
        });
        yield return new WaitForSeconds(scaleDuration);
    }

    /// <summary>
    /// 工具函数实现闪烁
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <param name="blinkDuration"></param>
    /// <param name="blinkCount"></param>
    /// <returns></returns>
    public static IEnumerator BlinkEffect(SpriteRenderer spriteRenderer, float blinkDuration, int blinkCount)
    {
        float originalAlpha = spriteRenderer.color.a;
        float blinkAlpha = 0f;

        for (int i = 0; i < blinkCount; i++)
        {
            // 设置不透明度为目标值，然后再设置为原始值，实现闪烁效果
            spriteRenderer.DOFade(blinkAlpha, blinkDuration / 2);
            yield return new WaitForSeconds(blinkDuration / 2);
            spriteRenderer.DOFade(originalAlpha, blinkDuration / 2);
            yield return new WaitForSeconds(blinkDuration / 2);
        }
    }

    /// <summary>
    ///工具函数实现震动
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="shakeDuration"></param>
    /// <param name="strength"></param>
    /// <param name="vibrato"></param>
    /// <param name="randomness"></param>
    /// <returns></returns>
    public static IEnumerator ShakeEffect(Transform transform, float shakeDuration, float strength, int vibrato, float randomness)
    {
        Vector3 originalPosition = transform.position;

        //使用 DoTween 实现震动效果
        transform.DOShakePosition(shakeDuration, strength, vibrato, randomness);

        yield return new WaitForSeconds(shakeDuration);

        // 震动结束后恢复原始位置
        transform.position = originalPosition;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    // Fisher-Yates 洗牌算法
    public static void ShuffleList(List<SkillData> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            SkillData value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// 游戏结束的总控程序
    /// </summary>
    /// <param name="info"></param>
    public static void GameOver(string info)
    {
        GameObject.FindObjectOfType<DreamSceneAudios>().BGAudioSource.Stop();
        if("123" == info)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}
