using UnityEngine;

namespace IGT.Systems
{
    public class TurnCountCalculator : MonoBehaviour {
        public const int CT_MAX_LIMIT = 1000;
        
        public static void GiveMaxCT(UnitInfo unitInfo) {
            unitInfo.currentCT = CT_MAX_LIMIT;
        }
    }
}
