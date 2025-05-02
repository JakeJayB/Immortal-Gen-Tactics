using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        EquipmentLibrary.InitializeLibrary();

        TilemapCreator.RegisterCleanup();
        ChainSystem.RegisterCleanup();
        TurnSystem.RegisterCleanup();
        MapCursor.RegisterCleanup();
        ActionUtility.RegisterCleanup();
        CameraMovement.RegisterCleanup();
        AudioManager.RegisterCleanup();
        BGAudioManager.RegisterCleanup();
        SoundFXManager.RegisterCleanup();
        PartyManager.RegisterCleanup();
        CanvasUI.RegisterCleanup();

        // EquptmentLibrary can't find MemoryManager for whatever reason. So, this will do
        MemoryManager.AddListeners(EquipmentLibrary.Clear);
    }


}
