namespace ToDo.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Label { get; set; } = string.Empty;
        public bool IsDone { get; set; } = false;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public static Todo Create(int id, string label) 
        {
            return new Todo 
            { 
                Id = id, 
                Label = label,
                IsDone = false,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };
        }
    }
}
