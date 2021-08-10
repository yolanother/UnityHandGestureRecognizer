using UnityEngine;


namespace DoubTech.ValemGestures.BoneProviders
{
    public abstract class BoneProvider : MonoBehaviour, IBoneProvider
    {
        public abstract Transform BoneRoot { get; }
        public abstract Transform[] Bones { get; }
        public abstract bool AreBonesReady { get; }
    }

    public interface  IBoneProvider
    {
        public Transform BoneRoot { get; }
        public Transform[] Bones { get; }

        public bool AreBonesReady { get; }
    }
}
