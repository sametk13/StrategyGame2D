using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SKUtils.ScriptableSystem
{
    [CreateAssetMenu(menuName = "SkUtils/ScriptableSystem/ScriptableActionGameObject")]
    public class ScriptableActionGameObject : ScriptableObject
    {
        [ReadOnly] public UnityEvent<ProductData> ActionEvent;

        [ContextMenu("Call Action")] // Utilised to create a button

        public void AddListener(UnityAction<ProductData> action)
        {
            ActionEvent?.AddListener(action);
        }
        public void RemoveListener(UnityAction<ProductData> action)
        {
            ActionEvent?.RemoveListener(action);
        }
        public void CallAction(ProductData go)
        {           
            ActionEvent?.Invoke(go);
        }
    }
}
