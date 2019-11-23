using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(RectTransform))]
    public class EQCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject _buttonPrefab;

        private RectTransform _parent;
        private Canvas _parentCanvas;
        private List<GameObject> _allButtonsObjects = new List<GameObject>();
        private List<Button> _allButtons = new List<Button>();

        private void Awake()
        {
            _parent = GetComponent<RectTransform>();
            _parentCanvas = GetComponentInParent<Canvas>();
        }

        public void UpdateCanvas()
        {
            var originPoint = new Vector3(0, _parentCanvas.pixelRect.height/2 - 100);
            for (var i = 0; i < _allButtonsObjects.Count; i++)
                _allButtonsObjects[i].transform.localPosition = originPoint + Vector3.down * 100 * i;
        }

        public void AddNewItem(ConsumableItem item, GameController gc)
        {
            var newButtonObject = Instantiate(_buttonPrefab, _parent);
            var newButton = newButtonObject.GetComponent<Button>();
            newButton.GetComponentInChildren<Text>().text = item.Name;

            newButton.onClick.AddListener(() =>
            {
                item.Apply(gc.UIController.UpgradeManager.CurrentUnit);

                _allButtonsObjects.Remove(newButtonObject);
                _allButtons.Remove(newButton);
                Destroy(newButtonObject);
                UpdateCanvas();
            });

            _allButtonsObjects.Add(newButtonObject);
            _allButtons.Add(newButton);
            UpdateCanvas();
        }
    }
}