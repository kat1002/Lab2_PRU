using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    private readonly static UnityEvent _onGameOver = new UnityEvent();
    private readonly static UnityEvent _onGamePause = new UnityEvent();
    private readonly static UnityEvent _onGameUnPause = new UnityEvent();
    private readonly static UnityEvent _onCreateNextPlatform = new UnityEvent();
    private readonly static UnityEvent _onEarnScore = new UnityEvent();
    private readonly static UnityEvent<int> _onUpdateScore = new UnityEvent<int>();

    public static UnityEvent OnGameOver { get => _onGameOver; }
    public static UnityEvent OnGamePause { get => _onGamePause; }
    public static UnityEvent OnGameUnPause { get => _onGameUnPause; }
    public static UnityEvent OnEarnScore { get => _onEarnScore; }
    public static UnityEvent<int> OnUpdateScore { get => _onUpdateScore; }
    public static UnityEvent OnCreateNextPlatform { get => _onCreateNextPlatform; }
}
