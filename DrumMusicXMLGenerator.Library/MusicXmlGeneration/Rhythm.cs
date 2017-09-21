using System;
using System.Xml.Linq;
using DrumMusicXMLGenerator.Library.Interfaces;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Represents a note.
    /// </summary>
    public class Rhythm:IRhythm
    {
        private string Xml { get; set; }
        private readonly int value;
        
        /// <summary>
        /// Retrieves the musicXml that corresponds to this note.
        /// </summary>
        /// <returns><see cref="XElement"/> containing the required musicXml.</returns>
        public XElement GetXml()
        {
            return XElement.Parse(Xml);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="type">What note is it. Supported strings are: "whole", "half", "quarter", "eighth", "16th", and "32nd".</param>
        /// <param name="displayStep">Where should the note go on the staff. Defaults to correct placement for generic percussion.</param>
        /// <param name="displayOctave">Does something. Defaults to correct.</param>
        public Rhythm(string type, string displayStep="C", string displayOctave="5"){
            switch (type)
            {
                case "32nd":
                    value = 1;
                    break;
                case "16th":
                    value = 2;
                    break;
                case "eighth":
                    value = 4;
                    break;
                case "quarter":
                    value = 8;
                    break;
                case "half":
                    value = 16;
                    break;
                default:
                    //type = "32nd";
                    value = 1;
                    break;
            }
            Xml="<directedNote>"+
                    "<note>"+
                        "<unpitched>"+
                            "<display-step>"+displayStep+"</display-step>"+
                            "<display-octave>"+displayOctave+"</display-octave>"+
                        "</unpitched>"+
                        "<type>" + type +"</type>"+
                        "<stem>up</stem>"+
                        "<duration>1</duration>"+
                    "</note>"+
                "</directedNote>";
        }

        /// <summary>
        /// Does this note have the flam rudiment?
        /// </summary>
        /// <returns>false, because you have reached the core without finding one.</returns>
        public bool IsFlam()
        {
            return false;
        }
        
        /// <summary>
        /// Does this note have the flat flam rudiment?
        /// </summary>
        /// <returns>false, because you have reached the core without finding one.</returns>
        public bool IsFlatFlam()
        {
            return false;
        }
        
        /// <summary>
        /// How many 32nd notes is this note equivalent to?
        /// </summary>
        /// <returns>int answer</returns>
        public int GetVal()
        {
            return value;
        }
        
        /// <summary>
        /// Does this note start a tuplet?
        /// </summary>
        /// <returns>false, because you have reached the core without finding one.</returns>
        public bool IsTupletStart()
        {
            return false;
        }
        
        /// <summary>
        /// Does this note end a tuplet?
        /// </summary>
        /// <returns>false, because you have reached the core without finding one.</returns>
        public bool IsTupletEnd()
        {
            return false;
        }
        
        /// <summary>
        /// How many 32nd notes is the collection of notes that this note starts equivalent to?
        /// The same as GetVal().
        /// </summary>
        /// <returns>int answer</returns>
        public int GetLength()
        {
            return value;
        }
    }
}
