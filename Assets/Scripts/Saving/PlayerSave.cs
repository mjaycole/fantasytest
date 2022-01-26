using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerMovement movementScript = null;
    [SerializeField] PlayerLook lookScript = null;
    [SerializeField] PlayerHealth healthScript = null;
    [SerializeField] QuestList questList = null;

    #region Saving
    public void Save()
    {
        SavePlayerData();
    }

    public void SavePlayerData()
    {
        object[] dataToSave = new object[10];

        dataToSave[0] = 1;
        dataToSave[1] = healthScript.GetCurrentHealth();
        dataToSave[2] = healthScript.GetMaxHealth();
        dataToSave[3] = 10f;
        dataToSave[4] = 10f;
        dataToSave[5] = 10f;
        dataToSave[6] = 10f;

        float[] playerPos = new float[3];
        playerPos[0] = transform.position.x;
        playerPos[1] = transform.position.y;
        playerPos[2] = transform.position.z;

        dataToSave[7] = playerPos;
        dataToSave[8] = transform.rotation.y;
        dataToSave[9] = lookScript.GetLookRotation();


        SaveSystem.SavePlayer(dataToSave);
    }
    #endregion

    #region Loading
    public void Load()
    {
        LoadPlayerData();
    }

    public void LoadPlayerData()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        object[] dataToLoad = data.GetPlayerData();

        healthScript.SetCurrentHealth((float)dataToLoad[1]);
        healthScript.SetMaxHealth((float)dataToLoad[2]);

        Vector3 position;
        position.x = data.playerPosition[0];
        position.y = data.playerPosition[1];
        position.z = data.playerPosition[2];

        transform.position = position;

        transform.rotation = Quaternion.Euler(new Vector3(0, data.playerRotationY, 0));
        lookScript.SetLookRotation(data.cameraRotationX);
    }

    public void LoadQuestData()
    {

    }
    #endregion
}
