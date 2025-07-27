using System.Collections;

namespace IGT.Systems {
    public abstract class InstantBattleFX : IBattleFX {
        public abstract string Name { get; }
        public abstract IEnumerator Inflict(Unit unit);
    }
}
