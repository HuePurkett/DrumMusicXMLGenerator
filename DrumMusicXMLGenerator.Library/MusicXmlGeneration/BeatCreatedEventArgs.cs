using System;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Event arguments for the beat created event of <see cref="MusicXmlGenerator"/>
    /// </summary>
    public class BeatCreatedEventArgs:EventArgs
    {
        /// <summary>
        /// The beat that was created.
        /// </summary>
        public Beat Bea;
        /// <summary>
        /// The measure in which it was created.
        /// </summary>
        public Measure Meas;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="b">The beat that was created.</param>
        /// <param name="m">The measure inside of which it was created.</param>
        public BeatCreatedEventArgs(Beat b, Measure m)

        {
            Bea = b;
            Meas = m;
        }
    }
}
