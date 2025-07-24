using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace IGT.Tests.Editor {
    public class UnitInfoTests
    {
        private UnitInfo unitInfo;
        private UnitDefinitionData udd;

        [SetUp]
        public void Setup() {
            unitInfo = new UnitInfo(null);
            udd = UDDLoader.ReadJSON("UDD_Test.json");
            unitInfo.Initialize(udd);
            unitInfo.ResetCurrentStatPoints();
        }

        [Test]
        public void Unit_Stats_Are_Initialized_Properly_With_UDD() {
            List<string> failures = new();
            
            if (unitInfo.baseHP != udd.BaseStats.BaseHP) failures.Add("[BaseHP]: Mismatch");
            if (unitInfo.FinalHP != udd.BaseStats.BaseHP) failures.Add("[FinalHP]: Mismatch");

            if (unitInfo.baseMP != udd.BaseStats.BaseMP) failures.Add("[BaseMP]: Mismatch");
            if (unitInfo.FinalMP != udd.BaseStats.BaseMP) failures.Add("[FinalMP]: Mismatch");
            
            if (unitInfo.baseAP != udd.BaseStats.BaseAP) failures.Add("[BaseAP]: Mismatch");
            if (unitInfo.FinalAP != udd.BaseStats.BaseAP) failures.Add("[FinalAP]: Mismatch");
            
            if (unitInfo.baseAttack != udd.BaseStats.BaseAttack) failures.Add("[BaseAttack]: Mismatch");
            if (unitInfo.FinalAttack != udd.BaseStats.BaseAttack) failures.Add("[FinalAttack]: Mismatch");
            
            if (unitInfo.baseDefense != udd.BaseStats.BaseDefense) failures.Add("[BaseDefense]: Mismatch");
            if (unitInfo.FinalDefense != udd.BaseStats.BaseDefense) failures.Add("[FinalDefense]: Mismatch");
            
            if (unitInfo.baseMagicAttack != udd.BaseStats.BaseMagicAttack) failures.Add("[BaseMagicAttack]: Mismatch");
            if (unitInfo.FinalMagicAttack != udd.BaseStats.BaseMagicAttack) failures.Add("[FinalMagicAttack]: Mismatch");
            
            if (unitInfo.baseMagicDefense != udd.BaseStats.BaseMagicDefense) failures.Add("[BaseMagicDefense]: Mismatch");
            if (unitInfo.FinalMagicDefense != udd.BaseStats.BaseMagicDefense) failures.Add("[FinalMagicDefense]: Mismatch");
            
            if (unitInfo.baseMove != udd.BaseStats.BaseMove) failures.Add("[BaseMove]: Mismatch");
            if (unitInfo.FinalMove != udd.BaseStats.BaseMove) failures.Add("[FinalMove]: Mismatch");
            
            if (unitInfo.baseEvade != udd.BaseStats.BaseEvade) failures.Add("[BaseEvade]: Mismatch");
            if (unitInfo.FinalEvade != udd.BaseStats.BaseEvade) failures.Add("[FinalEvade]: Mismatch");
            
            if (unitInfo.baseSpeed != udd.BaseStats.BaseSpeed) failures.Add("[BaseSpeed]: Mismatch");
            if (unitInfo.FinalSpeed != udd.BaseStats.BaseSpeed) failures.Add("[FinalSpeed]: Mismatch");
            
            if (unitInfo.baseSense != udd.BaseStats.BaseSense) failures.Add("[BaseSense]: Mismatch");
            if (unitInfo.FinalSense != udd.BaseStats.BaseSense) failures.Add("[FinalSense]: Mismatch");

            if (unitInfo.UnitAffiliation != udd.UnitAffiliation) failures.Add("[UnitAffiliation]: Mismatch");
            
            if (failures.Any())
                Assert.Fail(string.Join("\n", failures));
        }

        [Test]
        public void Current_Stats_Are_Correctly_Reset_After_Initialization() {
            List<string> failures = new();
            
            if (unitInfo.currentHP != unitInfo.FinalHP) failures.Add("[CurrentHP]: Mismatch");
            if (unitInfo.currentMP != unitInfo.FinalMP) failures.Add("[CurrentMP]: Mismatch");
            if (unitInfo.currentAP != unitInfo.FinalAP) failures.Add("[CurrentAP]: Mismatch");
            if (unitInfo.currentCT != 0) failures.Add("[CurrentCT]: Mismatch");
            
            if (failures.Any())
                Assert.Fail(string.Join("\nAfter Initialization =>\n", failures));
        }

        [Test]
        public void Current_Stats_Are_Correctly_Reset_After_Damage_Calculation() {
            List<string> failures = new();
            
            DamageCalculator.DealFixedHPDamage(5, unitInfo);
            DamageCalculator.DealFixedMPDamage(5, unitInfo);
            DamageCalculator.DealFixedAPDamage(1, unitInfo);
            
            unitInfo.ResetCurrentStatPoints();
            if (unitInfo.currentHP != unitInfo.FinalHP) failures.Add("[CurrentHP]: Mismatch");
            if (unitInfo.currentMP != unitInfo.FinalMP) failures.Add("[CurrentMP]: Mismatch");
            if (unitInfo.currentAP != unitInfo.FinalAP) failures.Add("[CurrentAP]: Mismatch");
            if (unitInfo.currentCT != 0) failures.Add("[CurrentCT]: Mismatch");
            
            if (failures.Any())
                Assert.Fail(string.Join("\nAfter Damage Calculation =>\n", failures));
        }
        
        [Test]
        public void Vector2CellLocation_Returns_XZ_Format_Of_Units_CellLocation() {
            unitInfo.CellLocation = new Vector3Int(1, 2, 3);
            Vector2Int test = unitInfo.Vector2CellLocation();
            Assert.IsTrue(test.x == unitInfo.CellLocation.x);
            Assert.IsTrue(test.y == unitInfo.CellLocation.z);
        }
    }
}
