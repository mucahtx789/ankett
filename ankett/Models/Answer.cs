namespace ankett.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int OptionId { get; set; }
        public User Employee { get; set; }
        public Option Option { get; set; }
    }
}
