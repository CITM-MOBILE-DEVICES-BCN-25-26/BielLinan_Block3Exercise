namespace CleanRefactor.Tests
{
    public class MockPlayerRepository : IPlayerRepository
    {
        public PlayerState State { get; set; }
        public bool SaveWasCalled { get; private set; }

        public PlayerState Load() => State;

        public void Save(PlayerState state)
        {
            State = state;
            SaveWasCalled = true;
        }

        public void ResetSaveFlag() => SaveWasCalled = false;
    }
}