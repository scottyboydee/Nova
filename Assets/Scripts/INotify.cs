using UnityEngine;

public enum NotifyType
{
    None,
    AnimFinished,
    ReturnToPool,
}

public interface INotify
{
    void Notify(NotifyType notification, GameObject context = null);
}
