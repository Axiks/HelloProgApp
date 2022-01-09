namespace KonatsuWebApplication.Entities
{
    public class Habit : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
    }
}
