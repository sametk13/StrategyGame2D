using UnityEngine;
using UnityEngine.Events;

namespace SKUtils.ScriptableSystem
{
    public class ScriptableSystemGameObjectSetter : MonoBehaviour
    {
        [SerializeField] UnityEvent<ProductData> unityAction;
        [SerializeField] ScriptableActionGameObject scriptableActionGameObject;

        private void OnEnable()
        {
            scriptableActionGameObject.ActionEvent.AddListener(CallAction); // Invoking unityAction when calling Scriptable Action
        }
        private void OnDisable()
        {
            scriptableActionGameObject.ActionEvent.RemoveListener(CallAction); 
        }

        public void CallAction(ProductData data)
        {
            unityAction?.Invoke(data);
        }
    }
}
