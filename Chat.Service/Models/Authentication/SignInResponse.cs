﻿namespace Chat.Service.Models.Authentication
{
    public class AuthReponse
    {
        public TokenType AccessToken { get; set; } = null!;
        public TokenType RefreshToken { get; set; } = null!;

    }
}
