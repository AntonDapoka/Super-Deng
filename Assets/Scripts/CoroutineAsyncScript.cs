using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public static class CoroutineAsyncScript
{
    public static Task RunAsync(this MonoBehaviour runner, IEnumerator coroutine)
    {
        var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        runner.StartCoroutine(TrackCompletion(coroutine, completion));
        return completion.Task;
    }

    public static IEnumerator WaitAsync(this Task task)
    {
        while (!task.IsCompleted) yield return null;
        task.GetAwaiter().GetResult();
    }

    private static IEnumerator TrackCompletion(IEnumerator coroutine, TaskCompletionSource<bool> completion)
    {
        try
        {
            yield return coroutine;
        }
        finally
        {
            completion.TrySetResult(true);
        }
    }
}
