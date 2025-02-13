public enum NotifyType
{
    None,
    AnimFinished,
}

public interface INotify
{
    void Notify(NotifyType notification);
}
