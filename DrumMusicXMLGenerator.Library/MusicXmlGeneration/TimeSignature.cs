using System.Xml.Linq;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Represents a time signature.
    /// </summary>
    public class TimeSignature
    {
        private readonly string xml;
        /// <summary>
        /// How many beats to a measure?
        /// </summary>
        public int BeatNum { get; }
        /// <summary>
        /// What note value gets the beat?
        /// </summary>
        public int BeatVal { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="beats">Numerator. Defaults to 4.</param>
        /// <param name="beatType">Denominator. Defaults to 4.</param>
        public TimeSignature(int beats=4, int beatType=4)
        {
            BeatNum = beats;
            switch (beatType)
            {
                default:
                    BeatVal = 32;
                    break;
                case 2:
                    BeatVal = 16;
                    break;
                case 4:
                    BeatVal = 8;
                    break;
                case 8:
                    BeatVal = 4;
                    break;
                case 16:
                    BeatVal = 2;
                    break;
                case 32:
                    BeatVal = 1;
                    break;
            }
            xml = "<time><beats>"+beats+"</beats><beat-type>"+beatType+"</beat-type></time>";
        }

        /// <summary>
        /// Retrieves the musicXml for the time signature.
        /// </summary>
        /// <returns><see cref="XElement"/> containing the required musicXml.</returns>
        public XElement GetXml()
        {
            return XElement.Parse(xml);
        }
    }
}
