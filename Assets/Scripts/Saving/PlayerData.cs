using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("Data Points for Player")]
    public int playerLevel;
    public float currentHealth;
    public float maxHealth;
    public float currentMana;
    public float maxMana;
    public float currentStamina;
    public float maxStamina;
    public float[] playerPosition;
    public float playerRotationY;
    public float cameraRotationX;

    object[] data;

    public PlayerData(object[] dataToSave)
    {
        data = dataToSave;

        playerLevel = (int)data[0];
        currentHealth = (float)data[1];
        maxHealth = (float)data[2];
        currentMana = (float)data[3];
        maxMana = (float)data[4];
        currentStamina = (float)data[5];
        maxStamina = (float)data[6];
        playerPosition = (float[])data[7];
        playerRotationY = (float)data[8];
        cameraRotationX = (float)data[9];
    }

    public object[] GetPlayerData()
    {
        return data;
    }
}
