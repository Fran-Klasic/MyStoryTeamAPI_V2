using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStoryTeamAPI.Models.Requests;
using MyStoryTeamAPI.Models.Responses;
using MyStoryTeamAPI.Repository;

namespace MyStoryTeamAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/auth/canvas")]
    public class CanvasController : ControllerBase
    {
        private readonly CanvasRepository _canvasRepository;

        public CanvasController(CanvasRepository canvasRepository)
        {
            _canvasRepository = canvasRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<CanvasesDetailsResponse>> GetAllCanvases()
        {
            //Returns list of canvases with canvas names and background images and id, but without canvas data
            List<CanvasesDetailsResponse> canvases = _canvasRepository.GetAllCanvases();
            return canvases;
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<CanvasDetailsResponse> GetCanvas(int id)
        {
            //Returns canvas data and canvas name
            CanvasDetailsResponse canvasDetailsResponse = _canvasRepository.GetCanvasDetails(id);
            return canvasDetailsResponse;
        }

        [HttpPost]
        [Authorize]
        public ActionResult<int> CreateCanvas(CreateCanvasRequest createCanvasRequest)
        {
            //Creates new canvas with default name and default background image, returns id of created canvas
            int id = _canvasRepository.CreateCanvas(createCanvasRequest);
            return this.Ok(id);
        }

        [HttpPut]
        [Authorize]
        public ActionResult<int> UpdateCanvas(UpdateCanvasRequest updateCanvasRequest)
        {
            //Updates canvas data, name and background image
            int id = _canvasRepository.UpdateCanvas(updateCanvasRequest);
            return this.Ok(id);
        }

        [HttpDelete]
        [Authorize]
        public ActionResult<int> DeleteCanvas(DeleteCanvasRequest deleteCanvasRequest)
        {
            //Deletes canvas
            int id = _canvasRepository.DeleteCanvas(deleteCanvasRequest.ID_Canvas);
            return this.Ok(id);
        }
    }
}
