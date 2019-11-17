using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Stack<Action> OnClickActions { get; set; } = new Stack<Action>();

        public void OnPointerClick(PointerEventData eventData)
        {
            if(OnClickActions.Count > 0) OnClickActions.Peek().Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }
    }
}