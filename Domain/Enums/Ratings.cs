using System.ComponentModel;

namespace Models.Enums
{
    public enum Ratings
    {
        [Description("\U0001F622")]
        Very_Sad = 1,
        [Description("\u2639\uFE0F")]
        Sad = 2,
        [Description("\U0001F610")]
        Neutral = 3,
        [Description("\U0001F642")]
        Happy = 4,
        [Description("\U0001F603")]
        Very_Happy = 5
    }
}
