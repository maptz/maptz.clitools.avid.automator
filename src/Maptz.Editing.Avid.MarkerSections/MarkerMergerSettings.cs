namespace Maptz.Editing.Avid.MarkerSections
{

    public class MarkerMergerSettings
    {
        public double SectionDurationSeconds { get; set; } = 5.0;
        public SmpteFrameRate FrameRate { get; set; } = SmpteFrameRate.Smpte25;
    }
}