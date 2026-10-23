namespace SchoolHub.Models.ViewModels.Rating
{
    public class ListRatingViewModel
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string TeacherName { get; set; } = null!;
        public int EnrolledYear { get; set; }
        public decimal Exam1 { get; set; }
        public decimal Exam2 { get; set; }
        public decimal Exam3 { get; set; }
    }
}
