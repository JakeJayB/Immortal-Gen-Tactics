using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UnitInfoTests
    {
        private UnitInfo unitInfo;
        private UnitDefinitionData udd;

        [SetUp]
        public void Setup() {
            unitInfo = new UnitInfo(null);
            udd = UDDLoader.ReadJSON("UDD_Test.json");
        }
    }
}
