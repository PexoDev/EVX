using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectBodyPool<TType, TSpritesMatchingType> where TType: Entity
    {
        public static ObjectBodyPool<TType, TSpritesMatchingType> Instance { get; private set; }

        public Transform ParentTransform { get; set; }

        private readonly GameObject _objectBodyPrefab;
        private readonly Dictionary<TSpritesMatchingType, Sprite> _sprites;
        public readonly List<ObjectBodyPoolItem<TType>> Pool = new List<ObjectBodyPoolItem<TType>>();

        public ObjectBodyPool(GameObject objectBodyPrefab, Transform parentTransform, Dictionary<TSpritesMatchingType, Sprite> sprites)
        {
            ParentTransform = parentTransform;
            _objectBodyPrefab = objectBodyPrefab;
            _sprites = sprites;

            if (Instance == null) Instance = this;
        }

        public ObjectBodyPoolItem<TType> GetObject(TType @object, Action returnAction, TSpritesMatchingType spriteKey)
        {
            var item = Pool.FirstOrDefault(p => !p.InUse);

            if (item == null)
            {
                var newInstance = CreateNewInstance(@object);
                Pool.Add(newInstance);
                item = Pool.Last();
            }
            else
            {
                item.Object = @object;
            }

            item.Body.transform.position = @object.Position;
            item.Body.transform.rotation = Quaternion.Euler(@object.Rotation);

            item.Body.SetActive(true);

            if (item.Body != null && item.SpriteRenderer != null && spriteKey != null)
                item.SpriteRenderer.sprite =_sprites[spriteKey];

            item.InUse = true;
            return item;
        }

        public void UpdateAllBodies()
        {
            foreach (var bodyPoolItem in Pool.Where(item=>item.InUse).ToArray())
            {
                bodyPoolItem.Body.transform.position = bodyPoolItem.Object.Position;
                bodyPoolItem.Body.transform.rotation = Quaternion.Euler(bodyPoolItem.Object.Rotation);
            }
        }

        protected ObjectBodyPoolItem<TType> CreateNewInstance(TType @object)
        {
            return new ObjectBodyPoolItem<TType>(_objectBodyPrefab, @object, ParentTransform);
        }

        public void ReturnToPool(TType @object)
        {
            var item = Pool.FirstOrDefault(it => it.Object == @object);
            if (item == null) return;
            item.Body.SetActive(false);
            item.SpriteRenderer.sprite = null;
            item.Object = null;
            item.InUse = false;
    }
    }
}