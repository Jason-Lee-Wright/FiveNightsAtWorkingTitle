using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Game State", menuName = "Game State")]
public class GameState : ScriptableObject
{
    public Action onEnterState;

    public Action onExitState;
    public void EnterState() => onEnterState?.Invoke();

    public void ExitState() => onExitState?.Invoke();
}