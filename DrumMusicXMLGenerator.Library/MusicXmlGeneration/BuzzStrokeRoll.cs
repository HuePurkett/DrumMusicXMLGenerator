using System.Linq;
using System.Xml.Linq;
using DrumMusicXMLGenerator.Library.Interfaces;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Adds an buzz stroke roll rudiment to the note.
    /// </summary>
    public class BuzzStrokeRoll:IRudiment
    {
        
        /// <summary>
        /// Retrieves the musicXml that corresponds to this class and all that it encapsulates.
        /// </summary>
        /// <returns><see cref="XElement"/> containing the required musicXml.</returns>
        public XElement GetXml()
        {
            XElement dirnote=SubRhythm.GetXml();
            XElement note = dirnote.Element("note");
            XElement notations = note?.Element("notations");
            if (notations == null)
            {
                note?.Add(XElement.Parse("<notations><ornaments><tremolo type=\"single\">3</tremolo></ornaments></notations>"));
            }
            else
            {
                XElement orns=notations.Element("ornaments");
                if (orns == null)
                {
                    notations.Add(XElement.Parse("<ornaments><tremolo type=\"single\">3</tremolo></ornaments>"));
                }
                else
                {
                    XElement trem = orns.Element("tremolo");
                    if (trem == null)
                    {
                        orns.Add(XElement.Parse("<tremolo type=\"single\">3</tremolo>"));
                    }
                    else
                    {
                        if ((int)trem.Elements().First()!=3)
                        {
                            trem.ReplaceAll(3);
                        }
                    }
                }
            }
            return dirnote;
        }
        
        /// <summary>
        /// Note being modified.
        /// </summary>
        public IRhythm SubRhythm { get; set; }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sub">Note to be modified.</param>
        public BuzzStrokeRoll(IRhythm sub)
        {
            SubRhythm = sub;
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
