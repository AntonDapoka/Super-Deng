using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimationManagerScript : MonoBehaviour
{
    [Header("Distance Settings")]
    [SerializeField] private float distanceMoveImage = 900f;
    [SerializeField] private float distanceMoveButtons = 300f;

    [Header("Duration Settings")]
    [SerializeField] private float durationMoveImage = 2f;
    [SerializeField] private float durationMoveButtons = 1f;
    [SerializeField] private float durationSwitchButtons = 1f;

    [Header("UI Curves")]
    [SerializeField] private AnimationCurve curveMoveSettings;

    private class ActiveAnimation
    {
        public Coroutine coroutine;
        public TaskCompletionSource<bool> completion;
    }

    private readonly Dictionary<RectTransform, ActiveAnimation> activeAnimations = new();

    public void ShowPanel(RectTransform panel)
    {
        if (panel == null) return;
        StartAnimation(panel, MovePanel(panel, true));
    }

    public void HidePanel(RectTransform panel)
    {
        if (panel == null) return;
        StartAnimation(panel, MovePanel(panel, false));
    }

    public async Task ChangeButtonsAsync(Button[] buttonsToHide, Button[] buttonsToShow)
    {
        await HideButtonsAsync(buttonsToHide);
        await this.RunAsync(WaitSeconds(durationSwitchButtons));
        ShowButtons(buttonsToShow);
    }

    public void ShowButtons(Button[] buttons)
    {
        if (buttons == null)return;

        foreach (Button button in buttons)
        {
            if (button == null) continue;
            if (!button.TryGetComponent<RectTransform>(out var rect)) continue;
            StartAnimation(rect, ShowUI(rect));
        }
    }

    public Task HideButtonsAsync(Button[] buttons)
    {
        if (buttons == null) return Task.CompletedTask;

        var animations = new List<Task>();
        foreach (Button button in buttons)
        {
            if (button == null) continue;
            if (!button.TryGetComponent<RectTransform>(out var rect)) continue;
            animations.Add(StartAnimation(rect, HideUI(rect)));
        }
        return Task.WhenAll(animations);
    }

    private Task StartAnimation(RectTransform rect, IEnumerator animation)
    {
        StopAnimation(rect);

        var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        Coroutine coroutine = StartCoroutine(TrackAnimation(rect, animation, completion));
        activeAnimations[rect] = new ActiveAnimation { coroutine = coroutine, completion = completion };
        return completion.Task;
    }

    private IEnumerator TrackAnimation(RectTransform rect, IEnumerator animation, TaskCompletionSource<bool> completion)
    {
        try
        {
            yield return animation;
        }
        finally
        {
            RemoveAnimation(rect);
            completion.TrySetResult(true);
        }
    }

    private void StopAnimation(RectTransform rect)
    {
        if (activeAnimations.TryGetValue(rect, out ActiveAnimation active))
        {
            // StopCoroutine aborts the iterator without running its finally block,
            // so the pending task must be completed here or awaiters hang forever.
            if (active.coroutine != null) StopCoroutine(active.coroutine);
            active.completion.TrySetResult(false);
            activeAnimations.Remove(rect);
        }
    }

    private IEnumerator ShowUI(RectTransform rect)
    {
        rect.gameObject.SetActive(true);

        Vector2 start = rect.anchoredPosition;
        Vector2 target = start + distanceMoveButtons * Vector2.up;

        yield return AnimatePosition(rect, start, target, durationMoveButtons);

        RemoveAnimation(rect);
    }

    private IEnumerator HideUI(RectTransform rect)
    {
        Vector2 start = rect.anchoredPosition;
        Vector2 target = start + distanceMoveButtons * Vector2.down;

        yield return AnimatePosition(rect,start, target,durationMoveButtons);

        rect.gameObject.SetActive(false);

        RemoveAnimation(rect);
    }

    private IEnumerator MovePanel(RectTransform panel, bool show)
    {
        panel.gameObject.SetActive(true);

        Vector2 start = panel.anchoredPosition;
        Vector2 target = show ? start + distanceMoveImage * Vector2.down : start - distanceMoveImage * Vector2.down;

        yield return AnimatePosition(panel, start, target, durationMoveImage);

        panel.gameObject.SetActive(show);
        RemoveAnimation(panel);
    }

    private IEnumerator AnimatePosition(RectTransform rect, Vector2 start, Vector2 target, float duration)
    {
        if (duration <= 0f)
        {
            rect.anchoredPosition = target;
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float progress = curveMoveSettings.Evaluate(t);

            rect.anchoredPosition = Vector2.LerpUnclamped(start, target, progress);

            yield return null;
        }
        rect.anchoredPosition = target;
    }

    private static IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private void RemoveAnimation(RectTransform rect)
    {
        activeAnimations.Remove(rect);
    }

    private void OnDisable()
    {
        foreach (ActiveAnimation active in activeAnimations.Values)
        {
            if (active.coroutine != null) StopCoroutine(active.coroutine);
            active.completion.TrySetResult(false);
        }

        activeAnimations.Clear();
    }
}