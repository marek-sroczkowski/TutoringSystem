using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class RepeatedReservationService : IRepeatedReservationService
    {
        private readonly IRepeatedReservationRepository reservationRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ITutorRepository tutorRepository;
        private readonly ISubjectRepository subjectRepository;

        public RepeatedReservationService(IRepeatedReservationRepository reservationRepository,
            IStudentRepository studentRepository,
            ITutorRepository tutorRepository,
            ISubjectRepository subjectRepository)
        {
            this.reservationRepository = reservationRepository;
            this.studentRepository = studentRepository;
            this.tutorRepository = tutorRepository;
            this.subjectRepository = subjectRepository;
        }

        public async Task<IEnumerable<RepeatedReservationDto>> GetReservationsByStudent(long studentId)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));
            var resevations = reservationRepository.GetReservationsCollection(r => r.StudentId.Equals(studentId))
                .Select(reservation => new RepeatedReservationDto(reservation)).ToList();
            resevations.ForEach(reservation =>
            {
                var tutor = tutorRepository.GetTutorAsync(t => t.Id.Equals(reservation.TutorId)).Result;
                var subject = subjectRepository.GetSubjectAsync(s => s.Id.Equals(reservation.SubjectId)).Result;
                reservation.Tutor = $"{tutor.FirstName} {tutor.LastName}";
                reservation.Student = $"{student.FirstName} {student.LastName}";
                reservation.SubjectName = subject.Name;
            });

            return resevations;
        }

        public async Task<IEnumerable<RepeatedReservationDto>> GetReservationsByTutor(long tutorId)
        {
            var tutor = await tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId));
            var resevations = reservationRepository.GetReservationsCollection(r => r.TutorId.Equals(tutorId))
                .Select(reservation => new RepeatedReservationDto(reservation)).ToList();
            resevations.ForEach(reservation =>
            {
                var student = studentRepository.GetStudentAsync(s => s.Id.Equals(reservation.StudentId)).Result;
                var subject = subjectRepository.GetSubjectAsync(s => s.Id.Equals(reservation.SubjectId)).Result;
                reservation.Student = $"{student.FirstName} {student.LastName}";
                reservation.Tutor = $"{tutor.FirstName} {tutor.LastName}";
                reservation.SubjectName = subject.Name;
            });

            return resevations;
        }
    }
}