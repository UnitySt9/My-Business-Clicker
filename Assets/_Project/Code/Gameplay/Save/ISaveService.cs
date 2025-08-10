using Code.Gameplay.Save.Models;

namespace Code.Gameplay.Save
{
    public interface ISaveService
    {
        void SaveGame(GameSaveModel saveData);
        GameSaveModel LoadGame();
        bool HasSave();
    }
}