using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEntityEventSubscriber
{
    void SubscribeToLevelUp(UnityAction<int> callback);
    void UnsubscribeFromLevelUp(UnityAction<int> callback);
    void LevelUp();
}