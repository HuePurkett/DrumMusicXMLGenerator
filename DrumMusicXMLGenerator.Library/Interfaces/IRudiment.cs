namespace DrumMusicXMLGenerator.Library.Interfaces
{
    /// <summary>
    /// Interface that represents a rudiment. 
    /// Decorator of <see cref="IRhythm"/>
    /// </summary>
    public interface IRudiment:IRhythm
    {
        IRhythm SubRhythm { get; set; }
    }
}
