using System.Text.Json;
using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class ReleaseStatusJsonConverter : JsonConverter<ReleaseStatus>
{
  public override ReleaseStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    var value = reader.GetString();

    return value switch
    {
      "discontinued" => ReleaseStatus.Discontinued,
      "announced" => ReleaseStatus.Announced,
      "finished" => ReleaseStatus.Finished,
      "ongoing" => ReleaseStatus.Ongoing,
      "paused" => ReleaseStatus.Paused,

      _ => throw new JsonException($"Unknown ReleaseStatus value: {value}")
    };
  }

  public override void Write(Utf8JsonWriter writer, ReleaseStatus value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.ToString());
  }
}
