namespace MyStoryTeamAPI.Models.Canvas
{
    namespace MyStoryTeamAPI.Models.Canvas.Elements
    {
        public sealed class TextElement : CanvasElement
        {
            public string Data { get; set; } = string.Empty;
        }

        public sealed class ListElement : CanvasElement
        {
            public Datas.ListData_ Data { get; set; } = new([]);
        }

        public sealed class ImageElement : CanvasElement
        {
            public Datas.ImageData Data { get; set; } = new("");
        }

        public sealed class AudioElement : CanvasElement
        {
            public Datas.AudioData Data { get; set; } = new("");
        }

        public sealed class TaskElement : CanvasElement
        {
            public Datas.TaskData Data { get; set; } = new("", false);
        }

        public sealed class DateElement : CanvasElement
        {
            public Datas.DateData Data { get; set; } = new("", "");
        }

        public sealed class VideoElement : CanvasElement
        {
            public Datas.VideoData Data { get; set; } = new("");
        }
    }
}
