namespace DexiDev.Game.Gameplay
{
    public class GameplayManager
    {
        private PlayerController _playerController;
        
        public PlayerController PlayerController => _playerController;

        public void SetPlayerController(PlayerController playerController)
        {
            _playerController = playerController;
        }
    }
}