using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UnitInfoTests
    {
        private GameObject gameObj;
        //private UnitInfo unitInfo;

        [SetUp] // Runs before each test
        public void Setup()
        {
            //unitInfo = gameObj.AddComponent<UnitInfo>();
            EquipmentLibrary.InitializeLibrary();
        }
        
        [Test]
        public void RefreshAP_Should_Fully_Restore_Users_CurrentAP()
        {
            Assert.IsNotNull(EquipmentLibrary.Weapons[0], "Weapons library (ID: 0) should return a Short Sword.");
            Assert.IsNotNull(EquipmentLibrary.Armor[100], "Armor Library (ID: 100) should return a Bronze Helm.");
            Assert.IsNotNull(EquipmentLibrary.Accessories[200], "Armor Library (ID: 200) should return a Bloodstone.");
        }

        [Test]
        public void Library_Should_Return_Null_If_ID_Does_Not_Exist()
        {
            Assert.IsFalse(EquipmentLibrary.Weapons.ContainsKey(-1), "Weapons library (ID: -1) should not exist.");
            Assert.IsFalse(EquipmentLibrary.Armor.ContainsKey(10000), "Armor Library (ID: 10000) should not exist.");
            Assert.IsFalse(EquipmentLibrary.Armor.ContainsKey(-3333), "Armor Library (ID: -3333) should not exist.");
        }
    }
}
