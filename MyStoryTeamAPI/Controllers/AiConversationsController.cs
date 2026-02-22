using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStoryTeamAPI.Repository;
using MyStoryTeamAPI.Models.Requests;
using MyStoryTeamAPI.Models.Responses;

namespace MyStoryTeamAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/auth/ai")]
    public class AiConversationsController : Controller
    {
        private readonly AiConversationsRepository _aiConversationsRepository;

        public AiConversationsController(AiConversationsRepository aiConversationsRepository)
        {
            _aiConversationsRepository = aiConversationsRepository;
        }

        [HttpPost]
        [Authorize]
        //Create new AI conversation and return the new conversation's id
        public ActionResult<int> GenerateAIConversation(CreateAIConversationRequest generateAIConversationRequest)
        {
            int? result = _aiConversationsRepository.CreateAIConversation(generateAIConversationRequest);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        //Get all AI conversations for the user
        public ActionResult<List<GetAIConversationsResponse>> GetAllAIConversations()
        {
            List<GetAIConversationsResponse>? result = _aiConversationsRepository.GetAIConversations();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        //Get all messages in a specific AI conversation
        public ActionResult<List<GetAIMessagesResponse>> GetAllAIMessages(int id)
        {
            List<GetAIMessagesResponse>? result = _aiConversationsRepository.GetAIConversationMessages(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("message")]
        [Authorize]
        //Send prompt to AI and get response, then save both prompt and response to database
        public ActionResult<int> AddNewAIMessage(AddNewAIMessageRequest addNewAIMessageRequest)
        {
            int? result = _aiConversationsRepository.SendMessageToAI(addNewAIMessageRequest);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        //Update the title of an AI conversation
        public ActionResult UpdateAIConversationTitle(UpdateAIConversationTitleRequest updateAIConversationTitleRequest, int id)
        {
            bool result = _aiConversationsRepository.UpdateAIConversationTitle(updateAIConversationTitleRequest);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}