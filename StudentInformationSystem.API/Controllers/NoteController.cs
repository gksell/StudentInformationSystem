using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Core.Enums;

namespace StudentInformationSystem.API.Controllers
{
    [Route("api/notes")]
    [ApiController] 
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
        }

        [HttpPost]
        [Authorize(Roles ="Öğretmen")]
        public async Task<IActionResult> AddNote([FromBody] NoteRequestModel noteRequestModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _noteService.AddNoteAsync(noteRequestModel);

                return result.ResultStatus switch
                {
                    ResultStatus.Success => Ok(result.Data),
                    ResultStatus.Error => BadRequest(new { Message = result.Message }),
                    _ => StatusCode(500, "Internal Server Error")
                };
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("get-notes-by-student")]
        [Authorize(Roles ="Öğrenci")]
        public async Task<IActionResult> GetNotesByStudentId(int studentId)
        {
            var result = await _noteService.GetNotesByStudentIdAsync(studentId);

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Data),
                ResultStatus.Error => BadRequest(new { Message = result.Message }),
                _ => StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Internal Server Error" }),
            };
        }
    }
}
