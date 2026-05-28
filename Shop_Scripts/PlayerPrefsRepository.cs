using UnityEngine;

namespace CleanRefactor
{
    public class PlayerPrefsRepository : IPlayerRepository
    {
        public PlayerState Load()
        {
            return new PlayerState
            {
                Coins = PlayerPrefs.GetInt("Coins", 500),
                Level = PlayerPrefs.GetInt("PlayerLevel", 1),
                BombUses = PlayerPrefs.GetInt("BombUses", 0),
                ShieldUses = PlayerPrefs.GetInt("ShieldUses", 0),
                HasDoubleCoins = PlayerPrefs.GetInt("HasDoubleCoins", 0) == 1
            };
        }

        public void Save(PlayerState state)
        {
            PlayerPrefs.SetInt("Coins", state.Coins);
            PlayerPrefs.SetInt("PlayerLevel", state.Level);
            PlayerPrefs.SetInt("BombUses", state.BombUses);
            PlayerPrefs.SetInt("ShieldUses", state.ShieldUses);
            PlayerPrefs.SetInt("HasDoubleCoins", state.HasDoubleCoins ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}