// ------------------------------------------------------------------
// NOTE:
// In the EquipmentLibrary.json file, the enum values are read
// as integers in the following order:
//      Head = 0
//      Body = 1
//      Hands = 2
//      Feet = 3
//
// Attempting to use any other values will automatically register
// new equipment as ArmorType.Head by default.
// ------------------------------------------------------------------
public enum ArmorType
{
    Head,
    Body,
    Hands,
    Feet
}
