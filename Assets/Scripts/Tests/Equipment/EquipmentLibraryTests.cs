using System.IO;
using NUnit.Framework;

namespace Tests
{
    public class EquipmentLibraryTests
    {
        private EquipmentLibrary equipmentLibrary;

        [SetUp] // Runs before each test
        public void Setup()
        {
            equipmentLibrary = new EquipmentLibrary();
            EquipmentLibrary.InitializeLibrary();
        }
    
        [Test]
        public void Equipment_Library_File_Exists()
        {
            Assert.IsTrue(File.Exists(EquipmentLibrary.DEFAULT_DIRECTORY + EquipmentLibrary.FILE_NAME));
        }
        
        [Test]
        public void Library_Should_Return_Equipment_If_ID_Exists()
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
