namespace TddAcademy
{
    public interface ISource
    {
        bool HasNextCharacter { get; }

        char GetNextCharacter();
    }
}