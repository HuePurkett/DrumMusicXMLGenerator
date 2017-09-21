using System.Xml.Linq;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Represents a clef.
    /// </summary>
    public class Clef
    {
        private readonly string xml = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sign">Which sort of clef it is.</param>
        /// <param name="line">Where the clef should go on the staff.</param>
        public Clef(string sign="percussion", int line=2)
        {
            xml = "<clef><sign>" + sign + "</sign><line>" + line + "</line></clef>";
        }

        /// <summary>
        /// Gets the corresponding musicXml.
        /// </summary>
        /// <returns><see cref="XElement"/> containing the required musicXml.</returns>
        public XElement GetXml()
        {
            return XElement.Parse(xml);
        }
    }
}
