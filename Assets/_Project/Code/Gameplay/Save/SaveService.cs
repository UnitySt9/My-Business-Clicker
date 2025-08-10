using System.IO;
using Code.Gameplay.Save.Models;
using UnityEngine;

namespace Code.Gameplay.Save
{
    public class SaveService : ISaveService
    {
        private const string SAVE_FILE_NAME = "gamesave.json";
        private readonly string _savePath;

        public SaveService()
        {
            _savePath = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
        }

        public void SaveGame(GameSaveModel saveData)
        {
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(_savePath, json);
        }

        public GameSaveModel LoadGame()
        {
            if (!File.Exists(_savePath))
                return new GameSaveModel();

            string json = File.ReadAllText(_savePath);
            return JsonUtility.FromJson<GameSaveModel>(json);
        }

        public bool HasSave()
        {
            return File.Exists(_savePath);
        }
    }
} 