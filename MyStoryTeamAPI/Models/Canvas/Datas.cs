namespace MyStoryTeamAPI.Models.Canvas
{
    public class Datas
    {
        public record Vector2Int(int X, int Y);
        public record Vector3Int(int X, int Y, int Z);

        public record Connection(Guid Self, Guid Target);

        public record ListData_(List<string> ListData);
        public record VideoData(string Url);
        public record ImageData(string Base64File);
        public record AudioData(string Base64File);
        public record TaskData(string Data, bool Checked);
        public record DateData(string Date, string Data);


    }
}
