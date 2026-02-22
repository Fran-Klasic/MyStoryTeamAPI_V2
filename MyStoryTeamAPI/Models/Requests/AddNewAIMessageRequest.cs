namespace MyStoryTeamAPI.Models.Requests
{
    public class AddNewAIMessageRequest
    {
        public int ID_AI_Conversation { get; set; }
        public string? Content { get; set; }
        public string? Type { get; set; }
    }
}
