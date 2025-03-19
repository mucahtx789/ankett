﻿namespace ankett.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
