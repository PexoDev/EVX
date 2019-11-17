using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectBodyPoolItem<T>
    {
        public T Object { get; set; }

        public GameObject Body { get; set; }

        public SpriteRenderer SpriteRenderer { get;}

        public bool InUse { get; set; }

        public ObjectBodyPoolItem(GameObject _objectPrefab, T @object, Transform parent)
        {
            Object = @object;
            Body = UnityEngine.Object.Instantiate(_objectPrefab, parent);
            SpriteRenderer = Body.GetComponent<SpriteRenderer>();
            Body.SetActive(false);
        }
    }
}