using Microsoft.EntityFrameworkCore;
using MyStoryTeamAPI.Db;
using MyStoryTeamAPI.Models.Db;
using MyStoryTeamAPI.Models.Requests;
using MyStoryTeamAPI.Models.Responses;
using OpenAI;
using OpenAI.Chat;

namespace MyStoryTeamAPI.Repository
{
    public class AiConversationsRepository : RepositoryBase
    {
        private const int MAX_MESSAGES_PER_CONVERSATION = 100;
        private const int MAX_PROMPT_LENGTH = 500;
        private const int MIN_SECONDS_BETWEEN_MESSAGES = 5;

        private readonly OpenAIClient _openAi;

        private const string SYSTEM_PROMPT =
            "You are a kind, motivating coach helping users overcome social anxiety. " +
            "Keep answers short and concrete (3–6 sentences). " +
            "Be encouraging. If the user struggles, suggest an easier version of the task.";

        public AiConversationsRepository(
            AppDbContext dbContext,
            OpenAIClient openAIClient,
            IHttpContextAccessor httpContextAccessor
        ) : base(dbContext, httpContextAccessor)
        {
            _openAi = openAIClient;
        }

        //CREATE conversation
        public int CreateAIConversation(CreateAIConversationRequest request)
        {
            var conversation = new DbAIConversation
            {
                ID_User = this.GetCurrentUser().ID_User,
                AI_Conversation_Name = request.Title ?? "New conversation",
                Created_At = DateTime.UtcNow
            };

            DbContext.AIConversations.Add(conversation);
            DbContext.SaveChanges();

            return conversation.ID_AI_Conversation;
        }

        //GET all conversations
        public List<GetAIConversationsResponse> GetAIConversations()
        {
            return DbContext.AIConversations
                .Where(c => c.ID_User == this.GetCurrentUser().ID_User)
                .OrderByDescending(c => c.Created_At)
                .Select(c => new GetAIConversationsResponse
                {
                    ID_AI_Conversation = c.ID_AI_Conversation,
                    Title = c.AI_Conversation_Name
                })
                .ToList();
        }

        //GET messages
        public List<GetAIMessagesResponse> GetAIConversationMessages(int conversationId)
        {
            return DbContext.AIMessages
                .Where(m => m.ID_AI_Conversation == conversationId)
                .OrderBy(m => m.Created_At)
                .Select(m => new GetAIMessagesResponse
                {
                    ID_AI_Message = m.ID_AI_Message,
                    Content = m.Content,
                    Type = m.Type
                })
                .ToList();
        }

        //SEND message to AI
        public int? SendMessageToAI(AddNewAIMessageRequest request)
        {
            var userId = this.GetCurrentUser().ID_User;
            var conversation = DbContext.AIConversations
                .FirstOrDefault(c =>
                    c.ID_AI_Conversation == request.ID_AI_Conversation &&
                    c.ID_User == userId);

            #region Validation
            if (conversation == null)
                return null;

            if (string.IsNullOrWhiteSpace(request.Content))
                return null;

            if (request.Content.Length > MAX_PROMPT_LENGTH)
                return null;

            int messageCount = DbContext.AIMessages
                .Count(m => m.ID_AI_Conversation == request.ID_AI_Conversation);

            if (messageCount >= MAX_MESSAGES_PER_CONVERSATION)
                return null;

            var lastMessageTime = DbContext.AIMessages
                .Where(m => m.ID_AI_Conversation == request.ID_AI_Conversation)
                .OrderByDescending(m => m.Created_At)
                .Select(m => m.Created_At)
                .FirstOrDefault();

            if (lastMessageTime != default &&
                (DateTime.UtcNow - lastMessageTime).TotalSeconds < MIN_SECONDS_BETWEEN_MESSAGES)
            {
                return null;
            }

            // Limit to 50 messages per day across all conversations
            var todayCount = DbContext.AIMessages
                .Count(m =>
                    m.Type == "request" &&
                    m.Created_At.Date == DateTime.UtcNow.Date &&
                    DbContext.AIConversations.Any(c =>
                        c.ID_AI_Conversation == m.ID_AI_Conversation &&
                        c.ID_User == this.GetCurrentUser().ID_User));
            if (todayCount >= 50)
                return null;
            //-------------------------------------------------------
            #endregion

            var history = DbContext.AIMessages
                .Where(m => m.ID_AI_Conversation == request.ID_AI_Conversation)
                .OrderBy(m => m.Created_At)
                .ToList();

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(SYSTEM_PROMPT)
            };

            foreach (var msg in history)
            {
                messages.Add(
                    msg.Type == "request"
                        ? new UserChatMessage(msg.Content)
                        : new AssistantChatMessage(msg.Content)
                );
            }

            messages.Add(new UserChatMessage(request.Content));

            var chatClient = _openAi.GetChatClient("gpt-4o-mini");
            var response = chatClient.CompleteChat(messages);

            string aiReply = response.Value.Content[0].Text;

            var requestMessage = new DbAIMessage
            {
                ID_AI_Conversation = request.ID_AI_Conversation,
                Content = request.Content,
                Type = "request",
                Created_At = DateTime.UtcNow
            };

            var responseMessage = new DbAIMessage
            {
                ID_AI_Conversation = request.ID_AI_Conversation,
                Content = aiReply,
                Type = "response",
                Created_At = DateTime.UtcNow
            };

            DbContext.AIMessages.AddRange(
                requestMessage,
                responseMessage
            );

            DbContext.SaveChanges();

            return requestMessage.ID_AI_Message;
        }

        //UPDATE title
        public bool UpdateAIConversationTitle(UpdateAIConversationTitleRequest request)
        {
            var conversation = DbContext.AIConversations
                .FirstOrDefault(c =>
                    c.ID_AI_Conversation == request.ID_AI_Conversation &&
                    c.ID_User == this.GetCurrentUser().ID_User);

            if (conversation == null)
                return false;
            if (request.Title == null || request.Title.Length > 50)
                return false;

            conversation.AI_Conversation_Name = request.Title;
            DbContext.SaveChanges();

            return true;
        }
    }
}