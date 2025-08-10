using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Code.UI
{
    /// <summary>
    /// Компонент для кнопки сброса прогресса игры
    /// </summary>
    public class ResetProgressButton : MonoBehaviour
    {
        [SerializeField] private Button _resetButton;
        [SerializeField] private bool _requireConfirmation = true;
        
        private const string SAVE_KEY = "game_save_data";

        private void Start()
        {
            if (_resetButton == null)
                _resetButton = GetComponent<Button>();
                
            if (_resetButton != null)
            {
                _resetButton.onClick.AddListener(OnResetButtonClicked);
            }
        }

        private void OnDestroy()
        {
            if (_resetButton != null)
            {
                _resetButton.onClick.RemoveListener(OnResetButtonClicked);
            }
        }

        private void OnResetButtonClicked()
        {
            if (_requireConfirmation)
            {
                bool confirmed = ShowConfirmationDialog();
                if (!confirmed)
                    return;
            }

            ResetProgress();
        }

        private bool ShowConfirmationDialog()
        {
            #if UNITY_EDITOR
            return UnityEditor.EditorUtility.DisplayDialog(
                "Сброс прогресса",
                "Вы уверены, что хотите сбросить весь прогресс? Это действие нельзя отменить!",
                "Да, сбросить",
                "Отмена"
            );
            #else
            Debug.Log("Сброс прогресса подтвержден");
            return true;
            #endif
        }

        private void ResetProgress()
        {
            try
            {
                PlayerPrefs.DeleteKey(SAVE_KEY);
                PlayerPrefs.Save();
                
                Debug.Log("Прогресс успешно сброшен!");
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка при сбросе прогресса: {e.Message}");
            }
        }

        /// <summary>
        /// Публичный метод для вызова сброса из других скриптов
        /// </summary>
        public void ResetProgressPublic()
        {
            ResetProgress();
        }

        /// <summary>
        /// Проверка, есть ли сохраненный прогресс
        /// </summary>
        public bool HasSaveData()
        {
            return PlayerPrefs.HasKey(SAVE_KEY);
        }
    }
}
