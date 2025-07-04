using UnityEngine.Events;

public static class ExUnityEvent
{
    public static void AddListenerOnce(this UnityEvent unityEvent, UnityAction call)
    {
        UnityAction wrapperCall = null;
        wrapperCall = () =>
        {
            call?.Invoke();
            unityEvent.RemoveListener(wrapperCall);
        };
        unityEvent.AddListener(wrapperCall);
    }

    public static void AddListenerOnce<T0>(this UnityEvent<T0> unityEvent, UnityAction<T0> call)
    {
        UnityAction<T0> wrapperCall = null;
        wrapperCall = (arg0) =>
        {
            call?.Invoke(arg0);
            unityEvent.RemoveListener(wrapperCall);
        };
        unityEvent.AddListener(wrapperCall);
    }

    public static void AddListenerOnce<T0, T1>(this UnityEvent<T0, T1> unityEvent, UnityAction<T0, T1> call)
    {
        UnityAction<T0, T1> wrapperCall = null;
        wrapperCall = (arg0, arg1) =>
        {
            call?.Invoke(arg0, arg1);
            unityEvent.RemoveListener(wrapperCall);
        };
        unityEvent.AddListener(wrapperCall);
    }

    public static void AddListenerOnce<T0, T1, T2>(this UnityEvent<T0, T1, T2> unityEvent, UnityAction<T0, T1, T2> call)
    {
        UnityAction<T0, T1, T2> wrapperCall = null;
        wrapperCall = (arg0, arg1, arg2) =>
        {
            call?.Invoke(arg0, arg1, arg2);
            unityEvent.RemoveListener(wrapperCall);
        };
        unityEvent.AddListener(wrapperCall);
    }

    public static void AddListenerOnce<T0, T1, T2, T3>(this UnityEvent<T0, T1, T2, T3> unityEvent, UnityAction<T0, T1, T2, T3> call)
    {
        UnityAction<T0, T1, T2, T3> wrapperCall = null;
        wrapperCall = (arg0, arg1, arg2, arg3) =>
        {
            call?.Invoke(arg0, arg1, arg2, arg3);
            unityEvent.RemoveListener(wrapperCall);
        };
        unityEvent.AddListener(wrapperCall);
    }
}