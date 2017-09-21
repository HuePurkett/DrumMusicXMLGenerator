using System.Xml.Linq;
using DrumMusicXMLGenerator.Library.Interfaces;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Makes the note the start of a tuplet.
    /// Works with <see cref="TupletEnd"/> on some subsequent note.
    /// </summary>
    public class Tuplet:IRudiment
    {
        private int Actual { get; set; }
        private int Normal { get; set; }

        private readonly int number;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sub">Note to be modified.</param>
        /// <param name="act">How many notes does this tuplet contain?</param>
        /// <param name="norm">How many of those notes is the tuplet equivalent to, time-wise? Automatically filled in for an act of 3, 5, 6, 7, and 9.</param>
        /// <param name="num">Which tuplet is this? only needs to be filled in for multiple concurrent tuplets.</param>
        public Tuplet(IRhythm sub, int act, int norm=0, int num = 1)
        {
            SubRhythm = sub;
            number = num;
            Actual = act;
            if (norm != 0)
            {
                Normal = norm;
            }
            else
            {
                switch (act)
                {
                    case 3:
                        Normal =2;
                        break;
                    case 5:
                    case 6:
                    case 7:
                        Normal =4;
                        break;
                    case 9:
                        Normal =8;
                        break;
                    default:
                        Normal =norm;
                        break;
                }
            }
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

            XElement time = note.Element("time-modification");
            if (time == null)
            {
                time = XElement.Parse("<time-modification/>");
                note.Add(time);
            }
            time.ReplaceAll(XElement.Parse("<actual-notes>" + Actual + "</actual-notes>"));
            time.Add(XElement.Parse("<normal-notes>" +Normal+"</normal-notes>"));

            XElement notation = note.Element("notations");
            if (notation == null)
            {
                notation=new XElement("notations");
                note.Add(notation);
            }
            notation.Add(XElement.Parse("<tuplet type=\"start\" bracket=\"yes\" number=\"" + number + "\"/>"));
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
        /// <returns>true, because this is a tuplet.</returns>
        public bool IsTupletStart()
        {
            return true;
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
        /// </summary>
        /// <returns>int answer</returns>
        public int GetLength()
        {
            return Normal*SubRhythm.GetVal();
        }
    }
}
