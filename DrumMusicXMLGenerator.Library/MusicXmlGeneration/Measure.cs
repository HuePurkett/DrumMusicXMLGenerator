using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DrumMusicXMLGenerator.Library.Interfaces;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Represents a measure for the purpose of creating the correct musicXml.
    /// </summary>
    public class Measure
    {
        private readonly List<Beat> beats = new List<Beat>();
        /// <summary>
        /// The number of this measure.
        /// </summary>
        public int MeasureNumber { get; }
        private readonly int beatVal;
        private readonly int beatNum;
        /// <summary>
        /// Should this measure track whether or not it is full.
        /// </summary>
        public bool Oflow=true;
        /// <summary>
        /// Which <see cref="MusicXmlGenerator"/> made this.
        /// Used to invoke the BeatCreated event.
        /// </summary>
        public MusicXmlGenerator Progenitor=null;

        private XElement Meas { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="measureNumber">Which measure this is.</param>
        /// <param name="ts">The time signature you want used. Defaults to 4/4 time.</param>
        /// <param name="c">The clef you want used. Defaults to percussion.</param>
        public Measure(int measureNumber, TimeSignature ts=null, Clef c=null)
        {
            beatVal = ts?.BeatVal ?? 8;//assume 4/4 time
            beatNum = ts?.BeatNum ?? 4;
            MeasureNumber = measureNumber;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<!--============== Part: P1, Measure: {MeasureNumber} ==============-->");
            sb.AppendLine($"<measure number=\"{MeasureNumber}\">");
            if (MeasureNumber == 1)//attributes are only needed in the first measure
            {
                sb.AppendLine("<attributes>");
                sb.AppendLine( "<divisions>8</divisions>");
                sb.AppendLine( "<key>");
                sb.AppendLine(  "<fifths>0</fifths>");
                sb.AppendLine( "</key>");
                if (ts == null)
                {
                    sb.AppendLine("<time>");
                    sb.AppendLine("<beats>4</beats>");
                    sb.AppendLine("<beat-type>4</beat-type>");
                    sb.AppendLine("</time>");
                }
                else
                {
                    sb.Append(ts.GetXml());
                }
                if (c == null)
                {
                    sb.AppendLine("<clef>");
                    sb.AppendLine("<sign>percussion</sign>");
                    sb.AppendLine("<line>2</line>");
                    sb.AppendLine("</clef>");
                }
                else
                {
                    sb.Append(c.GetXml());
                }
                sb.AppendLine("</attributes>");
            }
            sb.AppendLine("</measure>");

            Meas = XElement.Parse(sb.ToString());
        }

        private decimal SumBeats()
        {
            return beats.Sum(b => b.NumberOfBeats());
        }

        /// <summary>
        /// Add a note to this measure.
        /// Can fail if it has a number of beats equal to the numberator of the time signature, all of which are full and if Oflow is true.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public bool Add(IRhythm note)
        {
            if (beats.Count > 0 && !beats.Last().IsFull() && beats.Last().Add(note))
            {
                return true;
            } //a new beat is needed
            if (Oflow && SumBeats() >= beatNum) 
            {
                return false; //fail to add on full--a new measure is needed
            }
            else
            {
                Beat newBeat=new Beat(beatVal);
                beats.Add(newBeat);
                newBeat.Add(note);//Guaranteed empty. Cannot fail.
                Progenitor.InvokeBeatCreated(newBeat, this);
            }
            return true;
        }
        
        /// <summary>
        /// Gets the corresponding musicXml.
        /// </summary>
        /// <returns><see cref="XElement"/> containing the required musicXml.</returns>
        public XElement GetXml(bool beam=true)
        {
            foreach (Beat n in beats)
            {
                Meas.Add(n.GetXml(beam).Elements());
            }
            return Meas;
        }
    }
}
