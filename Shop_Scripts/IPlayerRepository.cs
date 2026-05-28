namespace CleanRefactor
{
    public interface IPlayerRepository
    {
        PlayerState Load();
        void Save(PlayerState state);
    }
}