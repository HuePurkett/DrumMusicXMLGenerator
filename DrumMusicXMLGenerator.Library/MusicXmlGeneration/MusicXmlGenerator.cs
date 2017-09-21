using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DrumMusicXMLGenerator.Library.Interfaces;

namespace DrumMusicXMLGenerator.Library.Models.MusicXmlGeneration
{
    /// <summary>
    /// Represents the sheet music for the purposes of generating the correct musicXml.
    /// </summary>
    public class MusicXmlGenerator
    {
        private readonly List<Measure> measures = new List<Measure>();
        private readonly XElement root;
        private int currentMeasure = 1;
        private TimeSignature timeSig = null;
        private readonly bool oflow;

        /// <summary>
        /// A <see cref="Measure"/> was created.
        /// </summary>
        public event EventHandler MeasureCreated;
        /// <summary>
        /// A <see cref="Beat"/> was created.
        /// </summary>
        public event EventHandler BeatCreated;

        /// <summary>
        /// String containing the basic musicXml without any measures or notes.
        /// </summary>
        public string BlankXml { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="blankXml">String containing all musicXml elements outside of the measures.</param>
        /// <param name="handleOverflow">Do you want Add() to automatically move to new measures as they fill up?</param>
        public MusicXmlGenerator(string blankXml="<?xml version=\"1.0\" encoding=\'UTF-8\' standalone=\'no\' ?><!DOCTYPE score-partwise PUBLIC \"-//Recordare//DTD MusicXML 3.0 Partwise//EN\" \"http://www.musicxml.org/dtds/partwise.dtd\"><score-partwise version=\"3.0\"><part-list><score-part id=\"P1\"><part-name>SnareDrum</part-name><part-name-display><display-text>Snare Drum</display-text></part-name-display></score-part></part-list><part id=\"P1\"></part>\r\n</score-partwise>",
            bool handleOverflow=true)
        {
            BlankXml = blankXml;
            root = XElement.Parse(BlankXml);
            oflow = handleOverflow;
        }

        public Measure CreateMeasure(TimeSignature ts=null, Clef c=null)
        {
            if (ts != null)
            {
                timeSig = ts;
            }
            Measure newMeasure = new Measure(currentMeasure++, timeSig, c);
            measures.Add(newMeasure);
            newMeasure.Progenitor = this;
            newMeasure.Oflow = oflow;
            MeasureCreated?.Invoke(this, new MeasureCreatedEventArgs(newMeasure, currentMeasure-1));
            return newMeasure;
        }

        /// <summary>
        /// Add a note to the sheet music.
        /// </summary>
        /// <param name="note">Note you want added.</param>
        public void Add(IRhythm note)
        {
            if (measures.Count == 0 || !measures.Last().Add(note))
            {
                CreateMeasure().Add(note);//Guaranteed empty--cannot fail.
            }
        }

        /// <summary>
        /// Invokes the BeatCreated event.
        /// DO NOT USE.
        /// </summary>
        /// <param name="b">The beat that was created.</param>
        /// <param name="m">The measure that created it.</param>
        public void InvokeBeatCreated(Beat b, Measure m)
        {
            BeatCreated?.Invoke(this, new BeatCreatedEventArgs(b, m));
        }

        /// <summary>
        /// Retrieves the musicXml that corresponds to the sheet music.
        /// </summary>
        /// <returns><see cref="XElement"/> containing the required musicXml.</returns>
        public XDocument GetSheetMusic(bool beam=true)
        {
            XElement partNode = root.Element("part");

            if (partNode == null)
            {
                throw new ArgumentNullException(nameof(partNode));
            }

            foreach (Measure m in measures)
            {
                partNode.Add(m.GetXml(beam));
            }

            return new XDocument(root);
        }
    }
}