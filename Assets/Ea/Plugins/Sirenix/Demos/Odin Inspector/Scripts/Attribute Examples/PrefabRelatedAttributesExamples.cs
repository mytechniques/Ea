namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class PrefabRelatedAttributesExamples : MonoBehaviour
    {
        [HideInPrefabAssets]
        public int HiddenInPrefabAssets;

        [HideInPrefabInstances]
        public int HiddenInPrefabInstances;

        [HideInPrefabs]
        public int HiddenInPrefabs;

        [HideInNonPrefabs]
        public int HiddenInNonPrefabs;

        [DisableInPrefabAssets]
        public int DisabledInPrefabAssets;

        [DisableInPrefabInstances]
        public int DisabledInPrefabInstances;

        [DisableInPrefabs]
        public int DisabledInPrefabs;

        [DisableInNonPrefabs]
        public int DisabledInNonPrefabs;
    }
}