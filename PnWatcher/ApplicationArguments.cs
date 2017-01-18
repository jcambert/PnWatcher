namespace PnWatcher
{
    public class ApplicationArguments
    {
        public string Path { get;  set; }
        public bool Inspect { get; set; }
        public bool AutoRun { get; set; }
        public bool AllowMoveFile { get; set; }
        public int MaxStatusLinesCount { get; set; }
        public string Extension { get; internal set; }
    }
}