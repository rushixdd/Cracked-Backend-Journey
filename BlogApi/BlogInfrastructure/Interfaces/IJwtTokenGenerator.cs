﻿namespace BlogInfrastructure.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string userId, string email);
    }
}
