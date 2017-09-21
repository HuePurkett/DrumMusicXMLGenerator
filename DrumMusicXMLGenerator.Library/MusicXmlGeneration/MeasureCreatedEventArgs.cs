using System;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Event arguments for the measure created event of <see cref="MusicXmlGenerator"/>
    /// </summary>
    public class MeasureCreatedEventArgs:EventArgs
    {
        /// <summary>
        /// The measure that was created.
        /// </summary>
        public Measure Meas { get; }
        /// <summary>
        /// Which measure it is.
        /// </summary>
        public int MeasureNumber { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="m">The measure that was created.</param>
        /// <param name="mn">Which measure it is.</param>
        public MeasureCreatedEventArgs(Measure m, int mn)
        {
            Meas = m;
            MeasureNumber = mn;
        }
    }
}
