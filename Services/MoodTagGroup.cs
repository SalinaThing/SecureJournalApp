namespace SecureJournal.Data;

public static class MoodTagGroup
{
    public static readonly string[] EntryCategories =
    {
        "Family", "Work", "Personal Growth", "Reflection","Health", "Travel", "Fitness", "Studies", "Other"
    };

    public static readonly Dictionary<string, string[]> MoodGroups = new()
    {
        ["Positive"] = new[] { "Relaxed", "Grateful", "Confident", "Happy", "Excited", },
        ["Neutral"] = new[] { "Nostalgic", "Bored","Calm", "Thoughtful", "Curious" },
        ["Negative"] = new[] { "Angry", "Stressed","Sad", "Lonely", "Anxious" }
    };

    public static readonly string[] PrebuiltTags =
    {
        "Studies","Career","Work","Family","Cooking","Meditation","Yoga","Music","Shopping","Parenting","Projects","Planning",
        "Friends","Relationships","Health","Fitness","Personal Growth","Self-care","Spirituality","Birthday","Holiday",
        "Hobbies","Travel","Nature","Finance","Vacation","Celebration","Exercise", "Reading","Writing","Reflection"
    };
}
