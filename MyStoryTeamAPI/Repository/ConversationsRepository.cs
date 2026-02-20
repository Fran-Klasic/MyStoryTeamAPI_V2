using Azure.Core;
using Microsoft.EntityFrameworkCore;
using MyStoryTeamAPI.Db;
using MyStoryTeamAPI.Models.Db;
using MyStoryTeamAPI.Models.Requests;
using MyStoryTeamAPI.Models.Responses;

namespace MyStoryTeamAPI.Repository
{
    public class ConversationsRepository : RepositoryBase
    {
        public ConversationsRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public List<GetAllConversationsResponse>? GetAllConversations()
        {
            var currentUser = GetCurrentUser();

            return DbContext.ConversationParticipants
                .Where(cp => cp.ID_User == currentUser.ID_User)
                .Select(cp => new GetAllConversationsResponse
                {
                    ID_Conversation = cp.ID_Conversation,
                    ID_User = cp.ID_User,
                    Created_At = cp.Conversation!.Created_At
                })
                .OrderByDescending(c => c.Created_At)
                .ToList();
        }
        public List<GetAllMessagesResponse>? GetAllMessages(int conversationId)
        {
            var currentUser = GetCurrentUser();

            bool isParticipant = DbContext.ConversationParticipants.Any(cp =>
                cp.ID_Conversation == conversationId &&
                cp.ID_User == currentUser.ID_User);

            if (!isParticipant)
                return null;

            return DbContext.Messages
                .Where(m => m.ID_Conversation == conversationId)
                .OrderBy(m => m.Created_At)
                .Select(m => new GetAllMessagesResponse
                {
                    ID_Message = m.ID_Message,
                    ID_Conversation = m.ID_Conversation,
                    Message_Content = m.Content,
                    Created_At = m.Created_At,
                    ID_User_Sender = m.ID_User_Sender
                })
                .ToList();
        }
        public async Task<int?> CreateConversationsAsync(CreateConversationRequest request)
        {
            //Validate input
            if (request?.ID_Users == null || request.ID_Users.Count == 0)
                return null;

            //Remove duplicates & detect duplicates
            var uniqueUserIds = new HashSet<int>(request.ID_Users);
            if (uniqueUserIds.Count != request.ID_Users.Count)
                return null;

            //Validate users exist (single DB call)
            int existingUsersCount = await DbContext.Users
                .Where(u => uniqueUserIds.Contains(u.ID_User))
                .CountAsync();

            if (existingUsersCount != uniqueUserIds.Count)
                return null;

            //Transaction for consistency
            await using var transaction = await DbContext.Database.BeginTransactionAsync();

            try
            {
                //Create conversation
                var conversation = new DbConversation
                {
                    Created_At = DateTime.Now,
                    Conversation_Name = "New Conversation"
                };

                DbContext.Conversations.Add(conversation);
                //ID generated here
                await DbContext.SaveChangesAsync();

                //Create participants (no duplicates guaranteed)
                var participants = uniqueUserIds.Select(idUser =>
                    new DbConversationParticipant
                    {
                        ID_Conversation = conversation.ID_Conversation,
                        ID_User = idUser,
                        Joined_At = DateTime.Now
                    });

                await DbContext.ConversationParticipants.AddRangeAsync(participants);
                await DbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return conversation.ID_Conversation;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public int? CreateMessage(CreateMessageRequest request, int conversationId)
        {
            if (request == null ||
               request.ID_Sender == null ||
               string.IsNullOrWhiteSpace(request.Message))
                return null;

            var currentUser = GetCurrentUser();

            //Sender must be the current user
            if (request.ID_Sender != currentUser.ID_User)
                return null;

            //User must be participant
            bool isParticipant = DbContext.ConversationParticipants.Any(cp =>
                cp.ID_Conversation == conversationId &&
                cp.ID_User == currentUser.ID_User);

            if (!isParticipant)
                return null;

            var message = new DbMessage
            {
                ID_Conversation = conversationId,
                ID_User_Sender = currentUser.ID_User,
                Content = request.Message,
                Created_At = DateTime.UtcNow
            };

            DbContext.Messages.Add(message);
            DbContext.SaveChanges();

            return message.ID_Message;
        }
        public int? AddNewUserToConversation(AddNewUserRequest request, int conversationId)
        {
            if (request?.ID_User == null || request.ID_Conversation == null)
                return null;

            if (request.ID_Conversation != conversationId)
                return null;

            var currentUser = GetCurrentUser();

            //Current user must already be a participant
            bool currentUserIsParticipant = DbContext.ConversationParticipants.Any(cp =>
                cp.ID_Conversation == conversationId &&
                cp.ID_User == currentUser.ID_User);

            if (!currentUserIsParticipant)
                return null;

            //User must exist
            bool userExists = DbContext.Users.Any(u => u.ID_User == request.ID_User);
            if (!userExists)
                return null;

            //Cannot add same user twice
            bool alreadyParticipant = DbContext.ConversationParticipants.Any(cp =>
                cp.ID_Conversation == conversationId &&
                cp.ID_User == request.ID_User);

            if (alreadyParticipant)
                return null;

            var participant = new DbConversationParticipant
            {
                ID_Conversation = conversationId,
                ID_User = request.ID_User.Value,
                Joined_At = request.Joined_At
            };

            DbContext.ConversationParticipants.Add(participant);
            DbContext.SaveChanges();

            return participant.ID_Conversation_Participant;
        }
        public string? GetConversationName(int conversationId)
        {
            var currentUser = GetCurrentUser();

            bool isParticipant = DbContext.ConversationParticipants.Any(cp =>
                cp.ID_Conversation == conversationId &&
                cp.ID_User == currentUser.ID_User);

            if (!isParticipant)
                return null;

            return DbContext.Conversations
                .Where(c => c.ID_Conversation == conversationId)
                .Select(c => c.Conversation_Name)
                .FirstOrDefault();
        }
        public bool UpdateConversationName(int conversationId, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return false;

            var currentUser = GetCurrentUser();

            bool isParticipant = DbContext.ConversationParticipants.Any(cp =>
                cp.ID_Conversation == conversationId &&
                cp.ID_User == currentUser.ID_User);

            if (!isParticipant)
                return false;

            var conversation = DbContext.Conversations
                .FirstOrDefault(c => c.ID_Conversation == conversationId);

            if (conversation == null)
                return false;

            conversation.Conversation_Name = newName.Trim();

            DbContext.SaveChanges();

            return true;
        }
    }
}
