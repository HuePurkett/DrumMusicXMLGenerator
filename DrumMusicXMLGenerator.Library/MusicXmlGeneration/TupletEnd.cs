using System.Xml.Linq;
using DrumMusicXMLGenerator.Library.Interfaces;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Makes this note the last note of a tuplet.
    /// Does nothing unless there is a <see cref="Tuplet"/> on some previous note.
    /// </summary>
    public class TupletEnd:IRudiment
    {
        private readonly int number;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sub">Note to be modified.</param>
        /// <param name="num">Which tuplet is this?</param>
        public TupletEnd(IRhythm sub, int num = 1)
        {
            SubRhythm = sub;
            number = num;
        }
        
        /// <summary>
        /// Retrieves the musicXml that corresponds to this class and all that it encapsulates.
        /// </summary>
        /// <returns><see cref="XElement"/> containing the required musicXml.</returns>
        public XElement GetXml()
        {
            XElement dirnote = SubRhythm.GetXml();
            XElement note = dirnote.Element("note");
            if (note == null)
            {
                return dirnote;
            }
            XElement notation = note.Element("notations");
            if (notation == null)
            {
                notation=new XElement("notations");
                note.Add(notation);
            }
            notation.Add(XElement.Parse("<tuplet type=\"stop\" bracket=\"yes\" number=\"" + number + "\"/>"));
            return dirnote;
        }

        /// <summary>
        /// Does this note have the flam rudiment?
        /// </summary>
        /// <returns>bool answer</returns>
        public bool IsFlam()
        {
            return SubRhythm.IsFlam();
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
        /// Note being modified.
        /// </summary>
        public IRhythm SubRhythm { get; set; }
        
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
        /// <returns>true, because this is a tuplet end.</returns>
        public bool IsTupletEnd()
        {
            return true;
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
