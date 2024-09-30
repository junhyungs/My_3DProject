using UnityEngine.AI;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedGhoul : SharedVariable<Ghoul_BT>
    {
        public static implicit operator SharedGhoul(Ghoul_BT value) { return new SharedGhoul { mValue = value }; }
    }
}
