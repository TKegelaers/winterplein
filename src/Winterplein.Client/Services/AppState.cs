namespace Winterplein.Client.Services;

public class AppState
{
    private int _playerCount;
    private int _matchCount;

    public int PlayerCount
    {
        get => _playerCount;
        set { _playerCount = value; NotifyStateChanged(); }
    }

    public int MatchCount
    {
        get => _matchCount;
        set { _matchCount = value; NotifyStateChanged(); }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
