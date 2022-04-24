namespace TutoringSystem.Application.Models.Enums
{
    public enum AddTutorToStudentStatus
    {
        InternalError = -1,
        RequestCreated,
        IncorrectTutor,
        RequestWasAlreadyCreated,
        TutorWasAlreadyAdded
    }
}