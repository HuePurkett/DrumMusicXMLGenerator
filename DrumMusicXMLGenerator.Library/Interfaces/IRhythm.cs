using System.Xml.Linq;

namespace SnareDrumNotator.Library.Interfaces
{
    /// <summary>
    /// Interface that represents a single note.
    /// </summary>
    public interface IRhythm
    {
        XElement GetXml();

        bool IsFlam();

        bool IsFlatFlam();

        bool IsTupletStart();

        bool IsTupletEnd();

        int GetVal();

        int GetLength();
    }
}
