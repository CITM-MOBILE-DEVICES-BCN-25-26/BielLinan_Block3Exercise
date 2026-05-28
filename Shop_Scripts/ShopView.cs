using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRefactor
{
    public class ShopView : MonoBehaviour
    {
        [Header("UI Controls")]
        [SerializeField] TextMeshProUGUI coinsText;
        [SerializeField] TextMeshProUGUI levelText;
        [SerializeField] TextMeshProUGUI feedbackText;
        [SerializeField] Button bombButton;
        [SerializeField] Button shieldButton;
        [SerializeField] Button doubleCoinsButton;
        [SerializeField] Button levelUpButton;
        [SerializeField] Button addCoinsButton; 
        [SerializeField] Button resetProfileButton;  

        [Header("Audio")]
        [SerializeField]  AudioSource audioSource;

        private BuyItemUseCase _useCase;

        public void Initialize(BuyItemUseCase useCase)
        {
            _useCase = useCase;

            bombButton.onClick.AddListener(() => OnPurchaseClicked(ShopItemType.Bomb));
            shieldButton.onClick.AddListener(() => OnPurchaseClicked(ShopItemType.Shield));
            doubleCoinsButton.onClick.AddListener(() => OnPurchaseClicked(ShopItemType.DoubleCoins));
            levelUpButton.onClick.AddListener(OnLevelUpClicked);
            addCoinsButton.onClick.AddListener(OnAddCoinsClicked);
            resetProfileButton.onClick.AddListener(OnResetProfileClicked);

            RefreshUI();
        }

        private void OnLevelUpClicked()
        {
            _useCase.LevelUpPlayer();
            feedbackText.text = "Leveled up!";
            RefreshUI();
        }

        private void OnAddCoinsClicked()
        {
            _useCase.AddCoins(100);
            feedbackText.text = "+100 Coins added!";
            RefreshUI();
        }

        private void OnResetProfileClicked()
        {
            _useCase.ResetProfile();
            feedbackText.text = "Profile completely reset!";
            RefreshUI();
        }

        private void OnPurchaseClicked(ShopItemType itemType)
        {
            PurchaseStatus result = _useCase.Execute(itemType);
            ProcessFeedback(result, itemType);
            RefreshUI();
        }

        private void RefreshUI()
        {
            ShopStatusDto status = _useCase.GetCurrentStatus();

            coinsText.text = $"Coins: {status.CurrentCoins}";
            levelText.text = $"Level: {status.CurrentLevel}";

            bombButton.interactable = status.CanBuyBomb;
            shieldButton.interactable = status.CanBuyShield;
            doubleCoinsButton.interactable = status.CanBuyDoubleCoins;
        }

        private void ProcessFeedback(PurchaseStatus status, ShopItemType item)
        {
            if (status == PurchaseStatus.Purchased)
            {
                feedbackText.text = $"{item} purchased!";
                if (audioSource != null) audioSource.Play();
                return;
            }

            feedbackText.text = status switch
            {
                PurchaseStatus.NotEnoughCoins => $"Not enough coins for {item}",
                PurchaseStatus.MaxUsesReached => $"{item} already at max uses",
                PurchaseStatus.RequiredLevelNotReached => $"Need level 5 for {item}",
                PurchaseStatus.AlreadyOwned => $"{item} already purchased",
                _ => "Unknown error occurred."
            };
        }
    }
}