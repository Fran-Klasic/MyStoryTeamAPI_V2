using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStoryTeamAPI.Models.Requests;
using MyStoryTeamAPI.Models.Responses;
using MyStoryTeamAPI.Repository;

namespace MyStoryTeamAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/auth/conversations")]
    public class ConversationsController : Controller
    {
        private readonly ConversationsRepository _conversationsRepository;

        public ConversationsController(ConversationsRepository conversationsRepository)
        {
            _conversationsRepository = conversationsRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<GetAllConversationsResponse>> GetAllConversations()
        {
            List<GetAllConversationsResponse>? result = _conversationsRepository.GetAllConversations();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<List<GetAllMessagesResponse>> GetAllMessages(int id)
        {
            List<GetAllMessagesResponse>? result = _conversationsRepository.GetAllMessages(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<int> AddNewUserToConversation(AddNewUserRequest addNewUserRequest, int id)
        {
            int? result = _conversationsRepository.AddNewUserToConversation(addNewUserRequest, id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> CreateConversations(
            CreateConversationRequest createConversationRequest)
        {
            int? result = 
                await _conversationsRepository.CreateConversationsAsync(createConversationRequest);

            if (result == null)
                return NotFound();

            return Ok(result.Value);
        }


        [HttpPost("{id}")]
        [Authorize]
        public ActionResult<int> CreateMessage(CreateMessageRequest createMessageRequest, int id)
        {
            int? result = _conversationsRepository.CreateMessage(createMessageRequest, id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
