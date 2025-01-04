using System.Text.Json;
using System.Text.Json.Serialization;
using Jellyfin.Plugin.Hikka.Types.Abstract;

namespace Jellyfin.Plugin.Hikka.Types.Enums;

public class ReleaseStatus
{
  private ReleaseStatus(string value) { Value = value; }

  public string Value { get; private set; }

  public static ReleaseStatus Discontinued { get { return new ReleaseStatus("discontinued"); } }
  public static ReleaseStatus Announced { get { return new ReleaseStatus("announced"); } }
  public static ReleaseStatus Finished { get { return new ReleaseStatus("finished"); } }
  public static ReleaseStatus Ongoing { get { return new ReleaseStatus("ongoing"); } }
  public static ReleaseStatus Paused { get { return new ReleaseStatus("paused"); } }

  public override string ToString()
  {
    return Value;
  }
}

public class ReleaseStatusJsonCoverter : JsonConverter<ReleaseStatus>
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

public class ReleaseStatusesJsonCoverter : JsonArrayConverter<ReleaseStatus>
{
  public ReleaseStatusesJsonCoverter() : base(new ReleaseStatusJsonCoverter()) { }
}
