using System.Collections;

namespace IGT.Systems {
    public abstract class ExpiringBattleFX : IBattleFX {
        public abstract string Name { get; }
        public abstract int TurnsRemaining { get; protected set; }
        public abstract IEnumerator Inflict(Unit unit);
        public abstract IEnumerator Cycle();
    }
}
