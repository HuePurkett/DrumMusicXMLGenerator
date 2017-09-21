using System.Xml.Linq;
using DrumMusicXMLGenerator.Library.Interfaces;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Adds a flam rudiment to the note.
    /// Note: This is meant to work with <see cref="Beat"/>. If you call GetXml(), you will see no change to the musicXml from this class.
    /// </summary>
    public class Flam:IRudiment
    {
        /// <summary>
        /// Retrieves the musicXml that corresponds to this class and all that it encapsulates.
        /// </summary>
        /// <returns><see cref="XElement"/> containing the required musicXml.</returns>
        public XElement GetXml()
        {
            return SubRhythm.GetXml();
        }

        /// <summary>
        /// Note being modified.
        /// </summary>
        public IRhythm SubRhythm { get; set; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sub">Note to be modified.</param>
        public Flam(IRhythm sub)
        {
            SubRhythm = sub;
        }

        /// <summary>
        /// Does this note have the flam rudiment?
        /// </summary>
        /// <returns>true, because this is a flam rudiment.</returns>
        public bool IsFlam()
        {
            return true;
        }
        
        /// <summary>
        /// Does this note have the flat flam rudiment?
        /// </summary>
        /// <returns>bool answer</returns>
        public bool IsFlatFlam()
        {
            return SubRhythm.IsFlatFlam();
        }
        
        /// <summary>
        /// How many 32nd notes is this note equivalent to?
        /// </summary>
        /// <returns>int answer</returns>
        public int GetVal()
        {
            return SubRhythm.GetVal();
        }
        
        /// <summary>
        /// Does this note start a tuplet?
        /// </summary>
        /// <returns>bool answer</returns>
        public bool IsTupletStart()
        {
            return SubRhythm.IsTupletStart();
        }
        
        /// <summary>
        /// Does this note end a tuplet?
        /// </summary>
        /// <returns>bool answer</returns>
        public bool IsTupletEnd()
        {
            return SubRhythm.IsTupletEnd();
        }
        
        /// <summary>
        /// How many 32nd notes is the collection of notes that this note starts equivalent to?
        /// The same as GetVal() unless this note contains a <see cref="Tuplet"/>.
        /// </summary>
        /// <returns>int answer</returns>
        public int GetLength()
        {
            return SubRhythm.GetLength();
        }
    }
}
