using Code.Gameplay.Save.Models;
using UnityEngine;

namespace Code.Gameplay.Save
{
    public class PlayerPrefsSaveService : ISaveService
    {
        private const string SAVE_KEY = "game_save_data";

        public void SaveGame(GameSaveModel saveData)
        {
            string json = JsonUtility.ToJson(saveData, true);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
        }

        public GameSaveModel LoadGame()
        {
            if (!HasSave())
                return new GameSaveModel();

            string json = PlayerPrefs.GetString(SAVE_KEY);
            return JsonUtility.FromJson<GameSaveModel>(json);
        }

        public bool HasSave()
        {
            return PlayerPrefs.HasKey(SAVE_KEY);
        }
    }
} 