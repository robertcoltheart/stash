using MessagePack;

namespace Stash.Tests.Performance
{
    [MessagePackObject]
    public class ManyTypesModel
    {
        [Key(0)]
        public string StringValue { get; set; }

        [Key(1)]
        public int IntValue { get; set; }

        [Key(2)]
        public short ShortValue { get; set; }

        [Key(3)]
        public long LongValue { get; set; }
    }
}
