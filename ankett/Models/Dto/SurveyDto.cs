namespace ankett.Models.Dto
{
    
        public class SurveyCreateRequest
        {
            public string Title { get; set; }
            public List<QuestionCreateRequest> Questions { get; set; }
        }

        public class QuestionCreateRequest
        {
            public string Text { get; set; }
            public List<OptionCreateRequest> Options { get; set; }
        }

        public class OptionCreateRequest
        {
            public string Text { get; set; }
        }
    
}
