namespace Server.Common
{
    public class ValidationConstants
    {
        public const int UsernameMaxLength = 15;
        public const int UsernameMinLength = 5;
        public const int CommentsContentMinLength = 5;
        public const int CommentsContentMaxLength = 400;
        public const int Min = 3;
        public const int Max = 400;
        public const int MinContentLength = 5;
        public const int MaxContentLength = 500;
    }

    public class ServerPathConstants
    {
        public const string ImageDirectory = "/Images/";
        public const string CommonImageName = "avatar.";
        public const string DefaultName = "default.gif";

        public const string defaultImagePath = "Images/default.gif";
        public const string imagePath = "Images/{0}";
    }

    public class SearchPatternsConstats
    {
        public const string Username = "User";
        public const string TopicTitle = "Topic Title";
        public const string TopicName = "Topic Name";
    }
}
