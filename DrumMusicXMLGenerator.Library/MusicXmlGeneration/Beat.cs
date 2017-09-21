using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DrumMusicXMLGenerator.Library.Interfaces;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Represents a beat for the purpose of generating the correct musicXml.
    /// </summary>
    public class Beat
    {
        private readonly List<IRhythm> notes = new List<IRhythm>();
        private readonly int len;
        private int place = 0;
        private bool tupleting = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="length">Maximum total note value this beat should contain. Deternined by the denominator of the time signature.</param>
        public Beat(int length)
        {
            len = length;
        }

        /// <summary>
        /// Adds a note to this beat.
        /// Can fail if note puts beat over full.
        /// Will always take all notes of a tuplet unless the beat is full before it starts.
        /// </summary>
        /// <param name="note">Note to be added.</param>
        /// <returns>Bool successful or not.</returns>
        public bool Add(IRhythm note)
        {
            if (IsFull()&&!tupleting)
            {
                return false;
            }
            if (note.IsTupletStart())
            {
                tupleting = true;
                place +=note.GetLength();
            }
            if (tupleting)
            {
                notes.Add(note);
                if (note.IsTupletEnd())
                {
                    tupleting = false;
                }
                return true;
            }
            int bak = place;
            place += note.GetVal();
            if (notes.Count == 0)//always accept on empty
            {
                notes.Add(note);
                return true;
            }
            if (place/len>bak/len&&place%len!=0)//accept note causing rollover iff it makes this Beat's length evenly divisible by the length of a beat
            {
                place = bak;
                return false;
            }
            notes.Add(note);
            return true;
        }

        /// <summary>
        /// How many beats does this beat contain?
        /// 0 if empty, >0 otherwise.
        /// </summary>
        /// <returns>decimal answer.</returns>
        public decimal NumberOfBeats()
        {
            return place / (decimal)len;
        }

        /// <summary>
        /// Is this beat full?
        /// </summary>
        /// <returns>bool answer.</returns>
        public bool IsFull()
        {
            return place%len==0&&place>=len&&!tupleting;
        }

        /// <summary>
        /// Adds beams to notes as needed.
        /// </summary>
        private void Beaming()
        {
            ClearBeams();//just in case
            bool midBeam = false;
            for (int i = 0; i < notes.Count; i++)
            {
                int noteVal = notes[i].GetVal();
                if (noteVal == 32 || noteVal == 16 || noteVal == 8)//ignore note if whole, half, or quarter
                {
                    continue;
                }
                if(midBeam&&( i == notes.Count - 1 || notes[i + 1].IsTupletStart()||notes[i].IsTupletEnd()||notes[i+1].GetVal()>=8))//end beam
                {
                    notes[i]=new Beam(notes[i], "end");
                    midBeam = false;
                    continue;
                }
                if (midBeam)//continue beam
                {
                    notes[i]=new Beam(notes[i], "continue");
                    continue;
                }
                if (!( i == notes.Count - 1 || notes[i + 1].IsTupletStart()||notes[i].IsTupletEnd()||notes[i+1].GetVal()>=8))//start beam
                {
                    notes[i]=new Beam(notes[i], "begin");
                    midBeam = true;
                }
            }
        }

        /// <summary>
        /// Clear all beams from contained notes.
        /// </summary>
        private void ClearBeams()
        {
            for(int i=0; i<notes.Count; i++)
            {
                if (notes[i].GetType() == typeof(Beam))
                {
                    notes[i] = ((Beam)notes[i]).SubRhythm;
                }
            }
        }

        /// <summary>
        /// Retrieves the musicXml corresponding to all the contained notes.
        /// </summary>
        /// <param name="pleaseBeam">Should beams be added to the notes?</param>
        /// <returns></returns>
        public XElement GetXml(bool pleaseBeam=true)
        {
            if (pleaseBeam)
            {
                Beaming();
            }
            else
            {
                ClearBeams();
            }
            XElement beat=new XElement("beat");
            foreach (IRhythm n in notes)
            {
                if (n.IsFlam())
                {
                    XElement flam = new Rhythm("eighth").GetXml();
                    flam.Element("note")?.Add(XElement.Parse("<grace slash=\"yes\"/>"));
                    beat.Add(flam.Elements());
                }
                beat.Add(n.GetXml().Elements());
                if (n.IsFlatFlam())
                {
                    XElement flam = n.GetXml();
                    flam.Element("note")?.Add(XElement.Parse("<chord/>"));
                    beat.Add(flam.Elements());
                }
            }
            return beat;
        }
    }
}
