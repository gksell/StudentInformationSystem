using AutoMapper;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Core.Enums;
using StudentInformationSystem.Core.Results;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Interfaces.Repository.NoteRepository;

namespace StudentInformationSystem.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;
        private readonly ITeacherService _teacherService;

        public NoteService(INoteRepository noteRepository, IMapper mapper, IStudentService studentService, ICourseService courseService, ITeacherService teacherService)
        {
            _noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
            _teacherService = teacherService ?? throw new ArgumentNullException(nameof(teacherService));
        }

        public async Task<IDataResult<NoteDto>> AddNoteAsync(NoteRequestModel noteRequestModel)
        {
            if (noteRequestModel == null)
                throw new ArgumentNullException(nameof(noteRequestModel));

            var noteDto = _mapper.Map<NoteDto>(noteRequestModel);

            if (await _studentService.StudentExists(noteDto.StudentId) && await _courseService.CourseExists(noteDto.CourseId))
            {
                if (await IsStudentAlreadyGradedAsync(noteDto.StudentId, noteDto.CourseId))
                {
                    return new DataResult<NoteDto>(ResultStatus.Error, "Öğrencinin aynı kursta başka not kaydı bulunmaktadır.", null);
                }

                var noteEntity = _mapper.Map<Note>(noteDto);
                await _noteRepository.AddAsync(noteEntity);
                return new DataResult<NoteDto>(ResultStatus.Success, noteDto);
            }
            else
            {
                return new DataResult<NoteDto>(ResultStatus.Error, "Öğrenci veya kurs bilgisi hatalı.", null);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async Task<IDataResult<IEnumerable<StudentNoteDto>>> GetNotesByStudentIdAsync(int studentId)
        {
            var notes = await _noteRepository.GetAllFilterAsync(x=>x.StudentId == studentId);

            if (notes != null && notes.Any())
            {
                var studentNoteDtos = new List<StudentNoteDto>();

                foreach (var note in notes)
                {
                    var courseDto = await _courseService.GetCourseByIdAsync(note.CourseId);

                    if (courseDto != null)
                    {
                        var teacherDto = await _teacherService.GetTeacherByIdAsync(courseDto.TeacherId);
                        var studentNoteDto = new StudentNoteDto
                        {
                            CourseName = courseDto.CourseName,
                            Grade = note.Grade,
                            TeacherName = teacherDto.FirstName + " " + teacherDto.LastName,
                        };

                        studentNoteDtos.Add(studentNoteDto);
                    }
                }

                return new DataResult<IEnumerable<StudentNoteDto>>(ResultStatus.Success, studentNoteDtos);
            }
            else
            {
                return new DataResult<IEnumerable<StudentNoteDto>>(ResultStatus.Error, "Öğrencinin aldığı not bulunamadı.", null);
            }
        }
        public async Task<bool> IsStudentAlreadyGradedAsync(int studentId, int courseId)
        {
            var existingNote = await _noteRepository.GetFilterAsync(note => 
                                                                    note.StudentId == studentId && 
                                                                    note.CourseId == courseId
                                                                    );

            return existingNote != null;
        }

    }
}
