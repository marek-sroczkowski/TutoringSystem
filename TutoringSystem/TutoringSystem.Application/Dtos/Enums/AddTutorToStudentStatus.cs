namespace TutoringSystem.Application.Dtos.Enums
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