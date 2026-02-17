using MyStoryTeamAPI.Models.Canvas.MyStoryTeamAPI.Models.Canvas.Elements;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MyStoryTeamAPI.Models.Canvas
{
    public abstract class CanvasElement
    {
        public Guid Id { get; set; }
        public Datas.Vector3Int Position { get; set; } = default!;
        public Datas.Vector2Int Size { get; set; } = default!;
        public List<Datas.ConnectionDB> Connections { get; set; } = new();
    }

    public sealed class CanvasElementConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => typeof(CanvasElement).IsAssignableFrom(objectType);


        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object? existingValue,
            JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);

            var type = jo["type"]?.Value<string>()
                ?? throw new JsonSerializationException("Canvas element missing 'type'");

            CanvasElement element = type switch
            {
                "Text" => new TextElement(),
                "List" => new ListElement(),
                "Image" => new ImageElement(),
                "Audio" => new AudioElement(),
                "Task" => new TaskElement(),
                "Date" => new DateElement(),
                "Video" => new VideoElement(),
                _ => throw new JsonSerializationException($"Unknown canvas element type '{type}'")
            };

            serializer.Populate(jo.CreateReader(), element);
            return element;
        }

        public override void WriteJson(
         JsonWriter writer,
         object? value,
         JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var cleanSerializer = new JsonSerializer
            {
                ContractResolver = serializer.ContractResolver,
                NullValueHandling = serializer.NullValueHandling
            };

            foreach (var conv in serializer.Converters)
            {
                if (conv is not CanvasElementConverter)
                    cleanSerializer.Converters.Add(conv);
            }

            var jo = JObject.FromObject(value, cleanSerializer);

            jo["type"] = value switch
            {
                TextElement => "Text",
                ListElement => "List",
                ImageElement => "Image",
                AudioElement => "Audio",
                TaskElement => "Task",
                DateElement => "Date",
                VideoElement => "Video",
                _ => throw new JsonSerializationException("Unknown canvas element")
            };

            jo.WriteTo(writer);
        }

    }


}
