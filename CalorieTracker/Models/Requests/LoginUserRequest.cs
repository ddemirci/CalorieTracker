namespace CalorieTracker.Models.Requests
{
    public class LoginUserRequest
    {
        /// <summary>
        /// Username or email
        /// </summary>
        public string Username { get; set; }
        public string Password { get; set; }
    }
}